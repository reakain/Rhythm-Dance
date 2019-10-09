/* 
Copyright (c) 2019 Reakain & Bandit Bots LLC

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE
OR OTHER DEALINGS IN THE SOFTWARE.
*/

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
        public int note;
        public float position = 0.0f;
    }

    [Serializable]
    public class TrackerBeat : PlayerBeat
    {
        public bool hasPassed { get; private set; }
        public float score { get; private set; }

        public TrackerBeat(int beatNote, float beatPosition)
        {
            note = beatNote;
            position = beatPosition;
            hasPassed = false;
            score = 0.0f;
        }

        public TrackerBeat(PlayerBeat beat)
        {
            note = beat.note;
            position = beat.position;
            hasPassed = false;
            score = 0.0f;
        }

        public bool IsPressed(int poseButton, float songPosition, float pressRange)
        {
            var posDif = Math.Abs(position - songPosition);
            if (poseButton == note && posDif < pressRange)
            {
                score = posDif;
                return true;
            }
            return false;
        }

        public bool IsPassed(float songPosition, float pressRange)
        {
            var posDif = position - songPosition;
            if (posDif < -pressRange)
            {
                hasPassed = true;
                return true;
            }
            return false;
        }

        public bool IsPressedOrPassed(int poseButton, float songPosition, float pressRange)
        {
            bool presspass = false;
            presspass = IsPressed(poseButton, songPosition, pressRange);
            presspass = (presspass || IsPassed(songPosition, pressRange)) ? true : false;

            return presspass;
        }
    }

    [Serializable]
    public class Note
    {
        public int index = 0;
        public int move = 0;
    }
    #endregion

}