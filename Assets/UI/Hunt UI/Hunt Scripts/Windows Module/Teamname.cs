using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ScavengerHunt.GameStateModule;

namespace ScavengerHunt.WindowsModule
{
    public class Teamname : MonoBehaviour
    {
        public TMP_Text tmp_text;
        public GameState gameState;

        private void OnEnable()
        {
            SetTeamname();
        }

        public void SetTeamname()
        {
            tmp_text.text = BoldTags(gameState.currentTeam.teamName);
        }
        private string BoldTags(string input)
        {
            return "<b>" + input + "</b>";
        }
    }
}


