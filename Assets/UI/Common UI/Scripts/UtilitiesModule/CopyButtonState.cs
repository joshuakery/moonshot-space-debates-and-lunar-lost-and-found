using UnityEngine;
using UnityEngine.UI;

public class CopyButtonState : MonoBehaviour
{
    public Button self;

    public Button target;

    private void Update()
    {
       if (target.interactable != self.interactable)
        {
            self.interactable = target.interactable;
        }
    }
}
