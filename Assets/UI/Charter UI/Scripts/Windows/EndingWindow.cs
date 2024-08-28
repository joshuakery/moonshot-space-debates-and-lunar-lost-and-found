using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using CharterRules.Mission.WindowsModule;

namespace CharterRules.Mission.WindowsModule
{
    public class EndingWindow : GenericWindow
    {
        public TMP_Text heading;
        public TMP_Text body;

        public static string[] headings = new string[2] {
        "OUT OF TIME",
        "CONGRATULATIONS"
    };

        public static string[] bodies = new string[2] {
        "Too bad! You ran out of time before making any new rules. The International Space Agency will develop your settlement rules for you.",
        "Good work! Your rule choices will serve as the foundation for a settlement charter that will help us all live together on the Moon. It also tells us what kind of society we're going to live in. Well done!"
    };

        public void SetTexts()
        {
            if (gameState.allQuestionVotes == null) return;

            // if (gameState.allQuestionVotes.Where(
            //     kvp => (
            //         kvp.Value.votes != null &&
            //         kvp.Value.votes.Count > 0
            //     )
            // ).ToArray().Length > 0
            // ) //we have at least 1 vote
            if (gameState.CountQuestionsDecided() > 0)
            {
                if (heading != null)
                    heading.text = headings[1];

                if (body != null)
                    body.text = bodies[1];
            }
            else
            {
                if (heading != null)
                    heading.text = headings[0];

                if (body != null)
                    body.text = bodies[0];
            }

        }
    }
}


