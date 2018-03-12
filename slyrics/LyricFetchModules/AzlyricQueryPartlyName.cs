using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Local.Models;

namespace slyrics.LyricFetchModules
{
    class AzlyricQueryPartlyName : Azlyrics
    {
        public override LyricFetcherItem Lyric (Track track)
        {
            string songName = PartByDash(track.TrackResource.Name);

            if (songName != null)
            {
                songName = songName.Replace(' ', '+');
                string searchQuery = BASE_QUERY + songName;

                return GetLyricFromAzyl(searchQuery);
            }

            return new LyricFetcherItem();
        }
    }
}
