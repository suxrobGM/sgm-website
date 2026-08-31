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

<sub>Nine years of production work, now mostly in service of shipping research.</sub>

<img src="https://skillicons.dev/icons?i=cs,ts,cpp,kotlin,fastapi,nodejs,bun&theme=dark" alt="Languages and backend" width="330"/>
<br/>
<img src="https://skillicons.dev/icons?i=react,nextjs,angular,postgres,redis,docker,kubernetes,aws&theme=dark" alt="Frontend, data and cloud" width="376"/>

</div>

---

## Research

Three first-author papers in computer vision and multimodal ML.
Full list with abstracts and BibTeX: **[suxrobgm.net/research](https://suxrobgm.net/research)** ·
[Google Scholar](https://scholar.google.com/citations?user=p7ujRHoAAAAJ&hl=en)

### Publications

<table>
<tr>
<td width="33%">

<div align="center">

**MorphoCLIP**

[![arXiv](https://img.shields.io/badge/arXiv-2608.22690-b31b1b?style=flat-square&logo=arxiv)](https://arxiv.org/abs/2608.22690)
[![Code](https://img.shields.io/badge/-Code-181717?style=flat-square&logo=github)](https://github.com/suxrobgm/morphoclip)
[![Data](https://img.shields.io/badge/%F0%9F%A4%97_Data-FFD21E?style=flat-square)](https://huggingface.co/datasets/suxrobgm/cpjump1-dinov3-features)

</div>

Matches Cell Painting microscopy of perturbed cells to descriptions of the treatment in ordinary language. DINOv3 and BioClinical ModernBERT stay frozen and only small projection heads train, so it fits on one consumer GPU. Tested on **CPJUMP1** (51 plates, 3M+ images).

The precomputed DINOv3 embeddings are [on the Hub](https://huggingface.co/datasets/suxrobgm/cpjump1-dinov3-features) as WebDataset shards, so the expensive pass over the images does not have to be repeated.

![PyTorch](https://img.shields.io/badge/PyTorch-EE4C2C?style=flat-square&logo=pytorch&logoColor=white)
![Contrastive](https://img.shields.io/badge/Contrastive_Learning-333?style=flat-square)

</td>
<td width="33%">

<div align="center">

**Localize, Don't Beautify**

[![arXiv](https://img.shields.io/badge/arXiv-2608.02841-b31b1b?style=flat-square&logo=arxiv)](https://arxiv.org/abs/2608.02841)

</div>

Ask a commercial image editor to change one feature of a face and it beautifies the whole thing. Compares prompt-only steering, masked compositing, and inpainting across **six commercial editors** on 196 edits, scoring identity preservation and whether the edit stayed put. Plain masking localized better.

![ArcFace](https://img.shields.io/badge/ArcFace-333?style=flat-square)
![Inpainting](https://img.shields.io/badge/Inpainting-333?style=flat-square)

</td>
<td width="33%">

<div align="center">

**MelanomaNet**

[![arXiv](https://img.shields.io/badge/arXiv-2512.09289-b31b1b?style=flat-square&logo=arxiv)](https://arxiv.org/abs/2512.09289)
[![Code](https://img.shields.io/badge/-Code-181717?style=flat-square&logo=github)](https://github.com/suxrobgm/explainable-melanoma)

</div>

Explainable classification across all **9 ISIC 2019 categories** (0.856 weighted F1 on 25,331 images). GradCAM++ attention is broken down along the ABCDE criteria clinicians already use, then scored against those same features, so the interpretability claim rests on a metric.

![PyTorch](https://img.shields.io/badge/PyTorch-EE4C2C?style=flat-square&logo=pytorch&logoColor=white)
![GradCAM](https://img.shields.io/badge/GradCAM++-333?style=flat-square)

</td>
</tr>
</table>

### Applied Vision Work

Models built against real inputs rather than a clean benchmark split.

<table>
<tr>
<td width="50%">

<div align="center">

**[Med Image Scanner](https://github.com/suxrobgm/med-image-scanner)**

</div>

Pulls studies straight from hospital PACS over DICOM and runs detection models over them. Predictions show up as overlays in the viewer, alongside measurement and segmentation tools. HIPAA-ready.

![Python](https://img.shields.io/badge/Python-3776AB?style=flat-square&logo=python&logoColor=white)
![PyTorch](https://img.shields.io/badge/PyTorch-EE4C2C?style=flat-square&logo=pytorch&logoColor=white)
![OpenCV](https://img.shields.io/badge/OpenCV-5C3EE8?style=flat-square&logo=opencv&logoColor=white)
![Next.js](https://img.shields.io/badge/Next.js-000?style=flat-square&logo=nextdotjs&logoColor=white)

</td>
<td width="50%">

<div align="center">

**[Bookshelf Scanner](https://github.com/suxrobgm/bookshelf-scanner)**

</div>

Point a camera at a bookshelf and get back a list of what is on it. YOLO segmentation cuts out each spine, then a vision-language model reads the title and author off it.

![Python](https://img.shields.io/badge/Python-3776AB?style=flat-square&logo=python&logoColor=white)
![YOLO](https://img.shields.io/badge/YOLO-00FFFF?style=flat-square&logoColor=black)
![FastAPI](https://img.shields.io/badge/FastAPI-009688?style=flat-square&logo=fastapi&logoColor=white)
![Angular](https://img.shields.io/badge/Angular-DD0031?style=flat-square&logo=angular&logoColor=white)

</td>
</tr>
</table>

### Graduate Coursework

Kept separate from the published work above.

<table>
<tr>
<td width="50%">

<div align="center">

**[LightDepth](https://github.com/suxrobgm/lightdepth)**

</div>

Lightweight monocular depth estimation. Holds accuracy at **14.3M params** where Depth Anything V2 needs 24.8M, runs **72% faster**, and comes out slightly ahead on relative error on NYU Depth V2.

![PyTorch](https://img.shields.io/badge/PyTorch-EE4C2C?style=flat-square&logo=pytorch&logoColor=white)
![Model](https://img.shields.io/badge/ResNet18_+_UNet-333?style=flat-square)

</td>
<td width="50%">

<div align="center">

**[FSRCNN](https://github.com/suxrobgm/fsrcnn)**

</div>

Reproduction of FSRCNN (Dong et al., ECCV 2016) for super-resolution at 2x/3x/4x. Upsampling is learned end to end, which is where the **40x speedup** over SRCNN comes from (+1.78 dB PSNR on Set5).

![PyTorch](https://img.shields.io/badge/PyTorch-EE4C2C?style=flat-square&logo=pytorch&logoColor=white)
![AMP](https://img.shields.io/badge/Mixed_Precision-333?style=flat-square)

</td>
</tr>
</table>

---

## Software Engineering

<div align="center">

<table>
<tr>
<td align="center" width="33%">
<img width="60" src="https://img.icons8.com/fluency/96/user-group-man-man.png" alt="users"/>
<br/>
<strong>60K+</strong>
<br/>
<sub>Users on Meat.gg</sub>
</td>
<td align="center" width="33%">
<img width="60" src="https://img.icons8.com/fluency/96/rocket.png" alt="projects"/>
<br/>
<strong>10+</strong>
<br/>
<sub>Shipped Projects</sub>
</td>
<td align="center" width="33%">
<img width="60" src="https://img.icons8.com/fluency/96/checkmark.png" alt="jss"/>
<br/>
<strong>100%</strong>
<br/>
<sub>Upwork Job Success</sub>
</td>
</tr>
</table>

</div>

<table>
<tr>
<td width="50%">

<div align="center">

**[LogisticsX](https://logisticsx.app)** &nbsp; [![GitHub](https://img.shields.io/badge/-Source-181717?style=flat-square&logo=github)](https://github.com/suxrobgm/logistics-app)

</div>

Multi-tenant TMS for intermodal trucking. Wired into the big load boards (DAT, Truckstop), with ELD/HOS compliance, Stripe Connect, route optimization, and live tracking. **DDD + CQRS architecture.**

![C#](https://img.shields.io/badge/C%23-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![Angular](https://img.shields.io/badge/Angular-DD0031?style=flat-square&logo=angular&logoColor=white)
![Kotlin](https://img.shields.io/badge/Kotlin_KMP-7F52FF?style=flat-square&logo=kotlin&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=flat-square&logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=flat-square&logo=docker&logoColor=white)

</td>
<td width="50%">

<div align="center">

**[Meat.gg](https://meat.gg)** &nbsp; `60K+ users` `1K+ DAU`

</div>

Community platform for Counter-Strike 2 servers. Profiles and messaging, a shop running on Stripe, and a native plugin that lets admins ban, report, and moderate from inside the game.

![Next.js](https://img.shields.io/badge/Next.js-000?style=flat-square&logo=nextdotjs&logoColor=white)
![Bun](https://img.shields.io/badge/Bun-000?style=flat-square&logo=bun&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=flat-square&logo=postgresql&logoColor=white)
![Stripe](https://img.shields.io/badge/Stripe-635BFF?style=flat-square&logo=stripe&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=flat-square&logo=docker&logoColor=white)

</td>
</tr>
<tr>
<td width="50%">

<div align="center">

**[DepVault](https://depvault.com)** &nbsp; [![GitHub](https://img.shields.io/badge/-Source-181717?style=flat-square&logo=github)](https://github.com/suxrobGM/depvault)

</div>

Scans a project's dependencies across **8+ ecosystems** for known vulnerabilities via OSV.dev, and doubles as an encrypted secrets vault: AES-256-GCM, one-time secret sharing, CI/CD token injection.

![Next.js](https://img.shields.io/badge/Next.js-000?style=flat-square&logo=nextdotjs&logoColor=white)
![.NET](https://img.shields.io/badge/.NET_AOT-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=flat-square&logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=flat-square&logo=docker&logoColor=white)

</td>
<td width="50%">

<div align="center">

**[Blazor Form Builder](https://github.com/suxrobgm/blazor-form-builder)**

</div>

Drag-and-drop form designer that outputs JSON schema with a runtime renderer, so admin dashboards stop needing hand-written forms.

![C#](https://img.shields.io/badge/C%23-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![Blazor](https://img.shields.io/badge/Blazor-512BD4?style=flat-square&logo=blazor&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white)

</td>
</tr>
</table>

---

## Games

<table>
<tr>
<td width="50%">

<div align="center">

**Hearts of Iron IV: Economic Crisis**
[Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=2000532465) · [Releases](https://github.com/Economic-Crisis/Public-releases)

</div>

Large-scale mod with custom mechanics, AI behaviors, and balance systems.

<img src="./assets/hoi-4-ec.jpg" alt="HOI4 EC" width="100%"/>

</td>
<td width="50%">

<div align="center">

**Chestnut (MMO)**
[Play](https://www.chest-nut.io)

</div>

Real-time MMO with authoritative server, custom physics, and sync for **100+ concurrent players** with Web3 integration.

<img src="./assets/chestnut.jpg" alt="Chestnut MMO" width="100%"/>

</td>
</tr>
<tr>
<td width="50%">

<div align="center">

**ChessMate**
[Repo](https://github.com/suxrobGM/online-chess)

</div>

Online chess platform with AI opponents, rated/friendly PvP matchmaking.

<img src="https://raw.githubusercontent.com/suxrobGM/online-chess/main/screenshots/screenshot-3.jpg" alt="ChessMate" width="100%"/>

</td>
<td width="50%">

<div align="center">

**Maze**
[Repo](https://github.com/suxrobGM/maze-godot)

</div>

2D puzzle game with AI pathfinding and level progression.

<img src="https://raw.githubusercontent.com/suxrobGM/maze-godot/main/screenshots/game-scene.png" alt="Maze 2D" width="100%"/>

</td>
</tr>
</table>

---

<div align="center">

### GitHub Stats

![Metrics](https://github.com/suxrobGM/suxrobGM/blob/main/github-metrics.svg)

</div>

---

<div align="center">

### Let's Connect

Open to research collaborations and PhD-adjacent work. Happy to talk about **computer vision**, **multimodal ML**, and **explainable AI**, or about **.NET**, **TypeScript**, and **game dev**.

[![Google Scholar](https://img.shields.io/badge/Google_Scholar-4285F4?style=for-the-badge&logo=googlescholar&logoColor=white)](https://scholar.google.com/citations?user=p7ujRHoAAAAJ&hl=en)
[![Hugging Face](https://img.shields.io/badge/Hugging_Face-FFD21E?style=for-the-badge&logo=huggingface&logoColor=black)](https://huggingface.co/suxrobgm)
[![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/suxrobgm)
[![Portfolio](https://img.shields.io/badge/Portfolio-000?style=for-the-badge&logo=vercel&logoColor=white)](https://suxrobgm.net)
[![Telegram](https://img.shields.io/badge/Telegram-2CA5E0?style=for-the-badge&logo=telegram&logoColor=white)](https://t.me/suxrobgm)
[![Email](https://img.shields.io/badge/Email-EA4335?style=for-the-badge&logo=gmail&logoColor=white)](mailto:silyosbekov@gmail.com)

</div>

<!-- Footer Wave -->
<img src="https://capsule-render.vercel.app/api?type=waving&color=0:0d1117,50:161b22,100:1f6feb&height=120&section=footer" width="100%"/>
