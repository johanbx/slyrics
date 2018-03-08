using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Local.Models;

namespace slyrics.LyricFetchModules
{
    class AzlyricQueryNameArtist : Azlyrics
    {
        public override LyricFetcherItem Lyric (Track track)
        {
            string songName = track.TrackResource.Name.Replace(' ', '+');
            string artistName = track.ArtistResource.Name.Replace(' ', '+');
            string searchQuery = BASE_QUERY + songName + "+by+" + artistName;

            return GetLyricFromAzyl(searchQuery);
        }
    }
}
