using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CharterRules.SpaceNow.GameStateModule;
using CharterRules.SpaceNow.QuestionsLoaderModule;

namespace CharterRules.SpaceNow.Windows
{
    public class HomeButtonComponent : MonoBehaviour
    {
        public SpaceNowGameState gameState;
        public GameEvent closeAllWindowsEvent;
        public GameEvent gameplayEvent;

        public RawImage icon;
        public TMP_Text title;

        public Button button;

        public void SetContent(QuestionSet questionSet)
        {
            if (icon != null)
                icon.texture = questionSet.icon;

            if (title != null)
                title.text = questionSet.title;
        }

        public void SetButton(string key)
        {
            if (button != null)
            {
                button.onClick.AddListener(() =>
                {
                    gameState.currentCategory = key;
                    closeAllWindowsEvent.Raise();
                    gameplayEvent.Raise();
                });
            }
        }
    }
}


