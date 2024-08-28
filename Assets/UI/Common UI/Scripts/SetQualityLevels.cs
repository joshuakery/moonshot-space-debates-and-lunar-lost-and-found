using UnityEngine;
using System.Collections;
using rlmg.logging;
/*
* Created by Carl.
* Unity Forums - TaleOf4Gamers.
* Made in Unity 5.4 Beta.
*/
public class SetQualityLevels : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown("1")) //Fastest Quality
            {
                QualitySettings.SetQualityLevel(0, true);
                RLMGLogger.Instance.Log("Quality settings set to 'Fastest'", MESSAGETYPE.INFO);

            }

            if (Input.GetKeyDown("2")) //Fast Quality
            {
                QualitySettings.SetQualityLevel(1, true);
                RLMGLogger.Instance.Log("Quality settings set to 'Fast'", MESSAGETYPE.INFO);

            }

            if (Input.GetKeyDown("3")) //Simple Graphics
            {
                QualitySettings.SetQualityLevel(2, true);
                RLMGLogger.Instance.Log("Quality settings set to 'Simple'", MESSAGETYPE.INFO);

            }

            if (Input.GetKeyDown("4")) //Good Graphics
            {
                QualitySettings.SetQualityLevel(3, true);
                RLMGLogger.Instance.Log("Quality settings set to 'Good'", MESSAGETYPE.INFO);

            }

            if (Input.GetKeyDown("5")) //Beautiful Graphics
            {
                QualitySettings.SetQualityLevel(4, true);
                RLMGLogger.Instance.Log("Quality settings set to 'Beautiful'", MESSAGETYPE.INFO);

            }

            if (Input.GetKeyDown("6")) //Fantastic Graphics
            {
                QualitySettings.SetQualityLevel(5, true);
                RLMGLogger.Instance.Log("Quality settings set to 'Fantastic'", MESSAGETYPE.INFO);

            }
        }

    }
}
