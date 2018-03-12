using SpotifyAPI.Local.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slyrics.LyricFetchModules
{
    class MusixmatchQueryNameArtist : Musixmatch
    {
        public override LyricFetcherItem Lyric (Track track)
        {
            string songName = track.TrackResource.Name.Replace(' ', '+');
            string artistName = track.ArtistResource.Name.Replace(' ', '+');
            string searchQuery = BASE_QUERY + songName + "+" + artistName;

            return GetLyricFromMusixmatch(searchQuery);
        }
    }
}
