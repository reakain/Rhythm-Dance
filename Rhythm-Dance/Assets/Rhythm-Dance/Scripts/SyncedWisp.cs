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
    public class SyncedWisp : MonoBehaviour
    {
        //The animator controller attached to this GameObject
        public Animator animator;

        //Records the animation state or animation that the Animator is currently in
        public AnimatorStateInfo animatorStateInfo;

        //Used to address the current state within the Animator using the Play() function
        public int currentState;

        public int beatsPerLoop = 2;

        //the total number of loops completed since the looping clip first started
        public int completedLoops = 0;

        //The current position of the song within the loop in beats.
        public float loopPositionInBeats;

        //The current relative position of the song within the loop measured between 0 and 1.
        public float loopPositionInAnalog;

        // Start is called before the first frame update
        void Start()
        {
            //Load the animator attached to this object
            animator = GetComponent<Animator>();

            //Get the info about the current animator state
            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

            //Convert the current state name to an integer hash for identification
            currentState = animatorStateInfo.fullPathHash;
        }

        private void OnEnable()
        {
            Conductor.ConductorStateChange += delegate { ChangeState(); };
        }

        private void OnDisable()
        {
            Conductor.ConductorStateChange -= delegate { ChangeState(); };
        }

        void ChangeState()
        {
            if (Conductor.instance.currentState == Conductor.ConductorState.Stop)
            {
                animator.SetBool("PlayBeat", false);
                //animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

                //Convert the current state name to an integer hash for identification
                //currentState = animatorStateInfo.fullPathHash;
            }
            else
            {
                animator.SetBool("PlayBeat", true);
                //animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

                //Convert the current state name to an integer hash for identification
                //currentState = animatorStateInfo.fullPathHash;
            }
        }

        // Update is called once per frame
        void Update()
        {
            

            if (animator.GetBool("PlayBeat"))
            {
                if (Conductor.instance.songPositionInBeats >= (completedLoops + 1) * beatsPerLoop)
                    completedLoops++;
                loopPositionInBeats = Conductor.instance.songPositionInBeats - completedLoops * beatsPerLoop;

                loopPositionInAnalog = loopPositionInBeats / beatsPerLoop;

                //Start playing the current animation from wherever the current conductor loop is
                animator.Play("WispHop", -1, (loopPositionInAnalog));
                //Set the speed to 0 so it will only change frames when you next update it
                animator.speed = 0;
            }
            else
            {
                completedLoops = 0;
                animator.speed = 1;
            }
        }
    }
}