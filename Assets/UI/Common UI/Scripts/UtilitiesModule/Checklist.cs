using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checklist : MonoBehaviour
{
    public MoonshotStation moonshotStation;

    public Toggle didRoverToggle;
    public Toggle didMapToggle;
    public Toggle didArtToggle;
    public Toggle didCharterToggle;
    public Toggle didHuntToggle;

    private List<Toggle> toggles;

    private void Awake()
    {
        toggles = new List<Toggle>()
        {
            didRoverToggle,
            didMapToggle,
            didArtToggle,
            didCharterToggle,
            didHuntToggle
       };

        if (Client.instance != null)
            moonshotStation = Client.instance._moonshotStation;
    }

    private void OnEnable()
    {
        //ReOrganizeToTop();
        UpdateChecklist();
    }

    public void ReOrganizeToTop()
    {
        //First find the first toggle with its activity done
        int firstIndex = -1;
        if (moonshotStation == MoonshotStation.Rover) firstIndex = 0;
        else if (moonshotStation == MoonshotStation.Map) firstIndex = 1;
        else if (moonshotStation == MoonshotStation.Art) firstIndex = 2;
        else if (moonshotStation == MoonshotStation.Question) firstIndex = 3;
        else if (moonshotStation == MoonshotStation.Hunt) firstIndex = 4;

        //Then re-order, assuming the toggles are the only siblings in their parent
        if (firstIndex > 0)
        {
            for (int i = 0; i < firstIndex; i++)
            {
                Toggle toggle = toggles[i];
                toggle.gameObject.transform.SetAsLastSibling();
            }
        }
    }

    public void UpdateChecklist()
    {
        if (Client.instance == null || Client.instance.team == null || Client.instance.team.MoonshotTeamData == null)
        {
            didRoverToggle.isOn = false;
            didMapToggle.isOn = false;
            didArtToggle.isOn = false;
            didCharterToggle.isOn = false;
            didHuntToggle.isOn = false;
            return;
        }

        //assign isOn values
        didRoverToggle.isOn = Client.instance.team.MoonshotTeamData.didRoverActivity;
        didMapToggle.isOn = Client.instance.team.MoonshotTeamData.didMapActivity;
        didArtToggle.isOn = Client.instance.team.MoonshotTeamData.didArtActivity;
        didCharterToggle.isOn = Client.instance.team.MoonshotTeamData.didCharterActivity;
        didHuntToggle.isOn = Client.instance.team.MoonshotTeamData.didHuntActivity;
    }
}
