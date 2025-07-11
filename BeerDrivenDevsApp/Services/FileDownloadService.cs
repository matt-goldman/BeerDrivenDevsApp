namespace BeerDrivenDevsApp.Services;

public interface IFileDownloadService
{
    Task DownloadFileAsync(string url, string destinationPath, IProgress<double> progress,
        CancellationToken cancellationToken);
}
public class FileDownloadService(HttpClient httpClient)  : IFileDownloadService
{
    public async Task DownloadFileAsync(string url, string destinationPath, IProgress<double> progress, CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();

        var totalBytes = response.Content.Headers.ContentLength ?? -1L;
        var totalRead = 0L;
        var buffer = new byte[8192];
        var isMoreToRead = true;

        using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var fileStream = File.OpenWrite(destinationPath);

        do
        {
            var read = await contentStream.ReadAsync(buffer.AsMemory(0, buffer.Length), cancellationToken);
            if (read == 0)
            {
                isMoreToRead = false;
                progress.Report(1.0);
                continue;
            }

            await fileStream.WriteAsync(buffer.AsMemory(0, read), cancellationToken);

            totalRead += read;

            if (totalBytes != -1)
            {
                var percent = (double)totalRead / totalBytes;
                progress.Report(percent);
            }

        } while (isMoreToRead);
    }

}