using System.Xml.Serialization;
using BeerDrivenDevsApp.Models;

namespace BeerDrivenDevsApp.Utils;

public static class BddFeedDeserializer
{
    private static XmlSerializer _serializer = new XmlSerializer(typeof(Rss));
    
    public static List<Episode> DeserializeFeed(string feedData)
    {
        Rss? feed;

        List<Episode> episodes = [];
            
        try
        {
            feed = _serializer.Deserialize(new StringReader(feedData)) as Rss;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return [];
        }

        if (feed is not null)
        {
            episodes.AddRange(feed.Channel.Item.Select(item => new Episode
            {
                Title           = item.Title,
                AudioUrl        = item.Enclosure.Url,
                ReleasedOn      = DateTime.TryParse(item.PubDate, out var pubDate) ? pubDate : DateTime.Today,
                Summary         = item.Description,
                Notes           = item.Encoded,
                ThumbnailUrl    = item.Image,
                Duration        = item.Duration,
                EpisodeId   = int.TryParse(item.EpisodeNumber, out var result) ? result : 0,
                IsDownloaded    = false
            }));
        }
        
        return episodes;
    }
}