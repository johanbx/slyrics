using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapySharp.Network;
using ScrapySharp.Extensions;
using SpotifyAPI.Local.Models;
using HtmlAgilityPack;

namespace slyrics.LyricFetchModules
{
    public abstract class Musixmatch : LyricFetcher
    {
        protected const string BASE_HREF = "https://www.musixmatch.com";
        protected const string BASE_QUERY = BASE_HREF + "/search/";
        protected const string HREF_CSS = "ul.tracks.list h2.media-card-title a";
        protected const string LYRIC_CSS = "div.mxm-lyrics span p.mxm-lyrics__content";

        protected LyricFetcherItem GetLyricFromMusixmatch (string searchQuery)
        {
            string lyricHref = null;
            string lyric = null;
            bool status = false;

            try
            {
                WebPage page;

                page = browser.NavigateToPage(new Uri(searchQuery));
                lyricHref = BASE_HREF + page.Html.CssSelect(HREF_CSS).ToArray()[0].GetAttributeValue("href");
                page = browser.NavigateToPage(new Uri(lyricHref));

                HtmlNode[] lyricContainer = page.Html.CssSelect(LYRIC_CSS).ToArray();
                lyric = lyricContainer[0].InnerText + lyricContainer[1].InnerText;

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
