﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using SpotifyAPI.Local;
using SpotifyAPI.Local.Enums;
using SpotifyAPI.Local.Models;
using System.Diagnostics;
using System.Net;
using System.Xml;
using HtmlAgilityPack;

namespace slyrics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        SpotifyLyricHandler _spotify;

        public MainWindow ()
        {
            InitializeComponent();
            SpotifyLocalAPI spotify_connect = new SpotifyLocalAPI();
            if (!SpotifyLocalAPI.IsSpotifyRunning())
                return;
            if (!SpotifyLocalAPI.IsSpotifyWebHelperRunning())
                return;

            if (!spotify_connect.Connect())
                return;

            Spotify = new SpotifyLyricHandler(spotify_connect, this);
            
            UpdateUI();
            lyricsDataSetTableAdapters.LyricsTableAdapter asd = new lyricsDataSetTableAdapters.LyricsTableAdapter();
            asd.Insert("namn", "artist", "text");
        }

        internal SpotifyLyricHandler Spotify { get => _spotify; set => _spotify = value; }

        public void UpdateUI ()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.Title = String.Format("{0} - {1}", Spotify.currentArtist, Spotify.currentSong);
                textArea.Text = Spotify.currentLyrics;
            }));
        }
    }
}
