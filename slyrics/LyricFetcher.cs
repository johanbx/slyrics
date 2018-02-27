using SpotifyAPI.Local.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace slyrics
{
    public abstract class RequiredConstructor<T>
    {
        public RequiredConstructor (T parameters) { }
    }

    interface ILyricFetcher
    {
        StringBuilder ErrorLog { get; set; }

        Track Track { get; set; }

        string PopLog ();

        string Lyrics ();
    }
}
