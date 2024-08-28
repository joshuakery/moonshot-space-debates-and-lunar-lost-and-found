using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScavengerHunt.GameStateModule;

namespace ScavengerHunt.WindowsModule
{
    public class NumberButton : MonoBehaviour
    {
        public GameState gameState;

        public Button button;

        public void AddToCode(string value)
        {
            if (gameState.code.Length < 3)
            {
                gameState.code += value;
            }
        }

        public void SetState()
        {
            if (gameState.code.Length < 3)
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

