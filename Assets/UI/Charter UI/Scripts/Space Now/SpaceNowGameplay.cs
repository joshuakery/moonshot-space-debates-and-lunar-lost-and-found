using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CharterRules.SpaceNow.GameStateModule;
using CharterRules.QuestionsLoaderModule;

namespace CharterRules.SpaceNow.Windows
{
    public class SpaceNowGameplay: MonoBehaviour
    {
        public SpaceNowGameState gameState;

        public GameEvent SetupNextQuestion;

        public TMP_Text title;
        public TMP_Text situation;

        public RawImage optionAArguer;
        public TMP_Text optionA;

        public RawImage optionBArguer;
        public TMP_Text optionB;

        private void OnEnable()
        {
            SetTexts();
        }

        public void SetTexts()
        {
            if (gameState.currentQuestion != null)
            {
                if (title != null)
                    title.text = gameState.currentQuestion.title;

                if (situation != null)
                    situation.text = gameState.currentQuestion.situation;

                if (optionAArguer != null)
                    optionAArguer.texture = gameState.currentQuestion.options.optionA.arguerImageTex;

                if (optionA != null)
                    optionA.text = gameState.currentQuestion.options.optionA.argument;

                if (optionBArguer != null)
                    optionBArguer.texture = gameState.currentQuestion.options.optionB.arguerImageTex;

                if (optionB != null)
                    optionB.text = gameState.currentQuestion.options.optionB.argument;
            }
        }

    }
}

