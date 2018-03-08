﻿using System;
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
            int indexOfSplitChar = track.TrackResource.Name.LastIndexOf(" -");

            if (indexOfSplitChar != -1)
            {
                string songName = track.TrackResource.Name.Substring(0, indexOfSplitChar).Replace(' ', '+');
                string searchQuery = BASE_QUERY + songName;

                return GetLyricFromAzyl(searchQuery);
            }

            return new LyricFetcherItem();
        }
    }
}
