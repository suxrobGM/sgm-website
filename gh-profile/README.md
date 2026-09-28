<div align="center">

<!-- Animated Header Banner -->
<img src="https://capsule-render.vercel.app/api?type=waving&color=0:0d1117,50:161b22,100:1f6feb&height=220&section=header&text=Sukhrob%20Ilyosbekov&fontSize=42&fontColor=ffffff&animation=fadeIn&fontAlignY=35&desc=AI%20Research%20%C2%B7%20Computer%20Vision%20%C2%B7%20Multimodal%20ML&descSize=18&descAlignY=55&descColor=8b949e" width="100%"/>

<!-- Terminal-Style Introduction -->
<img src="./assets/terminal-intro.svg" alt="Terminal Introduction" width="800"/>

<!-- Badge Row -->
[![Google Scholar](https://img.shields.io/badge/Google_Scholar-4285F4?style=for-the-badge&logo=googlescholar&logoColor=white)](https://scholar.google.com/citations?user=p7ujRHoAAAAJ&hl=en)
[![Research](https://img.shields.io/badge/Publications-b31b1b?style=for-the-badge&logo=arxiv&logoColor=white)](https://suxrobgm.net/research)
[![Hugging Face](https://img.shields.io/badge/Hugging_Face-FFD21E?style=for-the-badge&logo=huggingface&logoColor=black)](https://huggingface.co/suxrobgm)
[![Portfolio](https://img.shields.io/badge/suxrobgm.net-000?style=for-the-badge&logo=vercel&logoColor=white)](https://suxrobgm.net)
[![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/suxrobgm)
[![Telegram](https://img.shields.io/badge/Telegram-2CA5E0?style=for-the-badge&logo=telegram&logoColor=white)](https://t.me/suxrobgm)
[![Buy Me a Coffee](https://img.shields.io/badge/Buy_Me_a_Coffee-FFDD00?style=for-the-badge&logo=buymeacoffee&logoColor=black)](https://buymeacoffee.com/suxrobgm)

</div>

---

<div align="center">

## Primary Stack &nbsp;·&nbsp; AI Research

<img src="https://skillicons.dev/icons?i=pytorch,python,opencv,tensorflow,anaconda&theme=dark" alt="AI/ML core" width="440"/>

![Hugging Face](https://img.shields.io/badge/Hugging_Face-FFD21E?style=flat-square&logo=huggingface&logoColor=black)
![CUDA](https://img.shields.io/badge/CUDA-76B900?style=flat-square&logo=nvidia&logoColor=white)
![NumPy](https://img.shields.io/badge/NumPy-013243?style=flat-square&logo=numpy&logoColor=white)
![scikit-learn](https://img.shields.io/badge/scikit--learn-F7931E?style=flat-square&logo=scikitlearn&logoColor=white)
![Weights & Biases](https://img.shields.io/badge/W%26B-FFBE00?style=flat-square&logo=weightsandbiases&logoColor=black)
![ONNX](https://img.shields.io/badge/ONNX-005CED?style=flat-square&logo=onnx&logoColor=white)

`vision-language models` &nbsp; `contrastive learning` &nbsp; `explainable AI` &nbsp; `medical imaging` &nbsp; `diffusion & inpainting`

<br/>

### Secondary Stack &nbsp;·&nbsp; Software Engineering

<small>Nine years of production work. These days I mostly use it to ship research.</small>

<img src="https://skillicons.dev/icons?i=cs,ts,cpp,kotlin,fastapi,nodejs,bun&theme=dark" alt="Languages and backend" width="330"/>
<br/>
<img src="https://skillicons.dev/icons?i=react,nextjs,angular,postgres,redis,docker,kubernetes,aws&theme=dark" alt="Frontend, data and cloud" width="376"/>

</div>

---

## Research

I do computer vision research, mostly for medicine and biology. Abstracts and BibTeX are on
**[suxrobgm.net/research](https://suxrobgm.net/research)**, citations on
[Google Scholar](https://scholar.google.com/citations?user=p7ujRHoAAAAJ&hl=en).

### Publications

> ### MorphoCLIP
>
> [![arXiv](https://img.shields.io/badge/arXiv-2608.22690-b31b1b?style=flat-square&logo=arxiv)](https://arxiv.org/abs/2608.22690)
> [![Code](https://img.shields.io/badge/-Code-181717?style=flat-square&logo=github)](https://github.com/suxrobgm/morphoclip)
> [![Data](https://img.shields.io/badge/%F0%9F%A4%97_Data-FFD21E?style=flat-square)](https://huggingface.co/datasets/suxrobgm/cpjump1-dinov3-features)
>
> Matches microscope images of treated cells to a plain-language description of the drug or gene behind the change. The encoders stay frozen and only small heads train, so one consumer GPU is enough.
>
> <small>**CPJUMP1** · 51 plates · 3M+ images</small>
>
> ![PyTorch](https://img.shields.io/badge/PyTorch-EE4C2C?style=flat-square&logo=pytorch&logoColor=white)
> ![Contrastive](https://img.shields.io/badge/Contrastive-333?style=flat-square)

> ### Localize, Don't Beautify
>
> [![arXiv](https://img.shields.io/badge/arXiv-2608.02841-b31b1b?style=flat-square&logo=arxiv)](https://arxiv.org/abs/2608.02841)
> [![Code](https://img.shields.io/badge/-Code-181717?style=flat-square&logo=github)](https://github.com/suxrobGM/localize-dont-beautify)
>
> Commercial image editors asked to change one facial feature tend to retouch the whole face. I compared three ways to keep the edit local, and plain masking did better than prompt-only steering.
>
> <small>**6 editors** · 196 edits · identity scored</small>
>
> ![ArcFace](https://img.shields.io/badge/ArcFace-333?style=flat-square)
> ![Inpainting](https://img.shields.io/badge/Inpainting-333?style=flat-square)

> ### MelanomaNet
>
> [![arXiv](https://img.shields.io/badge/arXiv-2512.09289-b31b1b?style=flat-square&logo=arxiv)](https://arxiv.org/abs/2512.09289)
> [![Code](https://img.shields.io/badge/-Code-181717?style=flat-square&logo=github)](https://github.com/suxrobgm/explainable-melanoma)
>
> Sorts skin lesions into all nine ISIC 2019 classes, then checks the model's GradCAM++ attention against the ABCDE criteria dermatologists already use and scores how well the two agree.
>
> <small>**ISIC 2019** · 25K images · 0.86 F1</small>
>
> ![PyTorch](https://img.shields.io/badge/PyTorch-EE4C2C?style=flat-square&logo=pytorch&logoColor=white)
> ![GradCAM](https://img.shields.io/badge/GradCAM++-333?style=flat-square)

### Vision Projects

> ### [Med Image Scanner](https://github.com/suxrobgm/med-image-scanner)
>
> Pulls studies from a hospital's PACS over DICOM and runs detection models on them. Predictions appear as overlays in the viewer next to the usual measurement and segmentation tools, and the whole thing is built for HIPAA.
>
> ![Python](https://img.shields.io/badge/Python-3776AB?style=flat-square&logo=python&logoColor=white)
> ![PyTorch](https://img.shields.io/badge/PyTorch-EE4C2C?style=flat-square&logo=pytorch&logoColor=white)
> ![OpenCV](https://img.shields.io/badge/OpenCV-5C3EE8?style=flat-square&logo=opencv&logoColor=white)
> ![Next.js](https://img.shields.io/badge/Next.js-000?style=flat-square&logo=nextdotjs&logoColor=white)

> ### [Bookshelf Scanner](https://github.com/suxrobgm/bookshelf-scanner)
>
> Snap a photo of a bookshelf and get back a list of the books. YOLO picks out each spine and a vision-language model reads the title and author.
>
> ![Python](https://img.shields.io/badge/Python-3776AB?style=flat-square&logo=python&logoColor=white)
> ![YOLO](https://img.shields.io/badge/YOLO-00FFFF?style=flat-square&logoColor=black)
> ![FastAPI](https://img.shields.io/badge/FastAPI-009688?style=flat-square&logo=fastapi&logoColor=white)
> ![Angular](https://img.shields.io/badge/Angular-DD0031?style=flat-square&logo=angular&logoColor=white)

> ### [LightDepth](https://github.com/suxrobgm/lightdepth)
>
> Depth from a single image in **14.3M parameters**, against 24.8M for Depth Anything V2. It runs **72% faster** and has slightly lower relative error on NYU Depth V2.
>
> ![PyTorch](https://img.shields.io/badge/PyTorch-EE4C2C?style=flat-square&logo=pytorch&logoColor=white)
> ![Model](https://img.shields.io/badge/ResNet18_+_UNet-333?style=flat-square)

> ### [FSRCNN](https://github.com/suxrobgm/fsrcnn)
>
> My reimplementation of FSRCNN (Dong et al., ECCV 2016) for 2x/3x/4x super-resolution. Learning the upsampling end to end is what makes it **40x faster** than SRCNN, and it still gains +1.78 dB PSNR on Set5.
>
> ![PyTorch](https://img.shields.io/badge/PyTorch-EE4C2C?style=flat-square&logo=pytorch&logoColor=white)
> ![AMP](https://img.shields.io/badge/Mixed_Precision-333?style=flat-square)

---

## Software Engineering

<div align="center">

![Users](https://img.shields.io/badge/Meat.gg-60K%2B_users-1f6feb?style=for-the-badge&logo=counterstrike&logoColor=white)
![Projects](https://img.shields.io/badge/Shipped-10%2B_projects-1f6feb?style=for-the-badge&logo=rocket&logoColor=white)
![Upwork](https://img.shields.io/badge/Upwork-100%25_job_success-1f6feb?style=for-the-badge&logo=upwork&logoColor=white)

</div>

> ### [LogisticsX](https://logisticsx.app)
>
> [![Source](https://img.shields.io/badge/-Source-181717?style=flat-square&logo=github)](https://github.com/suxrobgm/logistics-app)
>
> Multi-tenant system for running an intermodal trucking company. A multi-provider LLM agent handles dispatch, and the platform plugs into DAT and Truckstop, tracks ELD hours-of-service, plans routes, shows trucks live, and handles payouts through Stripe Connect. Built on DDD and CQRS.
>
> ![C#](https://img.shields.io/badge/C%23-512BD4?style=flat-square&logo=dotnet&logoColor=white)
> ![Angular](https://img.shields.io/badge/Angular-DD0031?style=flat-square&logo=angular&logoColor=white)
> ![Kotlin](https://img.shields.io/badge/Kotlin_KMP-7F52FF?style=flat-square&logo=kotlin&logoColor=white)
> ![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=flat-square&logo=postgresql&logoColor=white)
> ![Docker](https://img.shields.io/badge/Docker-2496ED?style=flat-square&logo=docker&logoColor=white)

> ### [Meat.gg](https://meat.gg)
>
> `60K+ users` &nbsp; `1K+ DAU`
>
> Community site for Counter-Strike 2 servers, with profiles, messaging, and a Stripe shop. A native server plugin lets admins ban players, handle reports, and moderate without leaving the game.
>
> ![Next.js](https://img.shields.io/badge/Next.js-000?style=flat-square&logo=nextdotjs&logoColor=white)
> ![Bun](https://img.shields.io/badge/Bun-000?style=flat-square&logo=bun&logoColor=white)
> ![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=flat-square&logo=postgresql&logoColor=white)
> ![Stripe](https://img.shields.io/badge/Stripe-635BFF?style=flat-square&logo=stripe&logoColor=white)
> ![Docker](https://img.shields.io/badge/Docker-2496ED?style=flat-square&logo=docker&logoColor=white)

> ### [DepVault](https://depvault.com)
>
> [![Source](https://img.shields.io/badge/-Source-181717?style=flat-square&logo=github)](https://github.com/suxrobGM/depvault)
>
> Checks a project's dependencies against OSV.dev for known vulnerabilities across **8+ ecosystems**. It's also an encrypted secrets vault (AES-256-GCM) with one-time sharing and a CLI that injects tokens into CI/CD.
>
> ![Next.js](https://img.shields.io/badge/Next.js-000?style=flat-square&logo=nextdotjs&logoColor=white)
> ![.NET](https://img.shields.io/badge/.NET_AOT-512BD4?style=flat-square&logo=dotnet&logoColor=white)
> ![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=flat-square&logo=postgresql&logoColor=white)
> ![Docker](https://img.shields.io/badge/Docker-2496ED?style=flat-square&logo=docker&logoColor=white)

> ### [Blazor Form Builder](https://github.com/suxrobgm/blazor-form-builder)
>
> Drag-and-drop form designer for Blazor. Forms are saved as JSON schema and rendered at runtime, so admin dashboards don't need hand-written forms.
>
> ![C#](https://img.shields.io/badge/C%23-512BD4?style=flat-square&logo=dotnet&logoColor=white)
> ![Blazor](https://img.shields.io/badge/Blazor-512BD4?style=flat-square&logo=blazor&logoColor=white)
> ![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white)

---

## Games

<table>
<tr>
<td width="50%" valign="top" align="center">

[![Hearts of Iron IV: Economic Crisis](./assets/hoi-4-ec.jpg)](https://steamcommunity.com/sharedfiles/filedetails/?id=2000532465)

**Hearts of Iron IV: Economic Crisis**

Big overhaul mod I led, with its own mechanics, AI behavior, and balance changes.

[Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=2000532465) · [Releases](https://github.com/Economic-Crisis/Public-releases)

</td>
<td width="50%" valign="top" align="center">

[![Chestnut MMO](./assets/chestnut.jpg)](https://www.chest-nut.io)

**Chestnut (MMO)**

Real-time MMO with an authoritative server and custom physics that keeps 100+ players in sync. Has Web3 integration.

[Play](https://www.chest-nut.io)

</td>
</tr>
<tr>
<td width="50%" valign="top" align="center">

[![ChessMate](https://raw.githubusercontent.com/suxrobGM/online-chess/main/screenshots/screenshot-3.jpg)](https://github.com/suxrobGM/online-chess)

**ChessMate**

Online chess with AI opponents and rated or friendly PvP matchmaking.

[Repo](https://github.com/suxrobGM/online-chess)

</td>
<td width="50%" valign="top" align="center">

[![Maze 2D](https://raw.githubusercontent.com/suxrobGM/maze-godot/main/screenshots/game-scene.png)](https://github.com/suxrobGM/maze-godot)

**Maze**

2D puzzle game with AI pathfinding and level progression.

[Repo](https://github.com/suxrobGM/maze-godot)

</td>
</tr>
</table>

---

<div align="center">

## GitHub Activity

<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./assets/activity-dark.svg">
  <img src="./assets/activity-light.svg" alt="Contributions per month, one row per year" width="680"/>
</picture>

<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./assets/cumulative-dark.svg">
  <img src="./assets/cumulative-light.svg" alt="Running total of contributions" width="680"/>
</picture>

<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./assets/mix-dark.svg">
  <img src="./assets/mix-light.svg" alt="Contributions per year split into commits, pull requests, issues and reviews" width="680"/>
</picture>

<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./assets/languages-dark.svg">
  <img src="./assets/languages-light.svg" alt="Share of code by language in repositories started each year" width="680"/>
</picture>

<small>All four regenerate daily from the GitHub GraphQL API via <a href="./scripts/activity_chart.py">activity_chart.py</a>.</small>

<br/><br/>

![Metrics](./github-metrics.svg)

</div>

---

<div align="center">

### Get in Touch

I'm open to research collaborations and PhD-adjacent work. Also happy to talk computer vision, .NET, TypeScript, or game dev.

[![Google Scholar](https://img.shields.io/badge/Google_Scholar-4285F4?style=for-the-badge&logo=googlescholar&logoColor=white)](https://scholar.google.com/citations?user=p7ujRHoAAAAJ&hl=en)
[![Hugging Face](https://img.shields.io/badge/Hugging_Face-FFD21E?style=for-the-badge&logo=huggingface&logoColor=black)](https://huggingface.co/suxrobgm)
[![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/suxrobgm)
[![Portfolio](https://img.shields.io/badge/Portfolio-000?style=for-the-badge&logo=vercel&logoColor=white)](https://suxrobgm.net)
[![Telegram](https://img.shields.io/badge/Telegram-2CA5E0?style=for-the-badge&logo=telegram&logoColor=white)](https://t.me/suxrobgm)
[![Email](https://img.shields.io/badge/Email-EA4335?style=for-the-badge&logo=gmail&logoColor=white)](mailto:silyosbekov@gmail.com)

</div>

<!-- Footer Wave -->
<img src="https://capsule-render.vercel.app/api?type=waving&color=0:0d1117,50:161b22,100:1f6feb&height=120&section=footer" width="100%"/>
