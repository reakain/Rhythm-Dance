# Rhythm Dance
[![Build Status](https://api.travis-ci.com/reakain/Rhythm-Dance.svg?branch=master)](https://travis-ci.com/reakain/Rhythm-Dance)
[![unity version](https://img.shields.io/badge/unity%20version-2019.1.14f1-green.svg)]()
Unity build out for rhythm based dance mini-game and tune-maker. 

## What is?
A simple Unity project to implement a dance-rhythm mini-game and accompanying dance/tune maker. Note this is currently hard-coded for 4 player input poses, despite the Json file being quite flexible on input numbers, and requires audio and information for both songs and instruments to be added by hand to the json files. [BeepBox](https://www.beepbox.com/) is a great resource for making both song and instrument audio clips.

### Song JSON Database
The song information is stored in a JSON file in the same folder as all the song source files. It has a number of inputs that are parsed to different effect. The Json file and audio clips are stored in ```Rhythm-Dance/Resources/Songs``` and an example set is already there for your use. The Json file has the following format.
```
{
    "songs": [
      {
        "title": "Song_Title",
        "bpm": "Song_Beats_Per_Minute",
        "source": "Song_FileName_No_Extension",
      "offset": "Song_Actual_Start_Offset_From_File_Start_In_Float_Seconds",
      "player_instrument": "Name_of_Player_Instrument_To_Use",
      "keys": [
        {
          "index": "The index note number from the player instrument for this player input button",
          "move": "Associated player input button index (used as array position, so starts with 0)"
        }
      ],
        "player_beats": [
            {
              "note": "The index of the isntrument note the user must play (Should correspond to an index value in keys",
                "position": "Position in song in float seconds"
              }
        ]
    }
    ]
}
```
The maybe-less-clear parts are described in greater detail below.

##### bpm
The beats per minute is an int filetype in code, and are used for song tracking as well as for matching animations to the song beat without needing to tweak your animation sprite for each song.

##### offset
This is a float value that's intended as seconds. Basically it helps wrap past the dead time that's often at the start of a song file, so that is then not included in the song position and beat position info anywhere else.

##### keys
The keys section is an array that respresents the different player button inputs and the instrument notes that are associated with them. These can be visualized with anything in code, but this is set up to visualize them as dancer poses. The purpose of providing the instrument note is to help with generating custom songs, as well as making sure that your player input sounds actually... y'know... Match the song.

 - ```index```
  
   The index parameter is which note in the instrument file to use. As it relates directly to an array position in code, it actually starts at 0 instead of one. You can put any int value in that is within the instrument file, or even double up. Be advised there's no checking to make sure the value is inside the instrument array bounds. 
 - ```move```
  
   The move parameter determines which player input to assign the note to. In code this is hardcoded still at four inputes, despite being flexible here. This value also refers to an array index, so it goes 0, 1, 2, 3 to represent input buttons 1, 2, 3, and 4.

##### player_beats
This is also an array, that takes a series of beats that it expects to find player input. It takes an instrument note value, which is indexed same as the index above, and a song position value.

 - ```note```
  
   The player instrument note to look for. It's an int value, and SHOULD correspond to the index note value of one of your inputs in the keys section above this. Otherwise the player can never hit that beat!
 - ```position```
  
   The position is a float value of the song position in seconds associated with this beat press. It should be the value AFTER the offset as been removed.

### Instrument JSON Database
The isntrument data is more straightforward by comparison to the song data. The instruments are each a audio file with a set of chords. These are then instruments that the user can play. They should be evenly sized, as the parser does a straight split of the audio clip based on what you tell it is the number of chords in the clip. These instrument files are used for player inputs in the song, so they'll know if they line up or not. The json file has the following file format, and it and the accompanying music files are in ```Rhythm-Dance/Resources/Instruments```
```
{
  "instruments": [
    {
      "name": "Instrument_1",
      "source": "Instrument_1_FileName_No_Extension",
      "notes": "Number_Of_Notes_In_File"
    },
    {
      "name": "Instrument_2",
      "source": "Instrument_2_FileName_No_Extension",
      "notes": "Number_Of_Notes_In_File"
    }
  ]
}
```

## To-Do
 - Comment up that code
 - Make it pretty
 - Ability to save player tunes
 - Actually add a sprite for the dancer
 - Finish dancer animation script
 - Make a base class for animations/rotations
 - Fix up the rotation to have same loop characteristics as the animation capability
 - Make the scoring... Better?

## Music Source
 - [Metronome-80](kookyprod.free.fr/bruitages/metronome80.wav)
 - [A lot of open source music](https://soundcloud.com/user-411047148/sets/ascension)
 - [Fingered Bass Chord](https://www.beepbox.co/#7n31s1k0l00e00t2mm0a7g0fj7i0r1o3210T5v1u51q1d5f7y1z6C1c0h0H-IHyiih9999998T5v1u38q1d5f8y1z8C1c0h2Ht7760Md9xb9pb9T1v1u93q1d4f6y2z1C0c2A8F4B0V1Qd007P5aa3E0019T4v1uf0q1z6666ji8k8k3jSBKSJJAArriiiiii07JCABrzrrrrrrr00YrkqHrsrrrrjr005zrAqzrjzrrqr1jRjrqGGrrzsrsA099ijrABJJJIAzrrtirqrqjqixzsrAjrqjiqaqqysttAJqjikikrizrHtBJJAzArzrIsRCITKSS099ijrAJS____Qg99habbCAYrDzh00b4h4h4h4h4h4h4h4h4h4h4h4h4h4h4h4h4h4h4h4h4h4p1sFE_96ShArV6iAp6iApahAp800000)
 - [Raindrop Chord](https://www.beepbox.co/#7n31s1k0l00e00t2mm0a7g0fj7i0r1o3210T1v1u3aq1d5f8y1z6C1c0A9F4B0V1Q07e0Pc436E006cT5v1u38q1d5f8y1z8C1c0h2Ht7760Md9xb9pb9T1v1u93q1d4f6y2z1C0c2A8F4B0V1Qd007P5aa3E0019T4v1uf0q1z6666ji8k8k3jSBKSJJAArriiiiii07JCABrzrrrrrrr00YrkqHrsrrrrjr005zrAqzrjzrrqr1jRjrqGGrrzsrsA099ijrABJJJIAzrrtirqrqjqixzsrAjrqjiqaqqysttAJqjikikrizrHtBJJAzArzrIsRCITKSS099ijrAJS____Qg99habbCAYrDzh00b4h4h4h4h4h4h4h4h4h4h4h4h4h4h4h4h4h4h4h4h4h4p1sFE_96ShArV6iAp6iApahAp800000)


## Tutorials
 - [Rhythm Tutorial 1](https://www.gamasutra.com/blogs/GrahamTattersall/20190515/342454/Coding_to_the_Beat__Under_the_Hood_of_a_Rhythm_Game_in_Unity.php)
 - [Rhythm Tutorial 2](https://www.gamasutra.com/blogs/YuChao/20170316/293814/Music_Syncing_in_Rhythm_Games.php)