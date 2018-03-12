using SpotifyAPI.Local.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slyrics.LyricFetchModules
{
    public class MusixmatchQueryName : Musixmatch
    {
        public override LyricFetcherItem Lyric (Track track)
        {
            string songName = track.TrackResource.Name.Replace(' ', '+');
            string searchQuery = BASE_QUERY + songName;

            return GetLyricFromMusixmatch(searchQuery);
        }
    }
}
