using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapySharp.Network;
using ScrapySharp.Extensions;
using SpotifyAPI.Local.Models;
using HtmlAgilityPack;
using ScrapySharp.Html;
using System.Diagnostics;


namespace slyrics.LyricFetchModules
{
    public abstract class Azlyrics : LyricFetcher
    {
        protected const string BASE_QUERY = "https://search.azlyrics.com/search.php?q=";
        protected const string HREF_CSS = "table.table.table-condensed tr td.text-left.visitedlyr a";
        protected const string LYRIC_CSS = "div.col-xs-12.col-lg-8.text-center div[class='']";

        protected LyricFetcherItem GetLyricFromAzyl(string searchQuery)
        {
            string lyricHref = null;
            string lyric = null;
            bool status = false;

            try
            {
                WebPage page;

                page = browser.NavigateToPage(new Uri(searchQuery));
                lyricHref = page.Html.CssSelect(HREF_CSS).ToArray()[0].GetAttributeValue("href");
                page = browser.NavigateToPage(new Uri(lyricHref));

                lyric = FinalizeLyric(lyric);

                status = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                status = false;
            }

            return new LyricFetcherItem(lyric, searchQuery, lyricHref, status);
        }
    }
}
