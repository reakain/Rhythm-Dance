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