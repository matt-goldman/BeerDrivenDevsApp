namespace BeerDrivenDevsApp.Controls;

public partial class EpisodeCard : ContentView
{
    public event EventHandler? DownloadButtonClicked;

    public EpisodeCard()
    {
        InitializeComponent();
    }

    public void OnDownloadButtonClicked(object sender, EventArgs e)
    {
        DownloadButtonClicked?.Invoke(this, EventArgs.Empty);
    }
}