using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Local.Models;

namespace slyrics.LyricFetchModules
{
    class AzlyricQueryName : Azlyrics
    {
        public override LyricFetcherItem Lyric (Track track)
        {
            string songName = track.TrackResource.Name.Replace(' ', '+');
            string searchQuery = BASE_QUERY + songName;

            return GetLyricFromAzyl(searchQuery);
        }
    }
}
