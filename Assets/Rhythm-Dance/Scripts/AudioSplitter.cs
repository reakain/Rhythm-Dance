using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioSplitter
{
    public static AudioClip[] BeatSplit(AudioClip clip, int beatSplit)
    {
        AudioClip[] splitClips = new AudioClip[beatSplit];
        float beatLength = clip.length / beatSplit;
        for(int i = 0; i < beatSplit; i++)
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
}
