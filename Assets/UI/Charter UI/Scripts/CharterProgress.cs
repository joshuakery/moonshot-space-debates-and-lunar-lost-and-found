using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CharterRules.GameStateModule;
using DG.Tweening;

namespace CharterRules.Mission.WindowsModule
{
    public class CharterProgress : MonoBehaviour
    {
        public GameState gameState;

        public Transform ruleList;
        public GameObject ruleItemPrefab;

        public Sprite aDot;
        public Sprite bDot;
        public Sprite emptyDot;

        public int maxDisplay = 5;

        public UISequenceManager sequenceManager;

        public void ClearProgress()
        {
            for (int i = 0; i < ruleList.childCount; i++)
            {
                Transform ruleItem = ruleList.GetChild(i);
                GameObject.Destroy(ruleItem.gameObject);
            }
        }

        public void UpdateProgress(float atPosition)
        {
            gameState.RecalcProgress();
            List<string> progress = gameState.progress;

            for (int i = ruleList.childCount; i < progress.Count; i++)
            {
                string number = (i + 1).ToString();
                string resolution = progress[i];
                int aCount = gameState.CountVotesForOptionForCurrentQuestion(0);
                int bCount = gameState.CountVotesForOptionForCurrentQuestion(1);
                CreateNewRuleItem(number, resolution, aCount, bCount);
            }


            if (progress.Count > maxDisplay)
            {
                for (int i = 0; i < ruleList.childCount; i++)
                {
                    if (i < progress.Count - maxDisplay)
                    {
                        Transform child = ruleList.GetChild(i);
                        if (child.gameObject.activeInHierarchy)
                        {
                            GenericWindow1 genericWindow = child.GetComponent<GenericWindow1>();
                            if (genericWindow != null) { genericWindow.Close(true); }
                            //Transform content = child.FindDeepChild("Content");
                            //GenericWindow1 contentWindow = content.gameObject.GetComponent<GenericWindow1>();
                            //if (contentWindow != null) { contentWindow.Close(true); }
                            //sequenceManager.AppendInterval(0.2f);
                        }

                    }
                }
            }


        }

        private void CreateNewRuleItem(string number, string resolution, int aCount, int bCount)
        {
            Transform ruleItem = Instantiate(ruleItemPrefab, ruleList).transform;

            TMP_Text counter = ruleItem.FindDeepChild("Number")
            .GetComponent<TMP_Text>();
            counter.text = number;

            TMP_Text body = ruleItem.FindDeepChild("Body")
            .GetComponent<TMP_Text>();
            body.text = resolution;



            Transform ACount = ruleItem.FindDeepChild("A Count");
            if (ACount != null)
            {
                TMP_Text ACountText = ACount.GetComponent<TMP_Text>();
                if (ACountText != null)
                {
                    ACountText.text = aCount.ToString();
                }
            }
            Transform BCount = ruleItem.FindDeepChild("B Count");
            if (BCount != null)
            {
                TMP_Text BCountText = BCount.GetComponent<TMP_Text>();
                if (BCountText != null)
                {
                    BCountText.text = bCount.ToString();
                }
            }
            Transform WinCount = ruleItem.FindDeepChild("Win Count");
            if (WinCount != null)
            {
                TMP_Text WinCountText = WinCount.GetComponent<TMP_Text>();
                if (WinCountText != null)
                {
                    int winScore = Mathf.Max(aCount, bCount);
                    WinCountText.text = System.String.Format("{0}/{1}", winScore, aCount + bCount);
                }
            }
            Transform WinDots = ruleItem.FindDeepChild("Win Dots");
            if (WinDots != null)
            {
                for (int i = 0; i < WinDots.childCount; i++)
                {
                    Transform child = WinDots.GetChild(i);
                    Image dot = child.gameObject.GetComponent<Image>();

                    if (i < aCount)
                        dot.sprite = aDot;
                    else if (i < aCount + bCount)
                        dot.sprite = bDot;
                    else
                        dot.sprite = emptyDot;
                }
            }

            //Transform content = ruleItem.FindDeepChild("Content");
            //GenericWindow1 contentWindow = content.gameObject.GetComponent<GenericWindow1>();
            //if (contentWindow != null) { contentWindow.Open(true); }
            GenericWindow1 genericWindow = ruleItem.GetComponent<GenericWindow1>();
            if (genericWindow != null) { genericWindow.Open(true); }

        }
    }
}


