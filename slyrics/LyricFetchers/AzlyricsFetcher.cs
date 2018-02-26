using HtmlAgilityPack;
using SpotifyAPI.Local.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slyrics
{
    // illegal take on azlyrics :D
    public class AzlyricsFetcher : RequiredConstructor<Track>, ILyricFetcher
    {
        StringBuilder _ErrorLog;
        StringBuilder _Query;
        Track track;

        public AzlyricsFetcher (Track track_) : base (track_)
        {
            track = track_;
            _ErrorLog = new StringBuilder();
            _Query = new StringBuilder();
        }

        public StringBuilder ErrorLog {
            get { return _ErrorLog; }
            set { _ErrorLog = value; }
        }

        public StringBuilder Query
        {
            get { return _Query; }
            set { _Query = value; }
        }

        public string PopLog ()
        {
            string log = ErrorLog.ToString();
            ErrorLog.Clear();
            return log;
        }

        public string Lyrics ()
        {
            string address = ExtractLyricAddress();
            string lyrics = ExtractLyrics(address);
            return FinalizeLyrics(lyrics);
        }

        public string ExtractLyricAddress ()
        {
            string xpath_extract_address = "//table[contains(@class, 'table table-condensed')]/tr/td[contains(@class, 'text-left visitedlyr')]//a";

            Query.Clear();
            Query.Append("https://search.azlyrics.com/search.php?q=");
            Query.Append(track.TrackResource.Name.Replace(' ', '+'));
            Query.Append("+by+");
            Query.Append(track.ArtistResource.Name.Replace(' ', '+'));

            ErrorLog.AppendLine("Tried to fetch lyric with query: ");
            ErrorLog.AppendLine(Uri.EscapeUriString(Query.ToString()));

            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(Uri.EscapeUriString(Query.ToString()));
            var node = htmlDoc.DocumentNode.SelectSingleNode(xpath_extract_address);

            ErrorLog.AppendLine("Tried to find value attribute for node: ");
            ErrorLog.AppendLine(node.Name);

            string address = node.Attributes["href"].Value;

            ErrorLog.AppendLine("The address found for the lyric was: ");
            ErrorLog.AppendLine(address);

            return address;
        }

        public string ExtractLyrics (string address)
        {
            string xpath_extract_lyrics = "//div[contains(@class, 'col-xs-12 col-lg-8 text-center')]/div[not(@class)]";

            ErrorLog.AppendLine("Loading the page: ");
            ErrorLog.AppendLine(address);

            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(address);

            var node = htmlDoc.DocumentNode.SelectSingleNode(xpath_extract_lyrics);

            ErrorLog.AppendLine(node.ToString());

            ErrorLog.AppendLine("The node for the lyrics is: ");
            ErrorLog.AppendLine(node.Name);

            return node.InnerText.Trim();
        }

        public string FinalizeLyrics (string lyrics)
        {
            return HtmlEntityTranslator.Descape(lyrics);
        }
    }
}
