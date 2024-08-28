using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CharterRules.SpaceNow.GameStateModule;
using CharterRules.SpaceNow.QuestionsLoaderModule;
using CharterRules.SpaceNow.Windows;

namespace CharterRules.SpaceNow.Windows
{
    public class HomeButtonsManager : MonoBehaviour
    {
        public SpaceNowGameState gameState;

        public GameObject homeButtonPrefab;
        public Transform homeButtonsContainer;

        private void OnEnable()
        {
            Reset();
        }

        public void Reset()
        {
            ClearHomeButtons();
            InstantiateHomeButtons(gameState.allQuestionSets);
        }

        private void ClearHomeButtons()
        {
            foreach (Transform child in homeButtonsContainer)
            {
                GameObject.Destroy(child.gameObject);
            }
            homeButtonsContainer.DetachChildren();
        }

        private void InstantiateHomeButtons(Dictionary<string, QuestionSet> allQuestionSets)
        {
            foreach (KeyValuePair<string, QuestionSet> kvp in allQuestionSets)
            {
                GameObject homeButtonGameObject = Instantiate(homeButtonPrefab, homeButtonsContainer);

                HomeButtonComponent homeButtonComponent = homeButtonGameObject.GetComponent<HomeButtonComponent>();

                if (homeButtonComponent != null)
                {
                    QuestionSet questionSet = kvp.Value;
                    homeButtonComponent.SetContent(questionSet);

                    string category = kvp.Key;
                    homeButtonComponent.SetButton(category);
                }
            }
        }
    }
}


