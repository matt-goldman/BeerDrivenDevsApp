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
        var buffer = new byte[128 * 1024]; // 128 KB buffer
        var isMoreToRead = true;
        var lastPercent = -1;

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
                // Decouple report frequency from buffer size: only update the UI when the
                // whole-percent value actually changes (~100 reports max, regardless of
                // file size or buffer). totalRead only grows, so percent never steps back.
                var percent = (int)(100 * totalRead / totalBytes);
                if (percent != lastPercent)
                {
                    lastPercent = percent;
                    progress.Report(percent / 100.0);
                }
            }

        } while (isMoreToRead);
    }

}