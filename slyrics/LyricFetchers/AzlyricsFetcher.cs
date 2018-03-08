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

            StringBuilder wholeNameQuery = new StringBuilder();
            StringBuilder partNameQuery = new StringBuilder();
            StringBuilder onlySongName = new StringBuilder();
            StringBuilder partNameNoArtistQuery = new StringBuilder();

            string baseQuery = "https://search.azlyrics.com/search.php?q=";
            string finalLyrics = null;
            string finalErrorMsg = null;

            // Search query with whole name
            wholeNameQuery.Append(baseQuery);
            wholeNameQuery.Append(_Track.TrackResource.Name.Replace(' ', '+'));
            wholeNameQuery.Append("+by+");
            wholeNameQuery.Append(_Track.ArtistResource.Name.Replace(' ', '+'));

            // Search query with the part before '-'
            int indexOfSplitChar = _Track.TrackResource.Name.LastIndexOf(" -");
            if (indexOfSplitChar != -1)
            {
                
                string partName = _Track.TrackResource.Name.Substring(0, indexOfSplitChar);
                partNameQuery.Append(baseQuery);
                partNameQuery.Append(partName.Replace(' ', '+'));
                partNameQuery.Append("+by+");
                partNameQuery.Append(_Track.ArtistResource.Name.Replace(' ', '+'));

                // Seach query with the part before '-' and with no artist (can get bad result)
                //TODO try have this as a last resort when fetching lyrics from other sources
                partNameNoArtistQuery.Append(baseQuery);
                partNameNoArtistQuery.Append(partName.Replace(' ', '+'));
            }

            // Search without artist (can get bad result)
            //TODO try have this as a last resort when fetching lyrics from other sources
            onlySongName.Append(baseQuery);
            onlySongName.Append(_Track.TrackResource.Name.Replace(' ', '+'));

            // The order of the searches
            queries.Add(wholeNameQuery);
            queries.Add(partNameQuery);
            queries.Add(onlySongName);
            queries.Add(partNameNoArtistQuery);

            // Loop through our queries and try to find a result
            foreach (StringBuilder query in queries)
            {
                if (query.Length < 1)
                    continue;

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
