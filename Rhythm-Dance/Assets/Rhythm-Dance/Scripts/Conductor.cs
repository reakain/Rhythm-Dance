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
        [SerializeField]
        public ConductorState currentState { get; private set; }

        public delegate void ConductorStateEvent();
        public static event ConductorStateEvent ConductorStateChange;

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

        void Start()
        {
            SetConductor();
            StartConducting();
        }

        void SetConductor()
        {
            //Load the AudioSource attached to the Conductor GameObject
            musicSource = GetComponent<AudioSource>();

            ChangeState(ConductorState.Stop);
        }

        void ChangeState(ConductorState state)
        {
            currentState = state;
            if (ConductorStateChange != null)
            {
                ConductorStateChange();
            }
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
                if (currentState == ConductorState.Play)
                {
                    CheckNoteBeat(note);
                }
                if (currentState == ConductorState.Record)
                {
                    songPosition = (float)(AudioSettings.dspTime - dspSongTime);
                    playerSongInput.Add(new PlayerNote() { note = note, position = songPosition });
                }
                playerSource.clip = playerInstrument.Notes[note];
                playerSource.Play();  
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

        void CheckNoteBeat(int note)
        {

        }

        int GetPlayerInput()
        {
            int note = -1; // invalid value to detect when note is pressed
            switch (currentState)
            {
                case ConductorState.Stop:
                    if (Input.GetButtonDown(KeysInput.instance.startRecordButton)) { StartRecord(); }
                    if (Input.GetButtonDown(KeysInput.instance.startSongButton)) { StartSong(); }
                    break;
                case ConductorState.Play:
                    if (Input.GetButtonDown(KeysInput.instance.stopSongButton)) { StopSong(); }
                    else
                    {
                        if (Input.GetButtonDown(KeysInput.instance.pose1Key))
                        { note = KeysInput.instance.poseKeyNotes[0]; }
                        if (Input.GetButtonDown(KeysInput.instance.pose2Key))
                        { note = KeysInput.instance.poseKeyNotes[1]; }
                        if (Input.GetButtonDown(KeysInput.instance.pose3Key))
                        { note = KeysInput.instance.poseKeyNotes[2]; }
                        if (Input.GetButtonDown(KeysInput.instance.pose4Key))
                        { note = KeysInput.instance.poseKeyNotes[3]; }
                    }
                    break;
                case ConductorState.Playback:
                    if (Input.GetButtonDown(KeysInput.instance.stopSongButton)) { StopSong(); }
                    break;
                case ConductorState.Record:
                    if (Input.GetButtonDown(KeysInput.instance.stopRecordButton)) { StopRecord(); }
                    else
                    {
                        if (Input.GetButtonDown(KeysInput.instance.pose1Key))
                        { note = KeysInput.instance.poseKeyNotes[0]; }
                        if (Input.GetButtonDown(KeysInput.instance.pose2Key))
                        { note = KeysInput.instance.poseKeyNotes[1]; }
                        if (Input.GetButtonDown(KeysInput.instance.pose3Key))
                        { note = KeysInput.instance.poseKeyNotes[2]; }
                        if (Input.GetButtonDown(KeysInput.instance.pose4Key))
                        { note = KeysInput.instance.poseKeyNotes[3]; }
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

                ChangeState(ConductorState.Play);
        }

        void StopSong()
        {
            musicSource.Stop();

            if(playerSource != null)
            {
                playerSource.Stop();
            }

                ChangeState(ConductorState.Stop);
        }

        void StartRecord()
        {
            playerSource.loop = false;
            playerSongInput = new List<PlayerNote>();

            StartConducting();

                ChangeState(ConductorState.Record);
        }

        void StopRecord()
        {
            playerSource.clip = AudioSplitter.MakeClipFromPlayer(playerInstrument.Notes, playerSongInput, songPosition);
            StartPlayback();
        }

        void StartPlayback()
        {
                ChangeState(ConductorState.Playback);

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