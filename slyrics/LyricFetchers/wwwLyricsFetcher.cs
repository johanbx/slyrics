using SpotifyAPI.Local.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slyrics.LyricFetchers
{
    class WwwLyricsFetcher : RequiredConstructor<Track>, ILyricFetcher
    {
        public WwwLyricsFetcher (Track parameters) : base(parameters)
        {
        }

        public StringBuilder ErrorLog { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public StringBuilder Query { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string ExtractLyricAddress ()
        {
            throw new NotImplementedException();
        }

        public string ExtractLyrics (string address)
        {
            throw new NotImplementedException();
        }

        public string FinalizeLyrics (string lyrics)
        {
            throw new NotImplementedException();
        }

        public string Lyrics ()
        {
            throw new NotImplementedException();
        }

        public string PopLog ()
        {
            throw new NotImplementedException();
        }
    }
}
