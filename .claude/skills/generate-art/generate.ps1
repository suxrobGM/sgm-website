<#
.SYNOPSIS
  Generates one image with Codex CLI's imagegen skill, then optionally resizes/crops/converts it.

.EXAMPLE
  # PNG only, kept in $env:TEMP/generate-art
  ./.claude/skills/generate-art/generate.ps1 -Prompt "a watercolor fox" -Size square

  # Style reference attached, written as a 1000px WebP
  ./.claude/skills/generate-art/generate.ps1 -PromptFile p.txt -Image ref.webp -Out wwwroot/images/x.webp -Width 1000

  # Scaled to 1200 wide and center-cropped to 630 tall (link-preview card)
  ./.claude/skills/generate-art/generate.ps1 -PromptFile p.txt -Out og.jpg -Width 1200 -Height 630
#>
param(
  # The image description. Refer to attachments as "the first attached image", etc.
  [string]$Prompt,
  [string]$PromptFile,
  # Reference images to attach (style, character, layout). WebP is converted to PNG first.
  [string[]]$Image = @(),
  [ValidateSet('square', 'landscape', 'portrait')] [string]$Size = 'landscape',
  # Final file (.webp, .jpg or .png). Omit to keep only the raw PNG.
  [string]$Out,
  # Resize to this width. With -Height too, the image is center-cropped to that height.
  [int]$Width,
  [int]$Height
)

$ErrorActionPreference = 'Stop'
if ($PromptFile) { $Prompt = Get-Content $PromptFile -Raw }
if (-not $Prompt) { throw 'Pass -Prompt or -PromptFile.' }

$work = Join-Path $env:TEMP 'generate-art'
New-Item -ItemType Directory -Force $work | Out-Null
$name = if ($Out) { [IO.Path]::GetFileNameWithoutExtension($Out) } else { 'image-' + (Get-Date -Format 'yyyyMMdd-HHmmss') }

# Codex only takes PNG/JPG attachments.
$attachments = for ($i = 0; $i -lt $Image.Count; $i++) {
  $src = (Resolve-Path $Image[$i]).Path
  if ($src -match '\.webp$') {
    $png = Join-Path $work "$name.ref$i.png"
    ffmpeg -v error -y -i $src $png
    $png
  } else { $src }
}

$dims = @{ square = '1024x1024'; landscape = '1536x1024'; portrait = '1024x1536' }[$Size]
$promptPath = Join-Path $work "$name.prompt.txt"
$log = Join-Path $work "$name.log"
Set-Content -Path $promptPath -Encoding utf8 -Value @"
Use the imagegen skill to generate one image ($Size $dims). Just generate it; do not try to save it anywhere.

$Prompt
"@

$codexArgs = @('exec', '--skip-git-repo-check') + ($attachments | ForEach-Object { '-i', "`"$_`"" }) + '-'
Write-Host "Generating $name..."
Start-Process codex -ArgumentList $codexArgs -NoNewWindow -Wait `
  -RedirectStandardInput $promptPath -RedirectStandardOutput $log -RedirectStandardError "$log.err"

# Codex's sandbox can't write files and doesn't reliably print the output path,
# but each image lands in a folder named after the session id.
$session = Select-String -Path $log, "$log.err" -Pattern 'session id: ([0-9a-f-]+)' |
  Select-Object -First 1 | ForEach-Object { $_.Matches[0].Groups[1].Value }
$generated = if ($session) { Get-ChildItem "$HOME/.codex/generated_images/$session/*.png" -ErrorAction SilentlyContinue | Select-Object -First 1 }
if (-not $generated) { throw "No image found for $name. See $log and $log.err" }

$raw = Join-Path $work "$name.png"
Copy-Item $generated.FullName $raw -Force
Write-Host "PNG: $raw"
if (-not $Out) { return }

$filters = @()
if ($Width) { $filters += "scale=${Width}:-2" }
if ($Width -and $Height) { $filters += "crop=${Width}:${Height}" }
$ffArgs = @('-v', 'error', '-y', '-i', $raw)
if ($filters) { $ffArgs += @('-vf', ($filters -join ',')) }
switch -Regex ($Out) {
  '\.webp$' { $ffArgs += @('-c:v', 'libwebp', '-quality', '82') }
  '\.jpe?g$' { $ffArgs += @('-q:v', '3') }
}
New-Item -ItemType Directory -Force (Split-Path -Parent ([IO.Path]::GetFullPath($Out))) | Out-Null
ffmpeg @ffArgs $Out
Write-Host "Wrote $Out"
