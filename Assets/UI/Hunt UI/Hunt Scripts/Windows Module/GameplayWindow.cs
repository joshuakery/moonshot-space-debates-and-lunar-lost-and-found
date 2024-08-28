using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScavengerHunt.GameStateModule;
using ScavengerHunt.ItemsLoaderModule;

namespace ScavengerHunt.WindowsModule
{
    public class GameplayWindow : MonoBehaviour
    {
        public GameState gameState;

        public ItemsLoader itemsLoader;

        public GameEvent Correct;

        public GameEvent Incorrect;

        public GameEvent AlreadyFound;

        public GameEvent AllFound;

        public GameEvent CloseAllSecondaryWindowsEvent;

        public Button finish;

        private void OnEnable()
        {
            UpdateFinishButton();
        }

        public void CheckCode()
        {
            if (itemsLoader.itemsData != null)
            {
                if (itemsLoader.itemsData.ContainsKey(gameState.code))
                {
                    gameState.CurrentItem = itemsLoader.itemsData[gameState.code];
                    if (!gameState.found.Contains(gameState.code))
                    {
                        gameState.found.Add(gameState.code);
                        gameState.recentFind = true;

                        Correct.Raise();

                        gameState.SaveFoundToFile();
                        gameState.SaveFoundToServer();

                        //CloseAllSecondaryWindowsEvent.Raise();


                        //if (gameState.found.Count >= gameState.target)
                        //{
                        //    AllFound.Raise();
                        //}
                    }
                    else
                    {
                        gameState.recentFind = false;
                        AlreadyFound.Raise();
                    }
                    gameState.code = "";

                }
                else
                {
                    gameState.code = "";
                    gameState.recentFind = false;
                    Incorrect.Raise();
                }
            }

        }

        public void UpdateFinishButton()
        {
            if (gameState.found.Count >= gameState.target)
            {
                finish.interactable = true;
            }
            else
            {
                finish.interactable = false;
            }
        }

        public void SetOffRecentFind()
        {
            gameState.recentFind = false;
        }

        private void Start()
        {
            if (itemsLoader == null)
                itemsLoader = (ItemsLoader)Object.FindObjectOfType(typeof(ItemsLoader));
        }
    }
}


