using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ScavengerHunt.GameStateModule;

namespace ScavengerHunt.WindowsModule
{
    public class Ending : MonoBehaviour
    {
        public GameState gameState;

        public TMP_Text heading;
        public TMP_Text body;

        string[] headings = new string[3] {
        "CONGRATULATIONS",
        "PARTIAL SUCCESS",
        "OUT OF TIME"
    };

        string[] bodies = new string[3] {
        "Great work! You found all of our crew members’ lost possessions. Having their treasured things will keep their spirits high as they perform their important jobs for <nobr>the Settlement.</nobr>",
        "Great work! You found some of our crew members’ lost possessions, and I feel confident that we will find the rest soon. Having their treasured things will keep their spirits high as they perform their important jobs for <nobr>the Settlement.</nobr>",
        "Too bad! You ran out of time before finding any of the lost objects. We’ll keep looking, and in the meantime we’ll find other ways to keep up the spirits of our crew as they perform their important jobs for <nobr>the Settlement.</nobr>"
    };

        public void SetTexts()
        {
            if (gameState.found.Count == gameState.target)
            {
                heading.text = headings[0];
                body.text = bodies[0];
            }
            else if (gameState.found.Count > 0)
            {
                heading.text = headings[1];
                body.text = bodies[1];
            }
            else
            {
                heading.text = headings[2];
                body.text = bodies[2];
            }
        }
    }
}

