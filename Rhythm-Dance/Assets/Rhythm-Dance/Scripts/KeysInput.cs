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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmDance
{
    public class KeysInput : MonoBehaviour
    {
        public static KeysInput instance;
        public string pose1Key = "Pose1";
        public string pose2Key = "Pose2";
        public string pose3Key = "Pose3";
        public string pose4Key = "Pose4";
        public string startSongButton = "Submit";
        public string stopSongButton = "Submit";
        public string startRecordButton = "Record";
        public string stopRecordButton = "Submit";
        public bool upButton { get; private set; }
        public bool downButton { get; private set; }

        [Range(0, 1)]
        public float analogueDeadzone = 0.05f;

        public int[] poseKeyNotes { get; private set; }

        private void Update()
        {
            if(Input.GetAxisRaw("Vertical") > analogueDeadzone)
            {
                upButton = true;
                downButton = false;
            }
            else if(Input.GetAxisRaw("Vertical") < -analogueDeadzone)
            {
                upButton = false;
                downButton = true;
            }
            else
            {
                upButton = false;
                downButton = false;
            }
        }

        void Awake()
        {
            instance = this;
            upButton = false;
            downButton = false;
        }

            public void SetKeys(Note[] keys)
        {
            poseKeyNotes = new int[keys.Length];
            foreach(var key in keys)
            {
                poseKeyNotes[key.move] = key.index;
            }
        }
    }
}
