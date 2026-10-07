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
/// Projects that are not papers. Kept separate from <see cref="Publication"/> on purpose;
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
        "Sukhrob Ilyosbekov's computer vision research on medical and scientific images, with " +
        "papers, code, and BibTeX.";

    public const string ResearchStatement =
        "My research is in computer vision for medicine and biology. I'm most interested in " +
        "models whose decisions an expert can check, and in representations that carry over to " +
        "images the model wasn't trained on.";

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
                "Cell Painting images show how cells change after a drug or a genetic perturbation. " +
                "MorphoCLIP learns to match those images to a plain-language description of the treatment, " +
                "in either direction. The image and text encoders (DINOv3 and BioClinical ModernBERT) stay " +
                "frozen and only small projection heads are trained, so it fits on one consumer GPU. We " +
                "evaluated it on CPJUMP1, which has 51 plates, over 3 million images, 303 compounds, and 160 " +
                "genes. Plate effects are corrected in the embedding space so the model can't score well " +
                "just by recognizing which plate an image came from. I led the design, training, and " +
                "evaluation, working with two co-authors at Northeastern.",
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
                "Ask a commercial image editor to change someone's nose and it will often retouch the whole " +
                "face, which defeats the point of a surgical preview. This paper tests how much of that a " +
                "client can fix without access to the model. I compared prompt-only steering, masked " +
                "compositing, and model-based inpainting across six commercial editors and one inpainting " +
                "model on 196 facelift and rhinoplasty edits. Identity preservation was measured with " +
                "ArcFace, along with whether each edit stayed where it was asked to. The simplest method, " +
                "masked compositing, kept edits in place better than inpainting did.",
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
                "A skin-lesion classifier covering all nine ISIC 2019 categories: EfficientNet V2 at 384x384, " +
                "trained with focal loss because some classes are rare. It reaches about 86% accuracy (0.86 " +
                "weighted F1) on roughly 25,000 dermoscopic images. The other half of the paper is about " +
                "explanations. GradCAM++ attention is split along the ABCDE criteria dermatologists already " +
                "use, and asymmetry, border irregularity, color variation (via K-means), and diameter are all " +
                "measured from the lesion mask. Since those features are numbers, the paper can score how " +
                "well the model's attention matches them instead of showing a few hand-picked heatmaps.",
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

    public static readonly IReadOnlyList<Project> OtherProjects =
    [
        new Project
        {
            Title = "LightDepth: Lightweight Monocular Depth Estimation",
            Summary =
                "Depth from a single image, using a ResNet18 encoder and a U-Net decoder with skip " +
                "connections. Compared with Depth Anything V2 it has 42% fewer parameters (14.3M vs. 24.8M), " +
                "runs 72% faster, and has lower relative error on NYU Depth V2.",
            RepoUrl = "https://github.com/suxrobgm/lightdepth",
            Tags = ["PyTorch", "ResNet18", "U-Net", "NYU Depth V2"],
        },
        new Project
        {
            Title = "FSRCNN: Accelerating Super-Resolution CNN",
            Summary =
                "My reimplementation of FSRCNN (Dong et al., ECCV 2016) for single-image super-resolution at " +
                "2x, 3x, and 4x. It matched the paper's gains over SRCNN on Set5 (+1.78 dB PSNR) and Set14 " +
                "(+1.26 dB). I also ran ablations on the shrinking and mapping layers.",
            RepoUrl = "https://github.com/suxrobgm/fsrcnn",
            Tags = ["PyTorch", "Mixed-Precision Training", "Set5/Set14/DIV2K"],
        },
        new Project
        {
            Title = "Bookshelf Scanner: Multi-Modal Book Detection and Recognition",
            Summary =
                "Take a photo of a bookshelf and get a list of the books on it. YOLO instance segmentation " +
                "finds each spine, and Moondream2, running through llama.cpp, reads the title and author. " +
                "FastAPI on the back end, with an Angular UI for fixing mistakes and exporting. Won an " +
                "Outstanding Project Award at Northeastern.",
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
                "Connects to a hospital's PACS over DICOM, pulls X-ray, CT, and MRI studies, and runs PyTorch " +
                "detectors on them, such as pneumonia on chest X-rays and intracranial hemorrhage on head " +
                "CTs. Findings appear as overlays in OHIF, the viewer radiologists already work in. Studies " +
                "are de-identified on the fly, and every access is role-checked and logged. FastAPI on the " +
                "back end, Next.js on the front.",
            RepoUrl = "https://github.com/suxrobgm/med-image-scanner",
            Tags = ["FastAPI", "PyTorch", "OpenCV", "OHIF", "DICOM", "Next.js"],
        },
        new Project
        {
            Title = "LogisticsX",
            Summary =
                "A trucking management system where a multi-provider LLM agent with custom tools handles " +
                "dispatch. It matches loads to trucks, checks each driver's federal hours-of-service limits, " +
                "and plans multi-stop routes. Around it is the rest of the product: a multi-tenant .NET backend, " +
                "Angular portals, a Kotlin Multiplatform driver app, and integrations with load boards and " +
                "telematics providers.",
            SiteUrl = "https://logisticsx.app",
            RepoUrl = "https://github.com/suxrobgm/logistics-app",
            Tags = ["Multi-Provider LLMs", "MCP", "Tool-Use Agents", ".NET 10", "Angular", "Kotlin Multiplatform"],
        },
    ];
}
