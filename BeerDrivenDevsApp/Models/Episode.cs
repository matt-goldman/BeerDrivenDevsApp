namespace BeerDrivenDevsApp.Models;

public class Episode
{
    public string EpisodeNumber { get; set; } = string.Empty;
    
    public DateTime ReleasedOn { get; set; }
    
    public string Title { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    public string Notes { get; set; }  = string.Empty;

    public string ThumbnailUrl { get; set; }  = string.Empty;

    public string AudioUrl { get; set; } = string.Empty;

    public bool IsDownloaded { get; set; } = false;
    
    public double DownloadProgress { get; set; } = 0.0;

    public string Duration { get; set; } =  string.Empty;
}