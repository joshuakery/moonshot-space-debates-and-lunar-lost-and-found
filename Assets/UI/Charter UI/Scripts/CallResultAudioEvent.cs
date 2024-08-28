using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharterRules.GameStateModule;

public class CallResultAudioEvent : MonoBehaviour
{
    public GameState gameState;
    public UISequenceManager uiSequenceManager;

    public GameEvent aOrBEvent;
    public GameEvent tieEvent;
    public GameEvent nobodyVotedEvent;

    public void CallAudioEvent()
    {
        if (gameState.allQuestionData == null ||
            gameState.allQuestionData.Count == 0) return;

        string currentQuestionID = gameState.allQuestionDataOrder[gameState.currentQuestionIndex];

        int[] winners = gameState.GetWinners(currentQuestionID);

        if (winners.Length == 0)
        {
            if (nobodyVotedEvent != null)
                nobodyVotedEvent.Raise();
        }
        else if (winners.Length > 1)
        {
            if (tieEvent != null)
                tieEvent.Raise();
        }
        else
        {
            if (aOrBEvent != null)
                aOrBEvent.Raise();
        }
    }

    public void AppendAsCallback()
    {
        uiSequenceManager.AppendCallback(CallAudioEvent);
    }
}
