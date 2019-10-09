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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmDance
{
    [Serializable]
    public static class InstrumentLibrary
    {
        public static InstrumentImporter instrumentData = InstrumentImporter.CreateFromJSON(Resources.Load<TextAsset>("Instruments/InstrumentDatabase").text);

        public static Instrument GetInstrumentFromName(string instrumentName)
        {
            var instData = Array.Find(instrumentData.instruments, instrument => instrument.name == instrumentName);

            Instrument newInst = new Instrument(Resources.Load<AudioClip>("Instruments/" + instData.source), instData.notes, instData.name);
            return newInst;
        }

        public static string[] GetInstrumentList()
        {
            string[] list = new string[instrumentData.instruments.Length];
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = instrumentData.instruments[i].name;
            }
            return list;
        }
    }

    [Serializable]
    public class InstrumentImporter
    {
        public InstrumentData[] instruments;

        public static InstrumentImporter CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<InstrumentImporter>(jsonString);
        }
    }

    [Serializable]
    public class InstrumentData
    {
        public string name = "";
        public string source = "";
        public int notes = 16;
    }

    [Serializable]
    public class Instrument
    {
        [SerializeField]
        private AudioClip m_Source;
        [SerializeField]
        public AudioClip Source
        {
            get { return m_Source; }
            set
            {
                m_Source = value;
            }
        }
        [SerializeField]
        private int m_NumberNotes;
        [SerializeField]
        public int NumberNotes
        {
            get { return m_NumberNotes; }
            set
            {
                m_NumberNotes = value;
            }
        }
        public string Name;
        [SerializeField]
        public AudioClip[] Notes { get; private set; }

        public Instrument(AudioClip source, int numNotes, string name)
        {
            Name = name;
            NumberNotes = numNotes;
            Source = source;

            Notes = AudioSplitter.BeatSplit(Source, NumberNotes);
        }

        public void SetNotes()
        {
            Notes = AudioSplitter.BeatSplit(Source, NumberNotes);
        }
    }
}

