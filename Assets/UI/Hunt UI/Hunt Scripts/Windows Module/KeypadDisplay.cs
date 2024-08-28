using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ScavengerHunt.GameStateModule;

namespace ScavengerHunt.WindowsModule
{
    public class KeypadDisplay : MonoBehaviour
    {
        public GameState gameState;
        public TMP_Text display;

        private void Start()
        {
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            display.text = gameState.code;
        }
    }
}


