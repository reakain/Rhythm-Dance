using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmDance
{
    [Serializable]
    public struct PlayerNote
    {
        public int note;
        public float position;
    }

    [Serializable]
    public enum Instruments
    {
        RainDrop,
        SoftBass
    }

    

    [Serializable]
    public static class AudioSplitter
    {
        public static AudioClip[] BeatSplit(AudioClip clip, int beatSplit)
        {
            AudioClip[] splitClips = new AudioClip[beatSplit];
            float beatLength = clip.length / beatSplit;
            for (int i = 0; i < beatSplit; i++)
            {
                splitClips[i] = MakeSubclip(clip, i * beatLength, (i + 1) * beatLength);
            }
            return splitClips;
        }
        /**
       * Creates a sub clip from an audio clip based off of the start time
       * and the stop time. The new clip will have the same frequency as
       * the original.
       */
        private static AudioClip MakeSubclip(AudioClip clip, float start, float stop)
        {
            /* Create a new audio clip */
            int frequency = clip.frequency;
            float timeLength = stop - start;
            int samplesLength = (int)(frequency * timeLength);
            AudioClip newClip = AudioClip.Create(clip.name + "-sub", samplesLength, 1, frequency, false);
            /* Create a temporary buffer for the samples */
            float[] data = new float[samplesLength];
            /* Get the data from the original clip */
            clip.GetData(data, (int)(frequency * start));
            /* Transfer the data to the new clip */
            newClip.SetData(data, 0);
            /* Return the sub clip */
            return newClip;
        }

        public static AudioClip MakeClipFromPlayer(AudioClip[] notes, List<PlayerNote> songNotes, float recordTime)
        {
            int frequency = notes[0].frequency;
            int samplesLength = (int)(frequency * recordTime);
            AudioClip playerClip = AudioClip.Create(notes[0].name + "-player", samplesLength, 1, frequency, false);
            foreach(var songNote in songNotes)
            {
                playerClip = AddClipData(playerClip, notes[songNote.note], songNote.position, frequency);
            }
            return playerClip;
        }

        public static AudioClip AddClipData(AudioClip song, AudioClip note, float position, int frequency)
        {
            float[] data = new float[(int)(note.length * frequency)];
            note.GetData(data, 0);
            song.SetData(data, (int)(position * frequency));
            return song;
        }
    }
}