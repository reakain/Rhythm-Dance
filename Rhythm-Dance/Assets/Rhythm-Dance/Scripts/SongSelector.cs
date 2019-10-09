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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmDance
{
    public class SongSelector : MonoBehaviour
    {
        string[] songList;
        Song currentSong;
        public int currentSongNum = 0;

        public TMPro.TextMeshProUGUI songLabel;
        // Start is called before the first frame update
        void Start()
        {
            songList = SongLibrary.GetSongList();
            songLabel = GetComponent<TMPro.TextMeshProUGUI>();
            UpdateSong();
        }

        void UpdateSong()
        {
            songLabel.text = songList[currentSongNum];
            currentSong = SongLibrary.GetSongInfo(songList[currentSongNum]);

            Conductor.instance.musicSource.clip = Resources.Load<AudioClip>("Songs/" + currentSong.source);
            Conductor.instance.songBpm = currentSong.bpm;
            Conductor.instance.firstBeatOffset = currentSong.offset;
            Conductor.instance.playerInstrument = InstrumentLibrary.GetInstrumentFromName(currentSong.player_instrument);
            KeysInput.instance.SetKeys(currentSong.keys);
            Conductor.instance.SetTrackerBeats(currentSong.player_beats);
            
        }

        // Update is called once per frame
        void Update()
        {
            if (KeysInput.instance.upButton)
            {
                currentSongNum -= 1;
                UpdateSong();
            }
            if (KeysInput.instance.downButton)
            {
                currentSongNum += 1;
                UpdateSong();
            }

        }
    }
}