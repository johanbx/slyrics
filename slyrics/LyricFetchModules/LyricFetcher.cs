using ScrapySharp.Network;
using SpotifyAPI.Local.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slyrics.LyricFetchModules
{
    public abstract class LyricFetcher
    {
        const string NO_LYRIC_MSG = "No lyric has been loaded yet";

        protected LyricFetcherItem currentLyric;
        protected ScrapingBrowser browser;

        public LyricFetcher ()
        {
            browser = new ScrapingBrowser();
            currentLyric = new LyricFetcherItem();
        }

        public abstract LyricFetcherItem Lyric (Track track);
    }

    public class LyricFetcherItem
    {
        public string searchQuery;
        public string lyric;
        public string lyricHref;
        public bool status;

        public LyricFetcherItem ()
        {
            this.searchQuery = null;
            this.lyric = null;
            this.status = false;
        }

        public LyricFetcherItem (string lyric, string searchQuery, string lyricHref, bool status)
        {
            this.lyric = lyric;
            this.searchQuery = searchQuery;
            this.searchQuery = lyricHref;
            this.status = status;

            Debug.WriteLine(
                String.Format("LyricFetcherItem created with:" +
                "\nstatus'{0}'" +
                "\nsearchQuery '{1}'" +
                "\nlyricHref {2}", 
                status, searchQuery, lyricHref)
            );
        }
    }
}
