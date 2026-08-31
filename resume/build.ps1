<#
.SYNOPSIS
  Compiles the LaTeX resumes and copies the PDFs into wwwroot, where the site
  serves them from. Compiling without the copy is how the site goes stale.

.PARAMETER Name
  Which resumes to build. Partial names work: "phd" matches resume-phd.
  Builds all of them when omitted.

.PARAMETER NoSync
  Compile only; leave wwwroot alone.

.EXAMPLE
  .\build.ps1
  .\build.ps1 phd
  .\build.ps1 resume-aiml -NoSync
#>
[CmdletBinding()]
param(
  [Parameter(Position = 0, ValueFromRemainingArguments = $true)]
  [string[]]$Name,

  [switch]$NoSync
)

$resumeDir = $PSScriptRoot
$wwwroot = Join-Path (Split-Path $resumeDir -Parent) 'src\SGM.WebApp\wwwroot'

if (-not (Get-Command latexmk -ErrorAction SilentlyContinue)) {
  throw "latexmk not found on PATH. Install MiKTeX (or TeX Live) first."
}

$sources = Get-ChildItem -Path $resumeDir -Filter '*.tex' | Sort-Object Name

if ($Name) {
  $pattern = ($Name | ForEach-Object { [regex]::Escape($_) }) -join '|'
  $sources = @($sources | Where-Object { $_.BaseName -match $pattern })

  if (-not $sources) {
    throw "No .tex file in $resumeDir matched: $($Name -join ', ')"
  }
}

$failed = @()

foreach ($source in $sources) {
  $base = $source.BaseName
  Write-Host "==> $($source.Name)" -ForegroundColor Cyan

  # -halt-on-error so a broken macro fails loudly instead of scrolling past;
  # -quiet keeps the package-loading wall out of the way, errors still print.
  & latexmk -pdf -quiet -interaction=nonstopmode -halt-on-error `
    -outdir="$resumeDir" "$($source.FullName)" | Out-Null

  if ($LASTEXITCODE -ne 0) {
    $failed += $base
    Write-Host "    FAILED - see $base.log" -ForegroundColor Red
    continue
  }

  # Only PDFs the site already serves get synced; drafts stay in resume/.
  $target = Join-Path $wwwroot "$base.pdf"
  if (-not $NoSync -and (Test-Path $target)) {
    Copy-Item (Join-Path $resumeDir "$base.pdf") $target -Force
    Write-Host "    synced -> wwwroot\$base.pdf" -ForegroundColor DarkGray
  }
}

# Drop .aux/.fls/.fdb_latexmk. Its exit code is not a build result, so ignore it.
& latexmk -c -quiet -outdir="$resumeDir" | Out-Null

if ($failed) {
  Write-Host "Failed to build: $($failed -join ', ')" -ForegroundColor Red
  exit 1
}

Write-Host "Done." -ForegroundColor Green
exit 0
