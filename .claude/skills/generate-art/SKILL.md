---
name: generate-art
description: Generate images (illustrations, card art, hero images, link-preview cards, icons, backgrounds) in any style with Codex CLI's imagegen skill, optionally from style or character reference images, and save them resized as WebP/JPG/PNG. Use whenever the user wants new artwork or wants existing artwork redone.
---

# Generate art

`generate.ps1` (next to this file) sends a prompt and any reference images to `codex exec`,
picks the PNG out of `~/.codex/generated_images/<session id>/`, and optionally resizes, crops
and converts it.

```powershell
$g = './.claude/skills/generate-art/generate.ps1'
& $g -Prompt "..." -Size square                                   # raw PNG in $env:TEMP/generate-art
& $g -PromptFile p.txt -Image style.webp -Out path/x.webp -Width 1000
& $g -PromptFile p.txt -Image style.webp,person.jpg -Out og.jpg -Width 1200 -Height 630
```

- `-Size` is `square` (1024x1024), `landscape` (1536x1024) or `portrait` (1024x1536).
- `-Width` scales the image. Add `-Height` to center-crop to that height after scaling.
- The output format comes from the `-Out` extension. Without `-Out` you get only the PNG.
- Each image takes a minute or two. For a batch, start several runs as separate background
  commands; five at once works fine.

## Writing the prompt

1. **Say what each attachment is for.** For example: "The first attached image is a STYLE
   reference only: match its art style and palette." Or: "The second attached image is the
   CHARACTER reference: draw that exact person." Without that, the model may copy the people
   or layout from a style reference. If you don't want them, say so: "Do NOT include the man
   from the reference."
2. **Describe the style in words too** (medium, line work, palette, lighting), even when a
   reference is attached.
3. **Name the focal point** and say what's on any screens or signs. Turn the abstract idea into
   something you can see.
4. **Composition** goes in the prompt: where the subject sits, what has to stay clear for
   overlays or cropping.
5. **Ban text** unless you need it ("No readable text, no logos, no letters anywhere").
   Generated lettering usually comes out garbled.

## Checking results

Look at every image before using it, and zoom in on the key detail at full size
(`ffmpeg -i x.png -vf crop=w:h:x:y crop.jpg`). Small previews can hide a missing element. A
first attempt sometimes drops the most important detail. When that happens, regenerate with it
stated more forcefully ("... must be clearly visible and is the key detail"). Never commit the
raw PNGs.

## This site's presets

Art lives in `src/SGM.WebApp/wwwroot/images/`. Vice City art goes in `vc/`, named by the item's
`Slug` from `Data/PortfolioData.cs`.

| Use | Size | Output |
| --- | --- | --- |
| Mission (job) card | square | `vc/mission-<slug>.webp`, `-Width 640` |
| Property (project) card | landscape | `vc/property-<slug>.webp`, `-Width 1000` |
| Vice City hero | landscape | `vc/vc-hero-desktop.webp`, `-Width 1536` |
| Link preview | landscape | `og/<page>.jpg`, `-Width 1200 -Height 630` |

**Vice City style.** Attach `vc/property-medscanner.webp` (landscape) or `vc/mission-emtech.webp`
(square) as the style reference, and describe it as "1980s Vice City / Miami synthwave
illustration, bold ink outlines, saturated pink-orange sunset, teal and magenta neon, palm
trees, glossy reflective surfaces".

- Leave Sukhrob out of mission and property cards. They show the work, not him; repeating his
  portrait made every card look the same. For the hero and OG images, attach
  `images/myself-vc.jpg` as the character reference.
- When a project has a `Screenshot`, the screenshot covers about the bottom-right 42% x 40% of
  the card art. Keep the subject in the left half and center.
- On phones, mission art is cropped to 16:10, so keep the subject in the middle band.
- For OG cards, keep text and faces out of the top and bottom 110px of the 1024px source, since
  the 630px crop cuts them off. They're JPG because some scrapers skip WebP. Each page adds its
  card with `<OgImage>` inside `<HeadContent>`.
