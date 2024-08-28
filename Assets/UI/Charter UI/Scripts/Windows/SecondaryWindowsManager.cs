using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharterRules.GameStateModule;
using CharterRules.Mission;

namespace CharterRules.WindowsModule
{
    public class SecondaryWindowsManager : GenericWindowManager
    {
        public UIManager uiManager;
        public GameEvent welcomeEvent;
        public GenericWindow1 welcomeWindow;

        public override void CloseAllWindows()
        {
            if (genericWindows == null) { return; }

            if (uiManager.currentEvent == welcomeEvent)
            {
                foreach (GenericWindow1 genericWindow in genericWindows)
                {
                    if (genericWindow != welcomeWindow)
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


