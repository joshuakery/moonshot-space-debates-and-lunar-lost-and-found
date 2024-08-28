using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharterRules.BallotModule
{
    public class BallotHideConfirms : MonoBehaviour
    {
        public Canvas confirmA;
        public Canvas confirmB;

        private void OnEnable()
        {
            HideConfirms();
        }

        public void HideConfirms()
        {
            confirmA.enabled = false;
            confirmB.enabled = false;
        }
    }
}


