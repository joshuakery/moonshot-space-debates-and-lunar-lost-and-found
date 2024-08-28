using UnityEngine;
using UnityEngine.UI;
using ScavengerHunt.GameStateModule;

namespace ScavengerHunt.WindowsModule
{
    public class BackspaceButton : MonoBehaviour
    {
        public GameState gameState;

        public Button button;
        public void Backspace()
        {
            if (gameState.code.Length > 0)
            {
                gameState.code = gameState.code.Remove(gameState.code.Length - 1, 1);
            }
        }

        public void SetState()
        {
            if (gameState.code.Length <= 0)
            {
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
            }
        }
    }
}

