using UnityEngine;
using ScavengerHunt.GameStateModule;

namespace ScavengerHunt.AudioHelpersModule
{
    public class CallEndingAudio : MonoBehaviour
    {
        public GameState gameState;
        public UISequenceManager uiSequenceManager;

        public GameEvent CongratulationsAudioEvent;
        public GameEvent PartialSuccessAudioEvent;
        public GameEvent TooBadAudioEvent;

        public void CallAudioEvent()
        {
            if (gameState.found != null)
            {
                if (gameState.found.Count == 0)
                    TooBadAudioEvent.Raise();
                else if (gameState.found.Count > 0 && gameState.found.Count < gameState.target)
                    PartialSuccessAudioEvent.Raise();
                else if (gameState.found.Count >= gameState.target)
                    CongratulationsAudioEvent.Raise();
            }
        }

        public void AppendAudioAsCallback()
        {
            uiSequenceManager.AppendCallback(CallAudioEvent);
        }
    }

}
