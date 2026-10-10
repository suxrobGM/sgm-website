namespace SGM.WebApp.Data;

public sealed record Job
{
    public required string Title { get; init; }
    public required string Company { get; init; }
    public required string Location { get; init; }
    public required string Period { get; init; }
    public required string Summary { get; init; }

    /// <summary>Short stable key for per-theme assets, e.g. <c>images/vc/mission-{Slug}.webp</c>.</summary>
    public required string Slug { get; init; }

    public bool Current { get; init; }
}

public sealed record SkillGroup
{
    public required string Name { get; init; }

    /// <summary>Font Awesome classes, e.g. "fas fa-brain".</summary>
    public required string Icon { get; init; }

    public required string[] Skills { get; init; }
}

/// <summary>A research item as shown on the home pages, with a shorter blurb than <see cref="ResearchData"/>.</summary>
public sealed record ResearchHighlight
{
    public required string Title { get; init; }

    /// <summary>Name short enough for a tape label.</summary>
    public required string ShortName { get; init; }

    public required string Summary { get; init; }
    public required string Icon { get; init; }
    public required string[] Tags { get; init; }
    public string? RepoUrl { get; init; }
    public string? PaperUrl { get; init; }
}

public sealed record Showcase
{
    public required string Title { get; init; }
    public required string Summary { get; init; }

    /// <summary>One-line headline fact, e.g. "50K+ registered players".</summary>
    public required string Highlight { get; init; }

    public required string[] Tags { get; init; }
    public required string Slug { get; init; }
    public string? Screenshot { get; init; }
    public string? RepoUrl { get; init; }
    public string? SiteUrl { get; init; }
    public bool Award { get; init; }
}

public sealed record Degree
{
    public required string Title { get; init; }
    public required string School { get; init; }
    public required string Period { get; init; }
    public string? Note { get; init; }
}

public sealed record ContactLink
{
    public required string Label { get; init; }
    public required string Display { get; init; }
    public required string Url { get; init; }
    public required string Icon { get; init; }
}

/// <summary>
/// Source of truth for the themed home pages. Each theme renders these lists its own way;
/// the research page has fuller write-ups in <see cref="ResearchData"/>.
/// </summary>
public static class PortfolioData
{
    public const string FullName = "Sukhrob Ilyosbekov";
    public const string Tagline = "Software Engineer · ML & Computer Vision Researcher";
    public const string UpworkUrl = "https://www.upwork.com/freelancers/suxrobgm";

    /// <summary>About-me paragraphs shown by every theme. Trusted HTML (the last one links to /research).</summary>
    public static readonly string[] About =
    [
        "I'm a software developer and ML engineer. My main stack is .NET, TypeScript, and Python: C# and ASP.NET Core " +
        "on the backend, React, Angular, or Next.js on the frontend, and PyTorch whenever there's a model to train.",
        "I like owning both halves of a project, the model and the product around it, so the ML part ships instead of " +
        "staying in a notebook. My research is in computer vision for medical images, and the papers and code are on " +
        "the <a href=\"/research\">research page</a>.",
    ];

    public static readonly string[] Services = ["Full-Stack Development", "Machine Learning", "Game Development"];

    public static readonly IReadOnlyList<Job> Jobs =
    [
        new Job
        {
            Title = "Software Engineer",
            Company = "EmTech Care Labs",
            Location = "Portland, ME",
            Period = "Jan 2025 - Present",
            Slug = "emtech",
            Current = true,
            Summary =
                "Lead engineer on an Alzheimer's care platform on AWS, where 200+ caregivers and patients message " +
                "each other and share care plans. I folded three web and mobile codebases into one TypeScript " +
                "monorepo and did the security work (encryption, audit logs, access control) that got us through an " +
                "outside HIPAA readiness review with no critical findings. The ML features are mine too: a " +
                "decline-risk model and an NLP pipeline for clinical notes. I also mentor two junior engineers.",
        },
        new Job
        {
            Title = "Software Engineer",
            Company = "AllFactors",
            Location = "Santa Clara, CA",
            Period = "Oct 2021 - Dec 2024",
            Slug = "allfactors",
            Summary =
                "Moved an old ASP.NET Web Forms real-estate analytics app to Blazor WebAssembly, and pages got about " +
                "twice as fast. I built the live pricing feed (SignalR plus SQL Server change tracking) that pushed " +
                "updates to hundreds of users in under a second, along with the component library all the dashboards " +
                "were built from. The valuation and forecast models behind those dashboards retrained every night on " +
                "the day's transactions.",
        },
        new Job
        {
            Title = "Software Engineer",
            Company = "Smart Meal Service",
            Location = "Russia",
            Period = "Sep 2020 - Jul 2021",
            Slug = "smartmeal",
            Summary =
                "Wrote the C# software that ran a self-service food kiosk and a robotic cashier, payment terminal and " +
                "hardware protocols included. I also built the customer ordering app (Xamarin) and the admin " +
                "dashboard (ASP.NET Core and Angular). A hardware abstraction layer let the same code run on all " +
                "three machine models, and orders went to the right kitchen station automatically.",
        },
        new Job
        {
            Title = "Game Developer",
            Company = "Pentalight Technology",
            Location = "Malaysia",
            Period = "Mar 2020 - Feb 2021",
            Slug = "pentalight",
            Summary =
                "Built multiplayer networking, spatial audio, hand tracking, and the in-world UI for a VR smart-city " +
                "simulation in Unity, plus a desktop client for anyone without a headset. Scene builds used to take " +
                "hours of hands-on work. After I automated the art pipeline (model import, LODs, light baking), an " +
                "artist could start one and walk away.",
        },
    ];

    public static readonly IReadOnlyList<SkillGroup> Skills =
    [
        new SkillGroup
        {
            Name = "AI & Deep Learning",
            Icon = "fas fa-brain",
            Skills = ["PyTorch", "TensorFlow", "OpenCV", "YOLO", "CNNs", "Vision Transformers", "GradCAM++"],
        },
        new SkillGroup
        {
            Name = "Backend Development",
            Icon = "fas fa-server",
            Skills = ["ASP.NET Core", "Node.js", "NestJS", "FastAPI", "Spring Boot", "Bun", "ElysiaJS", "Prisma"],
        },
        new SkillGroup
        {
            Name = "Frontend Development",
            Icon = "fas fa-laptop-code",
            Skills = ["React", "Angular", "Next.js", "Blazor", "TypeScript", "Tailwind"],
        },
        new SkillGroup
        {
            Name = "Mobile & Desktop",
            Icon = "fas fa-mobile-alt",
            Skills = ["MAUI", "React Native", "WPF", "Avalonia"],
        },
        new SkillGroup
        {
            Name = "Game Development",
            Icon = "fas fa-gamepad",
            Skills = ["Unity", "Godot", "PhaserJS", "Colyseus"],
        },
        new SkillGroup
        {
            Name = "Cloud & DevOps",
            Icon = "fas fa-cloud",
            Skills = ["AWS", "Azure", "Docker", "Kubernetes", "CI/CD"],
        },
    ];

    public static readonly IReadOnlyList<ResearchHighlight> Research =
    [
        new ResearchHighlight
        {
            Title = "MorphoCLIP: Text-Supervised Cell Painting Retrieval",
            ShortName = "MorphoCLIP",
            Icon = "fas fa-dna",
            Summary =
                "Matches microscope images of treated cells to a plain-language description of the drug or gene " +
                "behind the change. The DINOv3 and BioClinical ModernBERT encoders stay frozen and only small heads " +
                "are trained, so one consumer GPU is enough. Tested on CPJUMP1 (51 plates, 3M+ images).",
            Tags = ["PyTorch", "DINOv3", "BioClinical ModernBERT", "Contrastive Learning"],
            RepoUrl = "https://github.com/suxrobgm/morphoclip",
            PaperUrl = "https://arxiv.org/abs/2608.22690",
        },
        new ResearchHighlight
        {
            Title = "Localize, Don't Beautify: Controlling Image-Editing APIs",
            ShortName = "Localize, Don't Beautify",
            Icon = "fas fa-image",
            Summary =
                "Image editors asked to change one facial feature tend to retouch the whole face, which ruins a " +
                "surgical preview. I tried three client-side fixes across six commercial editors and 196 edits, " +
                "checking identity with ArcFace and whether each edit stayed put. Plain masked compositing worked " +
                "best.",
            Tags = ["Image-Editing APIs", "Inpainting", "ArcFace"],
            PaperUrl = "https://arxiv.org/abs/2608.02841",
        },
        new ResearchHighlight
        {
            Title = "MelanomaNet: Explainable Skin Lesion Classification",
            ShortName = "MelanomaNet",
            Icon = "fas fa-microscope",
            Summary =
                "A nine-class skin-lesion classifier (EfficientNet V2, focal loss) with 0.86 weighted F1 on 25K ISIC " +
                "2019 images. Its GradCAM++ heatmaps are broken down by the ABCDE criteria dermatologists already " +
                "use, so a clinician can see what the model was looking at.",
            Tags = ["PyTorch", "EfficientNet V2", "GradCAM++", "OpenCV"],
            RepoUrl = "https://github.com/suxrobgm/explainable-melanoma",
            PaperUrl = "https://arxiv.org/abs/2512.09289",
        },
        new ResearchHighlight
        {
            Title = "LightDepth: Lightweight Monocular Depth Estimation",
            ShortName = "LightDepth",
            Icon = "fas fa-eye",
            Summary =
                "Estimates depth from one image with a ResNet18 encoder and U-Net decoder. Next to Depth Anything V2 " +
                "it has 42% fewer parameters (14.3M vs. 24.8M), runs 72% faster, and makes smaller errors on NYU " +
                "Depth V2.",
            Tags = ["PyTorch", "ResNet18", "U-Net", "Depth Estimation"],
            RepoUrl = "https://github.com/suxrobgm/lightdepth",
        },
        new ResearchHighlight
        {
            Title = "FSRCNN: Super-Resolution Neural Network",
            ShortName = "FSRCNN",
            Icon = "fas fa-expand",
            Summary =
                "Rebuilt FSRCNN (Dong et al., ECCV 2016) from the paper at 2x, 3x, and 4x. My version reproduced its " +
                "gains over SRCNN on Set5 (+1.78 dB PSNR) and Set14 (+1.26 dB), and I added ablations on the " +
                "shrinking and mapping layers.",
            Tags = ["PyTorch", "Super-Resolution", "Mixed Precision"],
            RepoUrl = "https://github.com/suxrobgm/fsrcnn",
        },
    ];

    public static readonly IReadOnlyList<Showcase> Projects =
    [
        new Showcase
        {
            Title = "LogisticsX",
            Slug = "logisticsx",
            Highlight = "LLM dispatch agent, 4 portals, live app",
            Screenshot = "images/portfolio/logisticsx.jpg",
            Summary =
                "Software for running a trucking company. A multi-provider LLM agent with custom tools matches loads to trucks, " +
                "checks drivers' federal hours-of-service limits, and plans multi-stop routes. The rest of the system " +
                "is four web portals, a Kotlin Multiplatform driver app, a multi-tenant .NET backend (DDD/CQRS), " +
                "Stripe payments, and integrations with DAT, Truckstop, Samsara, and Motive.",
            Tags = ["ASP.NET Core", "Angular", "Blazor", "Kotlin Multiplatform", "SignalR", "LLM Agents"],
            RepoUrl = "https://github.com/suxrobgm/logistics-app",
            SiteUrl = "https://logisticsx.app",
        },
        new Showcase
        {
            Title = "JobPilot",
            Slug = "jobpilot",
            Highlight = "Runs on your Claude Code or Codex plan",
            Screenshot = "images/portfolio/jobpilot.jpg",
            Summary =
                "An AI agent that applies to jobs for you. It searches LinkedIn, Indeed, and other boards, scores each " +
                "posting against your resume, tailors the resume, and fills in the application in a real browser on " +
                "your own machine. It runs inside Claude Code or Codex, so there's no API key or usage bill. A web " +
                "dashboard tracks every application from applied to offer, and a small .NET host links the agent to it.",
            Tags = ["Next.js", "ElysiaJS", "PostgreSQL", "Prisma", ".NET 10", "Playwright", "Claude Code"],
            RepoUrl = "https://github.com/suxrobGM/jobpilot",
            SiteUrl = "https://jobpilot.suxrobgm.net",
        },
        new Showcase
        {
            Title = "Meat.gg",
            Slug = "meatgg",
            Highlight = "50K+ registered players",
            Screenshot = "images/portfolio/meatgg.jpg",
            Summary =
                "Community site for Counter-Strike 2 servers, with 50K+ registered players and over 1K active on a " +
                "typical day. Players get profiles, friends, messaging, and a cosmetics shop (Stripe). A C++ " +
                "Metamod:Source 2 plugin ties the game servers to the backend, so admins can ban, handle reports, and " +
                "moderate without leaving the game.",
            Tags = ["ElysiaJS", "Next.js", "PostgreSQL", "Prisma", "Bun", "Docker", "Stripe"],
            SiteUrl = "https://meat.gg",
        },
        new Showcase
        {
            Title = "DepVault",
            Slug = "depvault",
            Highlight = "8 package ecosystems scanned",
            Screenshot = "images/portfolio/depvault.jpg",
            Summary =
                "Checks your dependencies against OSV.dev for known vulnerabilities in eight ecosystems, including " +
                "npm, PyPI, NuGet, Cargo, and Maven, and gets through 1,000+ of them in seconds. It's also an " +
                "encrypted secrets vault (AES-256-GCM) with one-time sharing and a .NET AOT CLI that injects tokens " +
                "into CI/CD pipelines.",
            Tags = ["Next.js", "ElysiaJS", "PostgreSQL", "Prisma", "Bun", ".NET AOT", "Docker"],
            RepoUrl = "https://github.com/suxrobGM/depvault",
            SiteUrl = "https://depvault.com",
        },
        new Showcase
        {
            Title = "Med Image Scanner",
            Slug = "medscanner",
            Highlight = "X-ray, CT and MRI from hospital PACS",
            Screenshot = "images/portfolio/med-image-scanner.jpg",
            Summary =
                "Pulls X-ray, CT, and MRI studies from hospital PACS over DICOM and runs PyTorch detectors on them, " +
                "for example pneumonia on chest X-rays and brain bleeds on head CTs. Results show up as overlays in " +
                "the OHIF viewer. Scans are de-identified on the fly, and access is role-based and logged. Built with " +
                "FastAPI and Next.js.",
            Tags = ["Python", "FastAPI", "Next.js", "PyTorch", "OHIF"],
            RepoUrl = "https://github.com/suxrobgm/med-image-scanner",
        },
        new Showcase
        {
            Title = "Bookshelf Scanner",
            Slug = "bookshelf",
            Highlight = "Outstanding Project Award, Northeastern",
            Award = true,
            Summary =
                "Snap a photo of a bookshelf and get back a list of the books. YOLO picks out each spine, Moondream2 " +
                "(via llama.cpp) reads the title and author, and an Angular UI lets you fix mistakes and export. " +
                "It won an Outstanding Project Award at Northeastern.",
            Tags = ["Python", "YOLO", "Moondream2", "FastAPI"],
            RepoUrl = "https://github.com/suxrobgm/bookshelf-scanner",
        },
    ];

    public static readonly IReadOnlyList<Degree> Education =
    [
        new Degree
        {
            Title = "M.S. Computer Science",
            School = "Northeastern University, Boston, MA",
            Period = "January 2024 - May 2026",
            Note = "GPA: 4.0/4.0",
        },
        new Degree
        {
            Title = "B.S. Computer Science",
            School = "Suffolk University, Boston, MA",
            Period = "September 2021 - May 2023",
            Note = "Cum Laude | GPA: 3.5/4.0",
        },
        new Degree
        {
            Title = "B.S. Computer Science",
            School = "INTI International College, Malaysia",
            Period = "September 2019 - July 2021",
        },
        new Degree
        {
            Title = "B.S. Software Engineering",
            School = "TUIT, Uzbekistan",
            Period = "September 2017 - June 2019",
        },
    ];

    public static readonly IReadOnlyList<ContactLink> Contacts =
    [
        new ContactLink { Label = "Email", Display = "silyosbekov@gmail.com", Url = "mailto:silyosbekov@gmail.com", Icon = "fas fa-envelope" },
        new ContactLink { Label = "LinkedIn", Display = "linkedin.com/in/suxrobgm", Url = "https://linkedin.com/in/suxrobgm", Icon = "fab fa-linkedin" },
        new ContactLink { Label = "GitHub", Display = "github.com/suxrobgm", Url = "https://github.com/suxrobgm", Icon = "fab fa-github" },
        new ContactLink { Label = "Telegram", Display = "@suxrobgm", Url = "https://t.me/suxrobgm", Icon = "fab fa-telegram" },
    ];

    public static readonly IReadOnlyList<ContactLink> Socials =
    [
        new ContactLink { Label = "GitHub", Display = "GitHub", Url = "https://github.com/suxrobgm", Icon = "fab fa-github" },
        new ContactLink { Label = "LinkedIn", Display = "LinkedIn", Url = "https://linkedin.com/in/suxrobgm", Icon = "fab fa-linkedin" },
        new ContactLink { Label = "Instagram", Display = "Instagram", Url = "https://instagram.com/suxrob_gm", Icon = "fab fa-instagram" },
        new ContactLink { Label = "Telegram", Display = "Telegram", Url = "https://t.me/suxrobgm", Icon = "fab fa-telegram" },
    ];
}
