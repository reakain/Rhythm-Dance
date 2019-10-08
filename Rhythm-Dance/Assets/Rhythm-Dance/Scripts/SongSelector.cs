﻿using System.Collections;
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
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown("up"))
            {
                currentSongNum -= 1;
                UpdateSong();
            }
            if (Input.GetKeyDown("down"))
            {
                currentSongNum += 1;
                UpdateSong();
            }

        }
    }
}