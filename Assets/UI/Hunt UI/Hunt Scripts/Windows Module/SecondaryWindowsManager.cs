using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ScavengerHunt.UIManagerModule;

namespace ScavengerHunt.WindowsModule
{

    public class SecondaryWindowsManager : GenericWindowManager
    {
        [System.Serializable]
        public class Exclusion
        {
            public GameEvent gameEvent;
            public GenericWindow1[] excluded;
        }

        public Exclusion[] exclusions;

        public Dictionary<GameEvent, Exclusion> exclusionsDict;

        public UIManager uiManager;

        // Start is called before the first frame update
        void Awake()
        {
            exclusionsDict = new Dictionary<GameEvent, Exclusion>();
            foreach (Exclusion exclusion in exclusions)
            {
                exclusionsDict.Add(exclusion.gameEvent, exclusion);
            }
        }


        public void CloseAllWindowsWithExclusions()
        {
            if (genericWindows == null) { return; }

            if (exclusionsDict.ContainsKey(uiManager.currentEvent))
            {
                Exclusion exclusion = exclusionsDict[uiManager.currentEvent];
                foreach (GenericWindow1 genericWindow in genericWindows)
                {
                    if (!exclusion.excluded.Contains(genericWindow))
                        genericWindow.Close();
                }
            }
            else
            {
                foreach (GenericWindow1 genericWindow in genericWindows)
                {
                    genericWindow.Close();
                }
            }
        }
    }
}

