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
            List<StringBuilder> queries = new List<StringBuilder>();
            string baseQuery = "https://search.azlyrics.com/search.php?q=";
            string finalLyrics = null;
            string finalErrorMsg = null;

            // Search query with whole name
            StringBuilder wholeName = new StringBuilder();
            wholeName.Append(baseQuery);
            wholeName.Append(_Track.TrackResource.Name.Replace(' ', '+'));
            wholeName.Append("+by+");
            wholeName.Append(_Track.ArtistResource.Name.Replace(' ', '+'));
            queries.Add(wholeName);

            // Search query with the part before '-'
            int indexOfSplitChar = _Track.TrackResource.Name.LastIndexOf(" -");
            if (indexOfSplitChar != -1)
            {
                StringBuilder partName = new StringBuilder();
                partName.Append(baseQuery);
                partName.Append(_Track.TrackResource.Name.Substring(0, indexOfSplitChar).Replace(' ', '+'));
                partName.Append("+by+");
                partName.Append(_Track.ArtistResource.Name.Replace(' ', '+'));
                queries.Add(partName);
            }

            // Search without artist (can get bad result)
            //TODO try having this as a last resort
            StringBuilder onlySongName = new StringBuilder();
            onlySongName.Append(baseQuery);
            onlySongName.Append(_Track.TrackResource.Name.Replace(' ', '+'));
            queries.Add(onlySongName);

            // Loop through our queries and try to find a result
            foreach (StringBuilder query in queries)
            {
                try
                {
                    Debug.WriteLine(string.Format("Searching with query: {0}", query));
                    string asd = query.ToString();
                    finalLyrics = FetchOneLyric(asd);
                    if (finalLyrics != null)
                    {
                        break;
                    }
                } catch (Exception ex)
                {
                    finalErrorMsg = ex.ToString();
                }
            }

            if (finalLyrics == null)
            {
                throw new HtmlWebException(finalErrorMsg);
            }

            return finalLyrics;
        }

        public string FetchOneLyric (string query)
        {
            string xpath_extract_address = "//table[contains(@class, 'table table-condensed')]/tr/td[contains(@class, 'text-left visitedlyr') and boolean(./small)]//a";
            string xpath_extract_lyrics = "//div[contains(@class, 'col-xs-12 col-lg-8 text-center')]/div[not(@class)]";
            string address;
            string lyrics;
            HtmlWeb web = new HtmlWeb();
            HtmlNode node;

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
            lyrics = HtmlEntityTranslator.RemoveBrackets(lyrics);
            return lyrics;
        }
    }
}
