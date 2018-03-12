using SpotifyAPI.Local.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slyrics.LyricFetchModules
{
    public class MusixmatchQueryPartlyNameArtist : Musixmatch
    {
        public override LyricFetcherItem Lyric (Track track)
        {
            string songName = PartByDash(track.TrackResource.Name);

            if (songName != null)
            {
                songName = songName.Replace(' ', '+');
                string artistName = track.ArtistResource.Name.Replace(' ', '+');
                string searchQuery = BASE_QUERY + songName + "+" + artistName;

                return GetLyricFromMusixmatch(searchQuery);
            }

            return new LyricFetcherItem();
        }
    }
}
