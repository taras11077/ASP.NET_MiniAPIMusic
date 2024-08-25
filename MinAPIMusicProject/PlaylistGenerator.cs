using System.Linq.Expressions;
using MinAPIMusicProject.Data;
using MinAPIMusicProject.Models;

namespace MinAPIMusicProject;

/*
 * - genres
 * - track title
 * - popularity
 * - date
 * 
 */

// pattern Builder
public class PlaylistGenerator
{
    private readonly List<Predicate<Track>> _conditions;
    private int _limit = 10;
    private string _title;

    public PlaylistGenerator()
    {
        _conditions = new List<Predicate<Track>>();
    }
    
    public PlaylistGenerator AddGenreRule(IEnumerable<string> genres)
    {
        _conditions.Add(t => genres.Contains(t.Genre.Name.ToLower()));
        return this;
    } 
    
    public PlaylistGenerator AddTrackTitleRule(IEnumerable<string> keyWords)
    {
        _conditions.Add(t =>
        {
            foreach (var word in keyWords)
            {
                if (t.Title.ToLower().Contains(word))
                    return true;
            }

            return false;
        });
        return this;
    } 
    
    public PlaylistGenerator AddPopularityRule(int? min = null, int? max = null)
    {
        if (min != null || max != null)
        {
            _conditions.Add(t => t.Listened >= (min ?? 0) && t.Listened <= (max ?? int.MaxValue));
        }
        return this;
    } 
    
    public PlaylistGenerator AddDateRule(DateTime? startDate = null, DateTime? endDate = null)
    {
        if (startDate != null && endDate != null)
        {
            _conditions.Add(t => t.CreatedAt >= (startDate ?? DateTime.MinValue) && t.CreatedAt <= (endDate ?? DateTime.Now));
        }

        return this;
    }
    
    public PlaylistGenerator SetTitle(string title)
    {
        _title = title;
        return this;
    }

    public PlaylistGenerator SetLimit(int limit)
    {
        _limit = limit;
        return this;
    }

    public Playlist Build(MusicContext context)
    {
        var tracks = context.Tracks.ToList().AsEnumerable();

        _conditions.ForEach(condition => tracks = tracks.Where(x => condition(x)));
        tracks = tracks.Take(_limit);
        
        var playlist = new Playlist()
        {
            Title = _title,
            Tracks = tracks.ToList(),
            User = context.Users.First(x => x.Login == "system")
        };

        return playlist;
    }
}