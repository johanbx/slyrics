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
    class AzlyricsFetcher : RequiredConstructor<Track>, ILyricFetcher
    {
        StringBuilder _ErrorLog;
        StringBuilder _Query;
        Track _Track;

        public AzlyricsFetcher (Track track_) : base (track_)
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
            string xpath_extract_address = "//table[contains(@class, 'table table-condensed')]/tr/td[contains(@class, 'text-left visitedlyr')]//a";
            string xpath_extract_lyrics = "//div[contains(@class, 'col-xs-12 col-lg-8 text-center')]/div[not(@class)]";
            string address;
            string lyrics;
            HtmlWeb web = new HtmlWeb();
            HtmlNode node;

            // Build search query
            query.Append("https://search.azlyrics.com/search.php?q=");
            query.Append(_Track.TrackResource.Name.Replace(' ', '+'));
            query.Append("+by+");
            query.Append(_Track.ArtistResource.Name.Replace(' ', '+'));

            // Try to fetch address for lyrics by using the search
            try
            {
                address = Uri.EscapeUriString(query.ToString());
                node = web.Load(address).DocumentNode.SelectSingleNode(xpath_extract_address);
            } catch
            {
                throw new HtmlWebException(string.Format("Could not fetch the address for the lyrics from {0}", query.ToString()));
            }
            

            // Once we fond the address, try to fetch the lyrics
            try
            {
                address = node.Attributes["href"].Value;
                node = web.Load(address).DocumentNode.SelectSingleNode(xpath_extract_lyrics);
            }
            catch
            {
                throw new HtmlWebException(string.Format("Could not fetch the address for the lyrics from {0}", address.ToString()));
            }

            // Tidy upp the lyrics
            lyrics = HtmlEntityTranslator.Descape(node.InnerText.Trim());

            return lyrics;
        }
    }
}
