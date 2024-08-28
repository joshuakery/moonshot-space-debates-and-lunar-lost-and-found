using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using CharterRules.GameStateModule;

namespace CharterRules.BallotModule
{
    public class BallotSequenceManager : MonoBehaviour
    {
        public UISequenceManager sequenceManager;

        public Ballot ballot;
        public GameState gameState;

        public List<GenericWindow1> myWindows;

        //options in custom sequencing
        public GenericWindow1 situation;
        public GenericWindow1 argument1;
        public GenericWindow1 argument2;
        public GenericWindow1 discussMessage;

        //options back in - check if ballot is already open
        public GenericWindow1 ballotContainer;

        //choice selection container back in


        //enable disable options
        public Image optionABorder;
        public Image optionACircle;
        public Image optionBBorder;
        public Image optionBCircle;
        public Sprite optionABorderSprite;
        public Sprite optionACircleSprite;
        public Sprite optionBBorderSprite;
        public Sprite optionBCircleSprite;
        public Sprite optionXBorderSprite;
        public Sprite optionXCircleSprite;

        //close all with start exception
        public GenericWindowManager contentManager;
        public GenericWindow1 startContainer;

        //start interactable callback
        public Button startButton;

        public bool firstHasComeIn = false;
        public bool didVote = false;

        private void Awake()
        {
            sequenceManager = ScriptableObject.CreateInstance<UISequenceManager>();
        }

        private void Start()
        {
            foreach (GenericWindow1 genericWindow in myWindows)
            {
                genericWindow.uiTweener.sequenceManager = sequenceManager;
            }

            firstHasComeIn = false;
        }

        public void ResetFirstHasComeInBool()
        {
            firstHasComeIn = false;
        }

        public void OptionsIn()
        {
            if (sequenceManager.currentSequence == null)
            {
                sequenceManager.currentSequence = DOTween.Sequence();
                sequenceManager.currentSequence.OnKill(() => {
                    sequenceManager.currentSequence = null;
                });
            }

            if (!firstHasComeIn && !gameState.IsFirstBallot(ballot.ballotID))
                firstHasComeIn = true;

            if (!firstHasComeIn)
            {
                situation.Open(true);
                argument1.Open(argument1.uiTweener.sequenceManager.currentSequence.Duration() + 0.34f);
                argument2.Open(argument2.uiTweener.sequenceManager.currentSequence.Duration() + 0.33f);
                discussMessage.Open(argument2.uiTweener.sequenceManager.currentSequence.Duration() + 0.33f);

                firstHasComeIn = true;
            }
            else
            {
                situation.Open(true);
                argument1.Open(argument1.uiTweener.sequenceManager.currentSequence.Duration());
                argument2.Open(argument2.uiTweener.sequenceManager.currentSequence.Duration());
                discussMessage.Open(argument2.uiTweener.sequenceManager.currentSequence.Duration());
            }
        }

        public void OptionsBackIn()
        {
            if (ballotContainer.isOpen)
            {
                situation.Open(true);
                argument1.Open(false);
                argument2.Open(false);
                discussMessage.Open(false);
            }
        }

        public void DisableOptionA()
        {
            optionABorder.sprite = optionXBorderSprite;
            optionACircle.sprite = optionXCircleSprite;
        }

        public void DisableOptionB()
        {
            optionBBorder.sprite = optionXBorderSprite;
            optionBCircle.sprite = optionXCircleSprite;
        }

        public void DisableOptions()
        {
            DisableOptionA();
            DisableOptionB();
        }

        public void EnableOptionA()
        {
            optionABorder.sprite = optionABorderSprite;
            optionACircle.sprite = optionACircleSprite;
        }

        public void EnableOptionB()
        {
            optionBBorder.sprite = optionBBorderSprite;
            optionBCircle.sprite = optionBCircleSprite;
        }

        public void EnableOptions()
        {
            EnableOptionA();
            EnableOptionB();
        }

        public void CloseAllWithStartException()
        {
            foreach (GenericWindow1 genericWindow in contentManager.genericWindows)
            {
                if (genericWindow != startContainer)
                    genericWindow.Close();
            }
        }

        public void CloseAllAndCompleteAsync()
        {
            foreach (GenericWindow1 genericWindow in contentManager.genericWindows)
            {
                genericWindow.CloseAndCompleteAsync();
            }
        }

        public void AppendStartInteractableCallback(UISequenceManager seqManager)
        {
           seqManager.AppendCallback(() =>
           {
               startButton.interactable = true;
           });
        }

        public void CompleteSequence()
        {
            if (sequenceManager.currentSequence != null)
                sequenceManager.currentSequence.Complete();

            sequenceManager.currentSequence = null;
        }

        public void CreateNewSequenceAfterCurrent()
        {
            sequenceManager.CreateNewSequenceAfterCurrent();
        }



        // Update is called once per frame
        void Update()
        {

        }
    }
}


