using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace CharterRules.BallotModule
{
    public class StaggeredBallotCountTween : MonoBehaviour
    {
        public List<Ballot> ballots;

        public float delayMultiplier = 0.5f;
        public float delayBase = 0.5f;

        public UISequenceManager sequenceManager;

        public GameEvent aAudioEvent;
        public GameEvent bAudioEvent;

        public void TriggerStaggeredAnimations()
        {
            Debug.Log("Triggering stagged animations");

            Sequence sequence = null;

            List<int> chosen = new List<int>();
            foreach (Ballot ballot in ballots)
            {
                int choice = ballot.gameState.GetVote(ballot.ballotID);
                if (choice >= 0)
                {
                    bool doAddAudio = false;

                    if (!chosen.Contains(choice))
                    {
                        chosen.Add(choice);
                        doAddAudio = true;
                    }

                    int index = chosen.IndexOf(choice);

                    if (ballot.bobAnimation != null)
                    {
                        if (sequence == null)
                            sequence = DOTween.Sequence();

                        Sequence pulse = ballot.bobAnimation.GetPulse();

                        float delay = (float)index * delayMultiplier + delayBase;
                        sequence.Insert(delay, pulse);

                        //add audio once for each option
                        if (doAddAudio)
                            AddAudio(sequence, choice, delay);
                    }
                }
            }

            if (sequence != null)
                sequenceManager.currentSequence.Append(sequence);
        }

        private void AddAudio(Sequence sequence, int choice, float delay)
        {
            Debug.Log(System.String.Format("Adding audio for choice {0} at {1}s.",choice,delay));
            if (choice == 0 && aAudioEvent != null)
            {
                sequence.InsertCallback(delay, () => { aAudioEvent.Raise(); });
            }
            else if (choice == 1 && bAudioEvent != null)
            {
                sequence.InsertCallback(delay, () => { bAudioEvent.Raise(); });
            }
        }
    }
}


