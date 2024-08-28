using UnityEngine;
using TMPro;
using CharterRules.GameStateModule;

namespace CharterRules.Mission.WindowsModule
{
    public class TeamName : MonoBehaviour
    {
        public TMP_Text tmp_text;
        public GameState gameState;

        private void OnEnable()
        {
            SetTeamname();
        }

        public void SetTeamname()
        {
            if (System.String.IsNullOrEmpty(gameState.currentTeam.teamName))
                tmp_text.text = "Team Teamname";
            else
                tmp_text.text = BoldTags(gameState.currentTeam.teamName);
        }
        private string BoldTags(string input)
        {
            return "<b>" + input + "</b>";
        }
    }
}



