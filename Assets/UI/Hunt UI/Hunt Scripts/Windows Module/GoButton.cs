using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScavengerHunt.GameStateModule;

namespace ScavengerHunt.WindowsModule
{
    public class GoButton : MonoBehaviour
    {
        public GameState gameState;
        public Button button;
        public void SetState()
        {
            if (gameState.code.Length == 3)
            {

                button.interactable = true;
            }
            else
            {
                button.interactable = false;
            }
        }
    }
}


