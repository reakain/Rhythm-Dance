using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmDance
{
    public class UITracker : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI playStateLabel;

        private void Awake()
        {
            
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnEnable()
        {
            Conductor.ConductorStateChange += delegate { ChangeStateLabel(); };
        }
        private void OnDisable()
        {
            Conductor.ConductorStateChange -= delegate { ChangeStateLabel(); };
        }


        void ChangeStateLabel()
        {
            playStateLabel.text = Conductor.instance.currentState.ToString();
        }
    }
}