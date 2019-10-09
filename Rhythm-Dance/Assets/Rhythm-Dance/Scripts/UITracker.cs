using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmDance
{
    public class UITracker : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI playStateLabel;

        public TMPro.TextMeshProUGUI scoreLabel;

        private void Awake()
        {
            
        }
        // Start is called before the first frame update
        void Start()
        {
            ChangeScoreLabel();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnEnable()
        {
            Conductor.ConductorStateChange += delegate { ChangeStateLabel(); };
            Conductor.ScoreChange += delegate { ChangeScoreLabel(); };
        }
        private void OnDisable()
        {
            Conductor.ConductorStateChange -= delegate { ChangeStateLabel(); };
            Conductor.ScoreChange -= delegate { ChangeScoreLabel(); };
        }


        void ChangeStateLabel()
        {
            playStateLabel.text = Conductor.instance.currentState.ToString();
        }

        void ChangeScoreLabel()
        {
            scoreLabel.text = Conductor.instance.playScore.ToString("N1");
        }
    }
}