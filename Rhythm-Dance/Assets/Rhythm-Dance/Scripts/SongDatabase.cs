using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmDance
{
    #region SongDatabases
    [Serializable]
    public static class SongLibrary
    {
        public static SongDatabase songLibrary = SongDatabase.CreateFromJSON(Resources.Load<TextAsset>("Songs/SongDatabase").text);

        public static Song GetSongInfo(string songTitle)
        {
            return Array.Find(songLibrary.songs, song => song.title == songTitle);
        }

        public static string[] GetSongList()
        {
            string[] list = new string[songLibrary.songs.Length];
            for(int i = 0; i < list.Length; i++)
            {
                list[i] = songLibrary.songs[i].title;
            }
            return list;
        }

    }

    [Serializable]
    public class SongDatabase
    {
        public Song[] songs;
        public static SongDatabase CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<SongDatabase>(jsonString);
        }
    }

    [Serializable]
    public class Song
    {
        public string title = "";
        public int bpm = 0;
        public string source = "";
        public float offset = 0.0f;
        public string player_instrument = "";
        public Note[] keys;
        public PlayerBeat[] player_beats;
    }

    [Serializable]
    public class PlayerBeat
    {
        public int pose;
        public float position = 0.0f;
    }

    [Serializable]
    public class Note
    {
        public int index = 0;
        public int move = 0;
    }
    #endregion

}