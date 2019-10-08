using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmDance
{
    public class Conductor : MonoBehaviour
    {
        //Song beats per minute
        //This is determined by the song you're trying to sync up to
        public float songBpm;

        //The number of seconds for each song beat
        public float secPerBeat;

        //Current song position, in seconds
        public float songPosition;

        //Current song position, in beats
        public float songPositionInBeats;

        //How many seconds have passed since the song started
        public float dspSongTime;

        //an AudioSource attached to this GameObject that will play the music.
        public AudioSource musicSource;

        //The offset to the first beat of the song in seconds
        public float firstBeatOffset;

        //the number of beats in each loop
        public float beatsPerLoop;

        //the total number of loops completed since the looping clip first started
        public int completedLoops = 0;

        //The current position of the song within the loop in beats.
        public float loopPositionInBeats;

        //The current relative position of the song within the loop measured between 0 and 1.
        public float loopPositionInAnalog;

        //Conductor instance
        public static Conductor instance;

        // States
        [Serializable]
        public enum ConductorState
        {
            Stop,
            Play,
            Record,
            Playback
        }
        private ConductorState currentState;

        // Beat Play variables
        public AudioSource playerSource; // Player input audio

        // Music Recording variables
        public float recordStopTime = 16.0f;
        public List<PlayerNote> playerSongInput;

        // Instruments
        public Instrument playerInstrument;

        void Awake()
        {
            instance = this;
            playerInstrument = InstrumentLibrary.GetInstrumentFromName("Fingered Bass");
        }

        void ChangeInstrument()
        {

        }

        void Start()
        {
            SetConductor();
            StartConducting();
        }

        void SetConductor()
        {
            //Load the AudioSource attached to the Conductor GameObject
            musicSource = GetComponent<AudioSource>();

            currentState = ConductorState.Stop;
        }

        // Update is called once per frame
        void Update()
        {
            var note = GetPlayerInput();
            if (currentState == ConductorState.Stop) { return; }

            //determine how many seconds since the song started
            songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);

            if (note >= 0)
            {
                playerSource.clip = playerInstrument.Notes[note];
                playerSource.Play();
                if (currentState == ConductorState.Record)
                {
                    songPosition = (float)(AudioSettings.dspTime - dspSongTime);
                    playerSongInput.Add(new PlayerNote() { note = note, position = songPosition });
                }
            }
            if (currentState == ConductorState.Record && songPosition >= recordStopTime)
            {
                StopRecord();
            }

            if (currentState == ConductorState.Play || currentState == ConductorState.Playback)
            {
                //determine how many beats since the song started
                songPositionInBeats = songPosition / secPerBeat;

                //calculate the loop position
                if (songPositionInBeats >= (completedLoops + 1) * beatsPerLoop)
                    completedLoops++;
                loopPositionInBeats = songPositionInBeats - completedLoops * beatsPerLoop;

                loopPositionInAnalog = loopPositionInBeats / beatsPerLoop;
            }
        }

        int GetPlayerInput()
        {
            int note = -1; // invalid value to detect when note is pressed
            switch (currentState)
            {
                case ConductorState.Stop:
                    if (Input.GetKeyDown("r")) { StartRecord(); }
                    if (Input.GetKeyDown("space")) { StartSong(); }
                    break;
                case ConductorState.Play:
                    if (Input.GetKeyDown("space")) { StopSong(); }
                    break;
                case ConductorState.Playback:
                    if (Input.GetKeyDown("space")) { StopSong(); }
                    break;
                case ConductorState.Record:
                    if (Input.GetKeyDown("space")) { StopRecord(); }
                    else
                    {
                        if (Input.GetKeyDown("a")) note = 1;  // C
                        if (Input.GetKeyDown("s")) note = 3;  // D
                        if (Input.GetKeyDown("d")) note = 5;  // E
                        if (Input.GetKeyDown("f")) note = 7;  // F
                        if (Input.GetKeyDown("g")) note = 9;  // G
                        if (Input.GetKeyDown("h")) note = 11;  // A
                        if (Input.GetKeyDown("j")) note = 13; // B
                        if (Input.GetKeyDown("k")) note = 15; // C
                    }
                    break;
                default:
                    break;
            }
            return note;
        }

        void StartSong()
        {
            //Calculate the number of seconds in each beat
            secPerBeat = 60f / songBpm;

            StartConducting();

            //Start the music
            musicSource.Play();

            currentState = ConductorState.Play;
        }

        void StopSong()
        {
            musicSource.Stop();

            if(playerSource != null)
            {
                playerSource.Stop();
            }

            currentState = ConductorState.Stop;
        }

        void StartRecord()
        {
            playerSource.loop = false;
            playerSongInput = new List<PlayerNote>();

            StartConducting();
            
            currentState = ConductorState.Record;
        }

        void StopRecord()
        {
            playerSource.clip = AudioSplitter.MakeClipFromPlayer(playerInstrument.Notes, playerSongInput, songPosition);
            StartPlayback();
        }

        void StartPlayback()
        {
            currentState = ConductorState.Playback;

            StartConducting();

            playerSource.loop = true;
            playerSource.Play();
        }

        void StartConducting()
        {
            //Record the time when the music starts
            dspSongTime = (float)AudioSettings.dspTime;
        }

        
    }
}