using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CharterRules.GameStateModule;
using DG.Tweening;

namespace CharterRules.Mission.WindowsModule
{
    public class Meters : MonoBehaviour
    {
        public GameState gameState;

        public Slider meter1;
        public Slider meter2;
        public Slider meter3;

        public Ease ease = Ease.OutQuad;

        public float duration = 0.4f;

        public UISequenceManager sequenceManager;

        public void UpdateMeters()
        {
            Sequence sequence = GetUpdateSequence();
            sequenceManager.currentSequence.Join(sequence);
        }

        public void UpdateMeters(bool doAppend)
        {
            Sequence sequence = GetUpdateSequence();

            if (doAppend)
                sequenceManager.currentSequence.Append(sequence);
            else
                sequenceManager.currentSequence.Join(sequence);
        }

        public void UpdateMetersAsync()
        {
            Sequence sequence = GetUpdateSequence();
            sequence.Complete();
        }

        private Sequence GetUpdateSequence()
        {
            gameState.RecalcScales();

            Tween meter1Tween = UpdateMeter(meter1, gameState.meter1);
            Tween meter2Tween = UpdateMeter(meter2, gameState.meter2);
            Tween meter3Tween = UpdateMeter(meter3, gameState.meter3);

            Sequence sequence = DOTween.Sequence();
            sequence.Join(meter1Tween);
            sequence.Join(meter2Tween);
            sequence.Join(meter3Tween);

            return sequence;
        }

        private Tween UpdateMeter(Slider meter, float newValue)
        {
            Tween _tween = null;

            _tween = meter.DOValue(newValue, duration);
            _tween.SetEase(ease);

            return _tween;
        }

        public void ResetMeters()
        {
            UpdateMeter(meter1, Average(meter1.maxValue, meter1.minValue));
            UpdateMeter(meter2, Average(meter2.maxValue, meter2.minValue));
            UpdateMeter(meter3, Average(meter3.maxValue, meter3.minValue));
        }

        private float Average(float a, float b)
        {
            return (float)(a + b) / 2;
        }


    }
}


