using HtmlAgilityPack;
using SpotifyAPI.Local.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slyrics.LyricFetchers
{
    class WwwLyricsFetcher : RequiredConstructor<Track>, ILyricFetcher
    {
        StringBuilder _ErrorLog;
        StringBuilder _Query;
        Track _Track;

        public WwwLyricsFetcher (Track track_) : base(track_)
        {
            _Track = track_;
            _ErrorLog = new StringBuilder();
            _Query = new StringBuilder();
        }

        public Track Track
        {
            get { return _Track; }
            set { _Track = value; }
        }

        public StringBuilder ErrorLog
        {
            get { return _ErrorLog; }
            set { _ErrorLog = value; }
        }

        public string PopLog ()
        {
            string log = ErrorLog.ToString();
            ErrorLog.Clear();
            return log;
        }

        public string Lyrics ()
        {
            StringBuilder query = new StringBuilder();
            string xpath_extract_address = "//div[contains(@class, 'lyric-meta within-lyrics')]";
            string xpath_extract_lyrics = "//pre[contains(@id, 'lyric-body-text')]";
            string lyrics = "";
            string lyric_address = "";
            HtmlWeb web = new HtmlWeb();

            // build search query
            query.Append("https://www.lyrics.com/lyrics/");
            query.Append(Uri.EscapeUriString(Track.TrackResource.Name));

            // fetch the address that contains the lyrics be doing a search query on the website
            try
            {
                HtmlNodeCollection nodes = web.Load(query.ToString()).DocumentNode.SelectNodes(xpath_extract_address);
                
                foreach (HtmlNode node in nodes)
                {
                    var artist = node.SelectSingleNode(".//p[contains(@class, 'lyric-meta-artists')]").InnerText;
                    var href = node.SelectSingleNode(".//p[contains(@class, 'lyric-meta-title')]/a").Attributes["href"].Value;
                    int levDist = HelpFunctions.LevenshteinDistance(artist, Track.ArtistResource.Name);
                    if (levDist < 3)
                    {
                        lyric_address = "https://www.lyrics.com" + href;
                        break;
                    }
                }
            }
            catch
            {
                throw new HtmlWebException(string.Format("Could not fetch the address for the lyrics from {0}", query.ToString()));
            }

            // fetch the lyrics from the correct address
            try
            {
                lyrics = web.Load(lyric_address).DocumentNode.SelectSingleNode(xpath_extract_lyrics).InnerText.Trim();
            }
            catch
            {
                throw new HtmlWebException(string.Format("Could not find any lyrics from {0}", lyric_address));
            }

            return lyrics;
        }
    }
}
