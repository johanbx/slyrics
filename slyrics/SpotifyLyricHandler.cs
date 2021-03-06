﻿using HtmlAgilityPack;
using slyrics.LyricFetchModules;
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

        public const string STRING_NONE = "NONE";
        public const string STRING_LOADING = "Loading...";
        public const string STRING_FAILED = "Failed";
        public const string STRING_LYRIC_FETCH_FAIL_MSG = "Failed to get lyrics, sorry :(";

        public SpotifyLyricHandler (SpotifyLocalAPI spotify_connect, MainWindow mainWindow_)
        {
            currentSong = STRING_NONE;
            currentArtist = STRING_NONE;
            currentLyrics = STRING_NONE;

            mainWindow = mainWindow_;
            _spotify = spotify_connect;
            _spotify.ListenForEvents = true;
            _spotify.OnTrackChange += ChangeLyrics;
        }

        public void Update()
        {
            currentSong = STRING_LOADING;
            currentArtist = STRING_LOADING;
            currentLyrics = STRING_LOADING;

            mainWindow.UpdateUI();

            StatusResponse status = _spotify.GetStatus();
            currentSong = status.Track.TrackResource.Name;
            currentArtist = status.Track.ArtistResource.Name;
            mainWindow.UpdateUI();

            currentLyrics = FetchLyrics(status.Track);

            if (currentSong == STRING_LOADING)
                currentSong = STRING_FAILED;

            if (currentArtist == STRING_LOADING)
                currentArtist = STRING_FAILED;

            if (currentLyrics == STRING_LOADING)
                currentLyrics = STRING_FAILED;

            mainWindow.UpdateUI();
        }

        private void ChangeLyrics (object sender, SpotifyAPI.Local.TrackChangeEventArgs e)
        {
            Update();
        }

        private string FetchLyrics (Track newTrack)
        {
            LyricFetcher[] lyricFetcherPool =
            {
                new MusixmatchQueryNameArtist(),
                new AzlyricQueryNameArtist(),
                new MusixmatchQueryPartlyNameArtist(),
                new AzlyricQueryPartlyNameArtist(),
                new MusixmatchQueryName(),

                // new AzlyricQueryName(),
                // new AzlyricQueryPartlyName()
            };

            foreach (LyricFetcher lyricFetcher in lyricFetcherPool)
            {
                Debug.WriteLine(String.Format("Fetching with: {0}", lyricFetcher.GetType()));

                try
                {
                    LyricFetcherItem item = lyricFetcher.Lyric(newTrack);
                    if (item.status)
                        return item.lyric;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            return STRING_LYRIC_FETCH_FAIL_MSG;
        }
    }
}
