using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CharterRules.BallotModule
{
    public class StaggeredBallotCount : MonoBehaviour
    {
        public string trigger;

        public List<Ballot> ballots;

        public float delayMultiplier;
        public float delayBase;

        public UnityEvent callbackEvent;

        private IEnumerator DelayedTrigger(float delay, Ballot ballot)
        {
            yield return new WaitForSeconds(delay);
            ballot.gameObject.GetComponent<Animator>().SetTrigger(trigger);
        }

        private IEnumerator DelayedTrigger(float delay, UnityEvent delayedEvent)
        {
            yield return new WaitForSeconds(delay);
            delayedEvent.Invoke();
        }

        public void TriggerStaggeredAnimations()
        {
            List<int> chosen = new List<int>();
            foreach (Ballot ballot in ballots)
            {
                int choice = ballot.gameState.GetVote(ballot.ballotID);
                if (choice >= 0)
                {
                    if (!chosen.Contains(choice)) chosen.Add(choice);
                    int index = chosen.IndexOf(choice);

                    float delay = (float)index * delayMultiplier + delayBase;
                    StartCoroutine(DelayedTrigger(delay, ballot));
                }

            }

            float callbackDelay = (float)chosen.Count * delayMultiplier + delayBase;
            StartCoroutine(DelayedTrigger(callbackDelay, callbackEvent));
        }

        public void TriggerStaggeredAnimations2()
        {
            int i = 0;
            foreach (Ballot ballot in ballots)
            {
                if (ballot.gameState.DidVote(ballot.ballotID))
                {
                    float delay = (float)i * delayMultiplier + delayBase;
                    StartCoroutine(DelayedTrigger(delay, ballot));

                    i++;
                }

            }

            float callbackDelay = (float)i * delayMultiplier + delayBase;
            StartCoroutine(DelayedTrigger(callbackDelay, callbackEvent));
        }
    }
}


