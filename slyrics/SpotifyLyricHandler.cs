using HtmlAgilityPack;
using SpotifyAPI.Local;
using SpotifyAPI.Local.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slyrics
{
    class SpotifyLyricHandler
    {
        SpotifyLocalAPI _spotify;
        MainWindow mainWindow;

        public string currentSong;
        public string currentArtist;
        public string currentLyrics;

        public SpotifyLyricHandler (SpotifyLocalAPI spotify_connect, MainWindow mainWindow_)
        {
            currentSong = "None";
            currentArtist = "None";
            currentLyrics = "None";

            mainWindow = mainWindow_;
            _spotify = spotify_connect;
            _spotify.ListenForEvents = true;
            _spotify.OnTrackChange += ChangeLyrics;

            Update();
        }

        private void Update()
        {
            UpdateSongInfo();

            currentSong = currentSong == "Loading..." ? "Failed" : currentSong;
            currentArtist = currentArtist == "Loading..." ? "Failed" : currentArtist;
            currentLyrics = currentLyrics == "Loading..." ? "Failed" : currentLyrics;

            mainWindow.UpdateUI();
        }

        private void ChangeLyrics (object sender, SpotifyAPI.Local.TrackChangeEventArgs e)
        {
            Update();
        }

        private string FetchLyrics (Track newTrack)
        {
            StringBuilder lyrics = new StringBuilder();
            ILyricFetcher azlyricsFetcher = new AzlyricsFetcher(newTrack);

            try
            {
                lyrics.Append(azlyricsFetcher.Lyrics());
            }
            catch
            {
                lyrics.AppendLine("failed to fetch lyrics");
                lyrics.Append(azlyricsFetcher.PopLog());
            }

            return lyrics.ToString();
        }

        private void UpdateSongInfo ()
        {
            currentSong = "Loading...";
            currentArtist = "Loading...";
            currentLyrics = "Loading...";

            StatusResponse status = _spotify.GetStatus();
            currentSong = status.Track.TrackResource.Name;
            currentArtist = status.Track.ArtistResource.Name;
            currentLyrics = FetchLyrics(status.Track);
        }
    }
}
