using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharterRules.GameStateModule;

namespace CharterRules.Mission
{


    public class CallEndingAudio : MonoBehaviour
    {
        public GameState gameState;
        public UISequenceManager uiSequenceManager;

        public GameEvent CongratulationsAudioEvent;
        public GameEvent TooBadAudioEvent;

        public void CallAudioEvent()
        {
            if (gameState.allQuestionVotes == null) return;

            if (gameState.CountQuestionsDecided() > 0)
            {
                if (CongratulationsAudioEvent != null)
                    CongratulationsAudioEvent.Raise();
            }
            else
            {
                if (TooBadAudioEvent != null)
                    TooBadAudioEvent.Raise();
            }
        }

        public void AppendAsCallback()
        {
            uiSequenceManager.AppendCallback(CallAudioEvent);
        }
    }

}
