namespace SGM.BlogApp.Pages;

public class SitemapModel : PageModel
{
    private readonly IWebHostEnvironment _env;

    public string SitemapContent { get; set; }

    public SitemapModel(IWebHostEnvironment env)
    {
        _env = env;
    }

    public void OnGet()
    {
        var sitemapFile = System.IO.Path.Combine(_env.WebRootPath, "sitemap.xml");
        SitemapContent = System.IO.File.ReadAllText(sitemapFile);
    }
}
