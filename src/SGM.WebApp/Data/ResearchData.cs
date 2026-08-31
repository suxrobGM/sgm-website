namespace SGM.WebApp.Data;

public sealed record Publication
{
    public required string Title { get; init; }

    /// <summary>Display order, e.g. "S. Ilyosbekov, S. Gajjar, R. Jin".</summary>
    public required string Authors { get; init; }

    /// <summary>Doubles as status: "arXiv preprint, 2026" or "Under review, MIDL 2027".</summary>
    public required string Venue { get; init; }

    public required string ArxivId { get; init; }

    public string? RepoUrl { get; init; }

    /// <summary>Written for this site. Not the paper's abstract, which stays on arXiv.</summary>
    public required string Summary { get; init; }

    public required string BibTex { get; init; }

    public required string[] Tags { get; init; }

    public string ArxivUrl => $"https://arxiv.org/abs/{ArxivId}";
}

/// <summary>
/// Coursework and applied work. Kept separate from <see cref="Publication"/> on purpose;
/// which list an entry sits in is what distinguishes the two kinds.
/// </summary>
public sealed record Project
{
    public required string Title { get; init; }
    public required string Summary { get; init; }
    public string? SiteUrl { get; init; }
    public string? RepoUrl { get; init; }
    public required string[] Tags { get; init; }
}

/// <summary>
/// Source of truth for research content. The themed home pages still duplicate it as
/// hardcoded markup and should be migrated onto these lists.
/// </summary>
public static class ResearchData
{
    public const string ScholarUrl = "https://scholar.google.com/citations?user=p7ujRHoAAAAJ&hl=en";
    public const string GitHubUrl = "https://github.com/suxrobgm";
    public const string LinkedInUrl = "https://www.linkedin.com/in/suxrobgm";
    public const string Email = "ilyosbekov.s@northeastern.edu";

    public const string PageDescription =
        "Sukhrob Ilyosbekov — computer vision and deep learning research. Publications on " +
        "explainable medical imaging, text-supervised representation learning for Cell Painting " +
        "microscopy, and client-side control of black-box image-editing models.";

    public const string ResearchStatement =
        "I work on making vision models trustworthy enough to deploy. Three threads run through " +
        "the work: interpretability a clinician can audit rather than take on faith, control over " +
        "black-box generative systems from outside the model, and representation learning that " +
        "transfers to scientific imaging. Building ML for a HIPAA-regulated clinical platform is " +
        "where those same questions arrive with consequences attached.";

    public static readonly IReadOnlyList<Publication> Publications =
    [
        new Publication
        {
            Title = "MorphoCLIP: Text-Supervised Contrastive Learning for Perturbation Matching in Cell Painting Images",
            Authors = "S. Ilyosbekov, S. Gajjar, R. Jin",
            Venue = "arXiv preprint, 2026",
            ArxivId = "2608.22690",
            RepoUrl = "https://github.com/suxrobgm/morphoclip",
            Summary =
                "A text-supervised contrastive model that matches Cell Painting microscopy images of drug- and " +
                "gene-perturbed cells to natural-language treatment descriptions. Frozen vision and language " +
                "backbones (DINOv3, BioClinical ModernBERT) are paired with trainable projection heads, so the " +
                "whole model trains on a single consumer GPU. Evaluated on bidirectional image-to-text and " +
                "text-to-image retrieval over the CPJUMP1 benchmark (51 plates, 3M+ cell images, 303 drugs, " +
                "160 genes), with batch correction in the embedding space to remove plate-to-plate variation. " +
                "Collaborative work with Northeastern co-authors; I led the model design, training, and evaluation.",
            BibTex =
                """
                @article{ilyosbekov2026morphoclip,
                  title   = {MorphoCLIP: Text-Supervised Contrastive Learning for
                             Perturbation Matching in Cell Painting Images},
                  author  = {Ilyosbekov, Sukhrob and Gajjar, S. and Jin, R.},
                  journal = {arXiv preprint arXiv:2608.22690},
                  year    = {2026},
                  url     = {https://arxiv.org/abs/2608.22690}
                }
                """,
            Tags = ["PyTorch", "DINOv3", "BioClinical ModernBERT", "Contrastive Learning", "CPJUMP1"],
        },
        new Publication
        {
            Title = "Localize, Don't Beautify: Client-Side Control of Image-Editing APIs for Cosmetic Surgery Previews",
            Authors = "S. Ilyosbekov",
            Venue = "arXiv preprint, 2026",
            ArxivId = "2608.02841",
            RepoUrl = null,
            Summary =
                "How much of a black-box image editor's behaviour can be controlled from outside the model? " +
                "Cosmetic-surgery previews are the test case: commercial APIs beautify the whole face when asked " +
                "to change a single feature. This paper compares prompt-only steering, masked compositing, and " +
                "model-based inpainting across six commercial editors and one inpainting model on 196 facelift " +
                "and rhinoplasty edits, scoring identity preservation with ArcFace alongside localization " +
                "accuracy. Masked compositing beat model-based inpainting on localization.",
            BibTex =
                """
                @article{ilyosbekov2026localize,
                  title   = {Localize, Don't Beautify: Client-Side Control of
                             Image-Editing APIs for Cosmetic Surgery Previews},
                  author  = {Ilyosbekov, Sukhrob},
                  journal = {arXiv preprint arXiv:2608.02841},
                  year    = {2026},
                  url     = {https://arxiv.org/abs/2608.02841}
                }
                """,
            Tags = ["Image-Editing APIs", "Inpainting", "ArcFace", "Prompt Engineering"],
        },
        new Publication
        {
            Title = "MelanomaNet: Explainable Deep Learning for Multi-Class Skin Lesion Classification",
            Authors = "S. Ilyosbekov",
            Venue = "arXiv preprint, 2025",
            ArxivId = "2512.09289",
            RepoUrl = "https://github.com/suxrobgm/explainable-melanoma",
            Summary =
                "An EfficientNet V2 classifier trained across all nine ISIC 2019 diagnostic categories at " +
                "384x384 resolution, reaching 85.6% accuracy and 0.856 weighted F1 on 25,331 dermoscopic images, " +
                "with focal loss against the heavy class imbalance. The contribution is interpretability: " +
                "GradCAM++ attention is decomposed along the ABCDE criteria dermatologists already use, " +
                "quantifying asymmetry, border irregularity, colour variation via K-means, and diameter directly " +
                "from the lesion mask. Alignment metrics between model attention and those extracted clinical " +
                "features let the interpretability claim be measured rather than argued from a handful of " +
                "example heatmaps.",
            BibTex =
                """
                @article{ilyosbekov2025melanomanet,
                  title   = {MelanomaNet: Explainable Deep Learning for
                             Multi-Class Skin Lesion Classification},
                  author  = {Ilyosbekov, Sukhrob},
                  journal = {arXiv preprint arXiv:2512.09289},
                  year    = {2025},
                  url     = {https://arxiv.org/abs/2512.09289}
                }
                """,
            Tags = ["PyTorch", "EfficientNet V2", "GradCAM++", "OpenCV", "ISIC 2019"],
        },
    ];

    public static readonly IReadOnlyList<Project> CourseProjects =
    [
        new Project
        {
            Title = "LightDepth: Lightweight Monocular Depth Estimation",
            Summary =
                "A ResNet18 encoder with a U-Net decoder and skip connections, holding accuracy with 42% fewer " +
                "parameters (14.3M vs 24.8M) than Depth Anything V2 and running 72% faster at inference, while " +
                "improving relative error on NYU Depth V2.",
            RepoUrl = "https://github.com/suxrobgm/lightdepth",
            Tags = ["PyTorch", "ResNet18", "U-Net", "NYU Depth V2"],
        },
        new Project
        {
            Title = "FSRCNN: Accelerating Super-Resolution CNN",
            Summary =
                "A reproduction of FSRCNN (Dong et al., ECCV 2016) for single-image super-resolution at 2x, 3x, " +
                "and 4x, matching the reported gains on Set5 and Set14 with ablations on the shrinking and " +
                "mapping layers.",
            RepoUrl = "https://github.com/suxrobgm/fsrcnn",
            Tags = ["PyTorch", "Mixed-Precision Training", "Set5/Set14/DIV2K"],
        },
        new Project
        {
            Title = "Bookshelf Scanner: Multi-Modal Book Detection and Recognition",
            Summary =
                "YOLO instance segmentation isolates each book spine on a shelf, then the Moondream2 " +
                "vision-language model reads title and author off each spine. Received the Outstanding Project " +
                "Award in the graduate Computer Vision course.",
            RepoUrl = "https://github.com/suxrobgm/bookshelf-scanner",
            Tags = ["YOLO", "Moondream2 VLM", "llama.cpp", "FastAPI"],
        },
    ];

    public static readonly IReadOnlyList<Project> AppliedProjects =
    [
        new Project
        {
            Title = "Med Image Scanner",
            Summary =
                "A HIPAA-compliant platform that pulls X-ray, CT, and MRI scans directly from hospital imaging " +
                "systems and runs PyTorch detection models on them, flagging pneumonia on chest X-rays and " +
                "intracranial hemorrhage on head CTs. Predictions render as overlays inside a real radiology " +
                "viewer (OHIF), with on-the-fly de-identification, audit logging, and role-based access.",
            RepoUrl = "https://github.com/suxrobgm/med-image-scanner",
            Tags = ["FastAPI", "PyTorch", "OHIF", "DICOM"],
        },
        new Project
        {
            Title = "LogisticsX",
            Summary =
                "An AI dispatcher that matches freight loads to trucks, checks federal driver hours-of-service " +
                "rules, and plans multi-stop routes on its own, turning a manual 15-minute decision into a " +
                "near-instant one. Runs on the Claude API through a custom tool-use agent.",
            SiteUrl = "https://logisticsx.app",
            RepoUrl = "https://github.com/suxrobgm/logistics-app",
            Tags = ["Claude API", "MCP", "Agentic Tool Use", ".NET"],
        },
    ];
}
