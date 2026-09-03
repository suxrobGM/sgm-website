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
        "Sukhrob Ilyosbekov: computer vision and deep learning research. Papers on explainable " +
        "medical imaging, text-supervised representation learning for Cell Painting microscopy, " +
        "and client-side control of black-box image-editing models.";

    public const string ResearchStatement =
        "I work on vision models that hold up outside the benchmark: interpretability a clinician " +
        "can act on, representations that transfer to scientific imaging, and control over " +
        "generative models that cannot be retrained. I also build ML for a regulated clinical " +
        "platform, which keeps the questions tied to real decisions.";

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
                "MorphoCLIP matches Cell Painting microscopy images of drug- and gene-perturbed cells to " +
                "descriptions of the treatment written in ordinary language. The vision and language backbones " +
                "stay frozen (DINOv3, BioClinical ModernBERT) and only small projection heads train on top, " +
                "which is what lets the whole thing run on one consumer GPU. We tested retrieval in both " +
                "directions on the CPJUMP1 benchmark: 51 plates, over 3 million cell images, 303 drugs, 160 " +
                "genes. Plate-to-plate variation is corrected in the embedding space, so the model is not just " +
                "learning which plate an image came from. Joint work with two Northeastern co-authors; I led " +
                "the model design, training, and evaluation.",
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
                "Ask a commercial image editor to change one feature of a face and it tends to beautify the " +
                "whole thing, which is a real problem when the picture is meant to be a surgical preview. The " +
                "question here is how much of that you can fix from the client side, without touching the " +
                "model. I compared prompt-only steering, masked compositing, and model-based inpainting across " +
                "six commercial editors and one inpainting model, over 196 facelift and rhinoplasty edits, " +
                "scoring how well identity survived (ArcFace) and how well the edit stayed where it was asked " +
                "to stay. Plain masked compositing localized better than model-based inpainting.",
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
                "An EfficientNet V2 classifier over all nine ISIC 2019 diagnostic categories at 384x384, " +
                "trained with focal loss to cope with the heavy class imbalance, reaching 85.6% accuracy and " +
                "0.856 weighted F1 on 25,331 dermoscopic images. The more interesting part is what happens " +
                "after the prediction. GradCAM++ attention is broken down along the ABCDE criteria " +
                "dermatologists already use, with asymmetry, border irregularity, color variation (K-means), " +
                "and diameter measured straight off the lesion mask. Because those clinical features come out " +
                "as numbers, how well they line up with the model's attention can be scored, so the " +
                "interpretability claim rests on a metric instead of a few good-looking heatmaps.",
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
                "A ResNet18 encoder with a U-Net decoder and skip connections for monocular depth. 42% fewer " +
                "parameters than Depth Anything V2 (14.3M against 24.8M), 72% faster inference, and lower " +
                "relative error on NYU Depth V2.",
            RepoUrl = "https://github.com/suxrobgm/lightdepth",
            Tags = ["PyTorch", "ResNet18", "U-Net", "NYU Depth V2"],
        },
        new Project
        {
            Title = "FSRCNN: Accelerating Super-Resolution CNN",
            Summary =
                "A reproduction of FSRCNN (Dong et al., ECCV 2016) for single-image super-resolution at 2x, " +
                "3x, and 4x. The paper's gains over SRCNN held up on Set5 (+1.78 dB PSNR) and Set14 (+1.26 dB), " +
                "with added ablations on the shrinking and mapping layers.",
            RepoUrl = "https://github.com/suxrobgm/fsrcnn",
            Tags = ["PyTorch", "Mixed-Precision Training", "Set5/Set14/DIV2K"],
        },
        new Project
        {
            Title = "Bookshelf Scanner: Multi-Modal Book Detection and Recognition",
            Summary =
                "Point a camera at a bookshelf and get back a list of what is on it. YOLO instance " +
                "segmentation isolates each spine, then Moondream2 (via llama.cpp) reads the title and author. " +
                "FastAPI backend, Angular UI for corrections and export. Outstanding Project Award in the " +
                "graduate Computer Vision course.",
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
                "Pulls X-ray, CT, and MRI studies from hospital PACS over DICOM and runs PyTorch detectors " +
                "over them: pneumonia on chest X-ray, intracranial hemorrhage on head CT. Predictions render " +
                "as overlays inside OHIF, the viewer radiologists already work in, with on-the-fly " +
                "de-identification, audit logging, and role-based access. FastAPI backend, Next.js frontend.",
            RepoUrl = "https://github.com/suxrobgm/med-image-scanner",
            Tags = ["FastAPI", "PyTorch", "OpenCV", "OHIF", "DICOM", "Next.js"],
        },
        new Project
        {
            Title = "LogisticsX",
            Summary =
                "A Claude tool-use agent that matches freight loads to trucks, checks federal hours-of-service " +
                "for the driver, and plans multi-stop routes without a dispatcher in the loop. It sits inside " +
                "a full transportation management system: multi-tenant .NET backend, Angular portals, Kotlin " +
                "Multiplatform driver app, and load-board and telematics integrations.",
            SiteUrl = "https://logisticsx.app",
            RepoUrl = "https://github.com/suxrobgm/logistics-app",
            Tags = ["Claude API", "MCP", "Tool-Use Agents", ".NET 10", "Angular", "Kotlin Multiplatform"],
        },
    ];
}
