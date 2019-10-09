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
