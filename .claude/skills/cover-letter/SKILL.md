---
name: cover-letter
description: |
  Generate a tailored cover letter for job applications based on the provided
  job description. Uses Sukhrob's full background, experience, and project
  portfolio to craft a compelling, personalized cover letter. Automatically
  applies the humanizer skill to ensure natural, human-sounding writing.
argument-hint: "<job_description>"
---

# Cover Letter Generator

You are writing a cover letter for **Sukhrob Ilyosbekov**, a senior full-stack engineer and computer vision researcher applying for a job. Your goal is to write a compelling, tailored cover letter that connects his experience to the role.

## Candidate Profile

### Identity

- **Name:** Sukhrob Ilyosbekov
- **Location:** Portland, ME
- **Email:** silyosbekov@gmail.com
- **Phone:** (857) 867-1942
- **Portfolio:** https://suxrobgm.net
- **LinkedIn:** https://linkedin.com/in/suxrobgm
- **GitHub:** https://github.com/suxrobgm

### Education

- M.S. Computer Science, Northeastern University (4.0 GPA, graduating May 2026)
- B.S. Computer Science, Suffolk University (Cum Laude, 3.5 GPA)
- B.S. Computer Science, INTI International College Subang, Malaysia (Transfer Program)
- B.S. Software Engineering, Tashkent University of Information Technologies

### Core Technical Skills

- **Languages:** C#, TypeScript, JavaScript, Python, Kotlin, C++
- **Backend:** ASP.NET Core, Node.js, NestJS, FastAPI, Spring Boot, ElysiaJS, Bun
- **Frontend:** React, Next.js, Angular, Blazor, Tailwind CSS
- **Mobile:** Kotlin Multiplatform, .NET MAUI, React Native
- **Databases:** PostgreSQL, SQL Server, MongoDB, Redis, DynamoDB, Firebase, Prisma
- **AI/ML:** PyTorch, TensorFlow, OpenCV, YOLO, LLM integration, DICOM/OHIF, EfficientNet, ResNet, U-Net, GradCAM++, Vision Transformers
- **Realtime:** SignalR, WebSockets, gRPC, Colyseus
- **Game Dev:** Unity, Godot, PhaserJS
- **Cloud & DevOps:** AWS (Amplify, S3, Lambda, Cognito), Azure, Docker, Kubernetes, CUDA

### Professional Experience

**Full Stack Developer | EmTech Care Labs | Portland, ME (Jan 2025 -- Jun 2025)**

- HIPAA-compliant healthcare platform serving 200+ caregivers and patients for Alzheimer's care planning
- Real-time messaging and notifications for collaborative care
- Led AWS Amplify Gen 1 to Gen 2 migration, cutting build/deployment times by 40%
- Unified codebase into TypeScript monorepo
- Consolidated 3 component libraries into single design system with 40+ reusable components

**Freelance Full Stack Developer | Upwork (Jul 2022 -- Present)**

- Top Rated Plus, 100% Job Success Score, 10+ full-stack projects
- Healthcare, real estate, gaming industries
- Built MMO game with custom server-side physics for 100+ concurrent players
- AI-powered medical imaging platform deployed on Azure Kubernetes
- Blazor form builder, Next.js/NestJS real estate platform

**.NET Software Engineer | Virtuworks | Miami, FL (Dec 2022 -- Dec 2023)**

- Migrated ASP.NET Web Forms to Blazor WebAssembly, reducing page load times by 50%
- 30+ responsive, reusable UI components

**.NET Software Engineer | Frost Pixel Studio | Russia (Oct 2021 -- May 2022)**

- Web apps and browser-based games: ASP.NET Core, Blazor, Angular, PhaserJS
- 30% performance improvement through server-side caching
- Knowledge management platform with Node.js, Dgraph, React

**.NET Software Engineer | Smart Meal Service | Russia (Sep 2020 -- Oct 2021)**

- Robotic cashier and self-service kiosk, 500+ daily transactions
- POS integration with WPF, gRPC, computer vision for product recognition
- Full SDLC across 5 concurrent projects

**Game Developer | Pentalight Technology | Malaysia (Mar 2020 -- Feb 2021)**

- Multiplayer VR networking for 20+ concurrent users, Unity, MLAPI, SteamVR

**Game Developer | EC Dev Team | Uzbekistan (2016 -- 2019)**

- Led RTS game development in Clausewitz Engine

### Key Projects

**LogisticsX** (https://logisticsx.app) -- Multi-tenant TMS for intermodal trucking. Load boards (DAT, Truckstop), ELD/HOS compliance (Samsara, Motive), Stripe Connect, real-time tracking, DDD + CQRS with MediatR. Stack: ASP.NET Core, EF Core, SignalR, Angular, Kotlin KMP, PostgreSQL, Stripe, Mapbox, Docker.

**Meat.gg** (https://meat.gg) -- CS2 community platform, 30K+ registered users, 1K+ DAU. Social features, in-game admin/ban/report system, Stripe shop, real-time game server integration. Stack: ElysiaJS, Next.js, PostgreSQL, Prisma, Bun, Docker.

**DepVault** (https://depvault.com) -- Dependency scanner & encrypted secrets vault. 8+ ecosystems, CVE detection via OSV.dev, AES-256-GCM encryption, one-time secret sharing, CI/CD token injection. Stack: Next.js, ElysiaJS, PostgreSQL, Prisma, .NET AOT CLI, Docker.

**Med Image Scanner** -- HIPAA-compliant DICOM viewer with AI-powered analysis. Hospital PACS connection, measurement/segmentation tools, disease-detection overlays. Stack: FastAPI, Next.js, PyTorch, OpenCV, OHIF Viewer, PostgreSQL.

**Bookshelf Scanner** -- CV + LLM book detection pipeline. YOLO segmentation + Moondream2 vision-language model for title/author extraction. Stack: Python, FastAPI, YOLO, Moondream2, PyTorch, Angular.

**Blazor Form Builder** -- Drag-and-drop form designer, JSON schema generation and rendering. Stack: Blazor WebAssembly, C#, SQL Server.

**Chestnut MMO** -- Real-time MMO with authoritative server, custom physics, 100+ concurrent players, Web3 integration.

**ChessMate** -- Online chess platform with AI opponents, rated/friendly PvP matchmaking. Stack: Spring Boot, Angular, WebSocket.

### Research & Publications

- **MelanomaNet** -- Explainable deep learning for multi-class skin lesion classification. Published: arXiv:2512.09289. EfficientNet V2, GradCAM++, automated ABCDE feature extraction, 9 ISIC 2019 categories.
- **LightDepth** -- Lightweight monocular depth estimation. 42% fewer parameters than Depth Anything V2, 72% faster inference, better error metrics on NYU Depth V2.
- **FSRCNN** -- Super-resolution CNN. 40x speedup over SRCNN, +1.78 dB PSNR on Set5.
- **AoE4 Matchmaking** -- ML-assisted player matching with XGBoost, K-means clustering, ELO rating.

### Awards

- National Physics Olympiad, 1st Place (Uzbekistan) -- first student from his high school to win at national level
- University Coding Competition Winner (TUIT)
- Outstanding Project Award, Bookshelf Scanner (Northeastern University)

---

## Process

### Step 1: Analyze the Job Description

Read the job description provided as the argument. Identify:

- Company name and what they do
- Role title and level
- Key responsibilities
- Required and preferred qualifications
- Tech stack and domain
- Company culture cues and values

### Step 2: Match Relevant Experience

From the candidate profile, select the most relevant:

- 2-3 work experiences that align with the role
- 2-3 projects that demonstrate required skills
- Research work (if the role involves AI/ML/CV)
- Education details (if relevant to the role level)

### Step 3: Write the Cover Letter

Follow this structure:

**Header:**

```
Sukhrob Ilyosbekov
Portland, ME | (857) 867-1942 | silyosbekov@gmail.com
linkedin.com/in/suxrobgm | github.com/suxrobgm | suxrobgm.net
```

**Opening paragraph (2-3 sentences):**

- State the role you're applying for
- Lead with your strongest, most relevant qualifier for this specific role
- Show you understand what the company does or what the team needs
- Do NOT use "I'm excited to apply" or "I'm writing to express my interest"

**Body paragraph 1 -- Relevant experience (3-5 sentences):**

- Connect your most relevant work experience to their needs
- Include specific metrics and outcomes
- Name projects and results, not just technologies
- Show you've solved problems similar to theirs

**Body paragraph 2 -- Technical depth (3-5 sentences):**

- Demonstrate deeper technical alignment with the role
- Reference specific projects or research that match their stack/domain
- Show understanding of their technical challenges
- If AI/ML role: reference publications and research

**Body paragraph 3 -- Why this company (2-3 sentences):**

- What specifically draws you to this role/company (be genuine, not generic)
- How your background uniquely positions you for their specific needs
- What you'd bring beyond the technical requirements

**Closing paragraph (2-3 sentences):**

- Express interest in discussing further
- Reference portfolio/GitHub if relevant
- Thank them for their time

**Sign-off:**

```
Best regards,
Sukhrob Ilyosbekov
```

### Step 4: Apply Humanizer

After writing the cover letter, invoke the `/humanizer` skill on the full text to remove any AI writing patterns. The final output must read as naturally written by a real person.

## Important Rules

1. **Keep it to one page.** 350-450 words for the body (excluding header/sign-off).
2. **No fluff.** Remove words like "passionate," "dedicated," "committed," "excited," "thrilled," "leverage," "utilize," "innovative," "cutting-edge," "eager," "dynamic."
3. **No generic openings.** Never start with "I am writing to express my interest" or "I was excited to see your posting."
4. **Be specific.** Reference actual project names, metrics, and technologies. Vague claims are ignored.
5. **Tailor aggressively.** Every sentence should connect to something in the job description. If a paragraph could apply to any job, rewrite it.
6. **Show, don't tell.** Instead of "I'm a strong communicator," show it through your writing. Instead of "I'm experienced in X," describe what you built with X.
7. **Match the company's tone.** Startup? Write conversationally. Enterprise/government? More formal.
8. **Do not invent experience.** Only reference projects and skills from the candidate profile.
9. **Write in first person** as Sukhrob.
10. **For AI/ML/research roles:** Lead with research publications and academic background. Emphasize the 4.0 GPA and arXiv publication.
11. **For senior/lead roles:** Lead with the 9+ years of experience, team collaboration at EmTech Care Labs, and architectural decisions (DDD, CQRS).
12. **For startup roles:** Lead with the breadth of shipped products and Upwork freelancing (demonstrates autonomy and delivery speed).

## Output Format

Output the complete cover letter with header and sign-off, formatted with proper spacing. Use plain text that can be pasted into any application form or converted to PDF.
