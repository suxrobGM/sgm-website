<div align="center">

<picture>
  <source media="(max-width: 600px)" srcset="./assets/terminal-intro-mobile.svg">
  <img src="./assets/terminal-intro.svg" alt="Sukhrob Ilyosbekov: computer vision and multimodal ML researcher, MS CS at Northeastern, software engineer" width="800"/>
</picture>

<br/>

[![Google Scholar](https://img.shields.io/badge/Google_Scholar-4285F4?style=for-the-badge&logo=googlescholar&logoColor=white)](https://scholar.google.com/citations?user=p7ujRHoAAAAJ&hl=en)
[![Publications](https://img.shields.io/badge/Publications-b31b1b?style=for-the-badge&logo=arxiv&logoColor=white)](https://suxrobgm.net/research)
[![Hugging Face](https://img.shields.io/badge/Hugging_Face-FFD21E?style=for-the-badge&logo=huggingface&logoColor=black)](https://huggingface.co/suxrobgm)
[![Portfolio](https://img.shields.io/badge/suxrobgm.net-000?style=for-the-badge&logo=vercel&logoColor=white)](https://suxrobgm.net)
[![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/suxrobgm)
[![Email](https://img.shields.io/badge/Email-EA4335?style=for-the-badge&logo=gmail&logoColor=white)](mailto:silyosbekov@gmail.com)

</div>

## Research

I do computer vision research, mostly for medicine and biology. Abstracts and BibTeX are on
**[suxrobgm.net/research](https://suxrobgm.net/research)**.

> <img align="right" width="38%" src="./assets/paper-morphoclip.webp" alt="PCA of MorphoCLIP image features, colored by microscopy channel"/>
>
> ### [MorphoCLIP](https://arxiv.org/abs/2608.22690)
>
> [![arXiv](https://img.shields.io/badge/arXiv-2608.22690-b31b1b?style=flat-square&logo=arxiv)](https://arxiv.org/abs/2608.22690)
> [![Code](https://img.shields.io/badge/-Code-181717?style=flat-square&logo=github)](https://github.com/suxrobgm/morphoclip)
> [![Data](https://img.shields.io/badge/%F0%9F%A4%97_Data-FFD21E?style=flat-square)](https://huggingface.co/datasets/suxrobgm/cpjump1-dinov3-features)
>
> Matches microscope images of treated cells to a plain-language description of the drug or gene behind the change. The encoders stay frozen and only small heads train, so one consumer GPU is enough.
>
> <small>**CPJUMP1** · 51 plates · 3M+ images</small><br clear="right"/>

> <img align="right" width="38%" src="./assets/paper-localize.webp" alt="One face edited three ways: input, prompt only, and masked composite"/>
>
> ### [Localize, Don't Beautify](https://arxiv.org/abs/2608.02841)
>
> [![arXiv](https://img.shields.io/badge/arXiv-2608.02841-b31b1b?style=flat-square&logo=arxiv)](https://arxiv.org/abs/2608.02841)
> [![Code](https://img.shields.io/badge/-Code-181717?style=flat-square&logo=github)](https://github.com/suxrobGM/localize-dont-beautify)
>
> Commercial image editors asked to change one facial feature tend to retouch the whole face. I compared three ways to keep the edit local, and plain masking did better than prompt-only steering.
>
> <small>**6 editors** · 196 edits · identity scored</small><br clear="right"/>

> <img align="right" width="38%" src="./assets/paper-melanoma.webp" alt="A skin lesion with its GradCAM++ heatmap and ABCDE criteria checks"/>
>
> ### [MelanomaNet](https://arxiv.org/abs/2512.09289)
>
> [![arXiv](https://img.shields.io/badge/arXiv-2512.09289-b31b1b?style=flat-square&logo=arxiv)](https://arxiv.org/abs/2512.09289)
> [![Code](https://img.shields.io/badge/-Code-181717?style=flat-square&logo=github)](https://github.com/suxrobgm/explainable-melanoma)
>
> Sorts skin lesions into all nine ISIC 2019 classes, then checks the model's GradCAM++ attention against the ABCDE criteria dermatologists already use and scores how well the two agree.
>
> <small>**ISIC 2019** · 25K images</small><br clear="right"/>

## Selected work

> <img align="right" width="38%" src="./assets/project-jobpilot.webp" alt="JobPilot submitting an application in a real browser"/>
>
> ### [JobPilot](https://github.com/suxrobGM/jobpilot)
>
> An AI agent that applies to jobs for you. It finds openings, tailors your resume, and fills out applications in a real browser on your own machine, running on your Claude Code or Codex subscription.
>
> <small>TypeScript · Playwright · [jobpilot.suxrobgm.net](https://jobpilot.suxrobgm.net)</small><br clear="right"/>

> <img align="right" width="38%" src="./assets/project-logisticsx.webp" alt="LogisticsX AI dispatcher proposing load assignments"/>
>
> ### [LogisticsX](https://logisticsx.app)
>
> Runs an intermodal trucking company end to end. An LLM agent handles dispatch, and the platform plugs into DAT and Truckstop, tracks hours of service, plans routes, shows trucks live, and pays drivers through Stripe Connect.
>
> <small>C# · Angular · Kotlin Multiplatform · PostgreSQL · [source](https://github.com/suxrobgm/logistics-app)</small><br clear="right"/>

> <img align="right" width="38%" src="./assets/project-meatgg.webp" alt="Meat.gg server list"/>
>
> ### [Meat.gg](https://meat.gg)
>
> Community site for Counter-Strike 2 servers with 60K+ users, with profiles, messaging and a Stripe shop. A native server plugin built on [voltmod](https://github.com/VoltyGames/voltmod) lets admins ban players and handle reports without leaving the game.
>
> <small>Next.js · Bun · C++23 · PostgreSQL</small><br clear="right"/>

> <img align="right" width="38%" src="./assets/project-depvault.webp" alt="DepVault dashboard"/>
>
> ### [DepVault](https://depvault.com)
>
> Checks a project's dependencies against OSV.dev for known vulnerabilities across 8+ ecosystems. It's also an encrypted secrets vault with one-time sharing and a CLI that injects tokens into CI/CD.
>
> <small>Next.js · .NET AOT · PostgreSQL · [source](https://github.com/suxrobGM/depvault)</small><br clear="right"/>

<details>
<summary><b>More projects</b></summary>

<br/>

**Vision**

- **[Med Image Scanner](https://github.com/suxrobgm/med-image-scanner)** pulls studies from a hospital's PACS over DICOM, runs detection models on them, and overlays the predictions in the viewer. Built for HIPAA.
- **[Bookshelf Scanner](https://github.com/suxrobgm/bookshelf-scanner)** turns a photo of a bookshelf into a list of books. YOLO finds each spine and a vision-language model reads the title and author.
- **[LightDepth](https://github.com/suxrobgm/lightdepth)** estimates depth from a single image with about half the parameters of Depth Anything V2, runs 72% faster, and has slightly lower error on NYU Depth V2.
- **[FSRCNN](https://github.com/suxrobgm/fsrcnn)** is my reimplementation of FSRCNN (Dong et al., ECCV 2016) for 2x, 3x and 4x super-resolution.

**Engineering**

- **[Blazor Form Builder](https://github.com/suxrobgm/blazor-form-builder)** is a drag-and-drop form designer. Forms are saved as JSON schema and rendered at runtime.
- **[voltmod](https://github.com/VoltyGames/voltmod)** is a native C++23 framework for writing Counter-Strike 2 server plugins.

</details>

<details>
<summary><b>Games</b></summary>

<br/>

<table>
<tr>
<td width="50%" valign="top">
<a href="https://steamcommunity.com/sharedfiles/filedetails/?id=2000532465"><img src="./assets/game-hoi4-ec.webp" alt="Hearts of Iron IV: Economic Crisis"/></a>
<br/><b><a href="https://steamcommunity.com/sharedfiles/filedetails/?id=2000532465">Hearts of Iron IV: Economic Crisis</a></b>
<br/><small>Overhaul mod I led, with its own mechanics, AI behavior and balance.</small>
</td>
<td width="50%" valign="top">
<a href="https://www.chest-nut.io"><img src="./assets/game-chestnut.webp" alt="Chestnut MMO"/></a>
<br/><b><a href="https://www.chest-nut.io">Chestnut</a></b>
<br/><small>Real-time MMO with an authoritative server that keeps 100+ players in sync.</small>
</td>
</tr>
<tr>
<td width="50%" valign="top">
<a href="https://github.com/suxrobGM/online-chess"><img src="./assets/game-chessmate.webp" alt="ChessMate"/></a>
<br/><b><a href="https://github.com/suxrobGM/online-chess">ChessMate</a></b>
<br/><small>Online chess with AI opponents and rated or friendly matchmaking.</small>
</td>
<td width="50%" valign="top">
<a href="https://github.com/suxrobGM/maze-godot"><img src="./assets/game-maze.webp" alt="Maze"/></a>
<br/><b><a href="https://github.com/suxrobGM/maze-godot">Maze</a></b>
<br/><small>2D puzzle game in Godot with AI pathfinding.</small>
</td>
</tr>
</table>

</details>

## Activity

<div align="center">

<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./assets/timeline-dark.svg">
  <img src="./assets/timeline-light.svg" alt="Contributions per month since 2019, with yearly totals" width="680"/>
</picture>

<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./assets/languages-dark.svg">
  <img src="./assets/languages-light.svg" alt="Languages in the repositories started each year" width="680"/>
</picture>

<small>Redrawn daily from the GitHub API by <a href="./scripts/activity_chart.py">activity_chart.py</a>.</small>

</div>

---

<div align="center">

Open to research collaborations and PhD-adjacent work, and happy to talk computer vision, .NET, TypeScript or game dev.

[Email](mailto:silyosbekov@gmail.com) · [Telegram](https://t.me/suxrobgm) · [LinkedIn](https://www.linkedin.com/in/suxrobgm) · [Buy me a coffee](https://buymeacoffee.com/suxrobgm)

</div>
