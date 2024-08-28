using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace CharterRules.BallotModule
{
    public class BallotOptionsToggle : MonoBehaviour
    {
        public Sprite aUnselected;
        public Sprite aSelected;
        public Sprite bUnselected;
        public Sprite bSelected;

        public Image aCircle;
        public Image bCircle;

        public GenericWindow1 aWindow;
        public GenericWindow1 bWindow;

        public void OnSelect(int selectedOption)
        {
            switch (selectedOption)
            {
                case 0:
                    if (aCircle.sprite != aSelected)
                    {
                        aWindow.Pulse(0f);
                        aWindow.uiTweener.sequenceManager.InsertCallback(0.15f, () =>
                        {
                            aCircle.sprite = aSelected;
                        });
                    }
                    if (bCircle.sprite == bSelected)
                    {
                        bWindow.Pulse(0f);
                        bWindow.uiTweener.sequenceManager.InsertCallback(0.15f, () =>
                        {
                            bCircle.sprite = bUnselected;
                        });
                    }
                    break;
                case 1:
                    if (aCircle.sprite == aSelected)
                    {
                        aWindow.Pulse(0f);
                        aWindow.uiTweener.sequenceManager.InsertCallback(0.15f, () =>
                        {
                            aCircle.sprite = aUnselected;
                        });
                    }
                    if (bCircle.sprite != bSelected)
                    {
                        bWindow.Pulse(0f);
                        bWindow.uiTweener.sequenceManager.InsertCallback(0.15f, () =>
                        {
                            bCircle.sprite = bSelected;
                        });
                    }
                    break;
            }
        }

        public void ToggleA(bool doToggleOn)
        {
            if (doToggleOn)
                aCircle.sprite = aSelected;
            else
                aCircle.sprite = aUnselected;
        }

        public void ToggleB(bool doToggleOn)
        {
            if (doToggleOn)
                bCircle.sprite = bSelected;
            else
                bCircle.sprite = bUnselected;
        }

        public void DeselectAll()
        {
            aCircle.sprite = aUnselected;
            bCircle.sprite = bUnselected;
        }
    }
}


