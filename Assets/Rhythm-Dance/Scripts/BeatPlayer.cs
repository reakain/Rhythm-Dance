using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmDance
{
    public class BeatPlayer : MonoBehaviour
    {
        public AudioClip beatChord;

        public AudioSource audio;

        public int beatsInAudio = 16;

        AudioClip[] beatClips;

        //Current song position, in seconds
        public float songPosition;

        public float dspSongTime;

        bool recordSong = false;

        public float recordStopTime = 16.0f;
        

        public List<PlayerNote> playerSongInput;

        private void Awake()
        {
            beatClips = AudioSplitter.BeatSplit(beatChord, beatsInAudio);
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
            var note = -1; // invalid value to detect when note is pressed
            if (Input.GetKeyDown("a")) note = 1;  // C
            if (Input.GetKeyDown("s")) note = 3;  // D
            if (Input.GetKeyDown("d")) note = 5;  // E
            if (Input.GetKeyDown("f")) note = 7;  // F
            if (Input.GetKeyDown("g")) note = 9;  // G
            if (Input.GetKeyDown("h")) note = 11;  // A
            if (Input.GetKeyDown("j")) note = 13; // B
            if (Input.GetKeyDown("k")) note = 15; // C

            if (note >= 0)
            { // if some key pressed...
                audio.clip = beatClips[note];
                audio.Play();
                if (recordSong)
                {
                    songPosition = (float)(AudioSettings.dspTime - dspSongTime);
                    playerSongInput.Add(new PlayerNote() { note = note, position = songPosition });
                }
            } 
            if(recordSong && songPosition >= recordStopTime)
            {
                StopRecordSong();
            }
        }

        void StartRecordSong()
        {
            dspSongTime = (float)(AudioSettings.dspTime);
            playerSongInput = new List<PlayerNote>();
            recordSong = true;
        }

        void StopRecordSong()
        {
            recordSong = false;
            AudioClip playerSong = AudioSplitter.MakeClipFromPlayer(beatClips, playerSongInput, recordStopTime);
        }
    }
}