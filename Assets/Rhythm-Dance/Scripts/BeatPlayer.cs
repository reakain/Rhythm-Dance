using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatPlayer : MonoBehaviour
{
    public AudioClip beatChord;

    public AudioSource audio;

    public int beatsInAudio = 16;

    AudioClip[] beatClips;

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
        }
    }
}
