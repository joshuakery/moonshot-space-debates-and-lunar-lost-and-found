using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScavengerHunt.ItemsLoaderModule;
using rlmg.logging;

namespace ScavengerHunt.GameStateModule
{
    [System.Serializable]
    public class ItemsJSON
    {
        public List<string> found;

        public ItemsJSON(List<string> m_found)
        {
            found = m_found;
        }
    }

    [CreateAssetMenu(fileName = "GameState", menuName = "GameState", order = 0)]
    public class GameState : ScriptableObject {

        public int currentRound;

        private MoonshotTeamData _currentTeam;
        private MoonshotTeamData _defaultTeam;

        public MoonshotTeamData currentTeam
        {
            get
            {
                if (_currentTeam != null)
                    return _currentTeam;
                else
                    return _defaultTeam;
            }
            set
            {
                _currentTeam = value;
            }
        }

        public string code;

        public List<string> found;

        string saveDir = "Found";
        public bool recentFind;

        public int target;

        public Item CurrentItem;

        public GameEvent ResetEvent;

        public void Reset()
        {
            code = "";
            found = new List<string>();
            recentFind = false;
            CurrentItem = null;
            _defaultTeam = new MoonshotTeamData();
            _defaultTeam.teamName = "Default Team";
            ResetEvent.Raise();
        }

        public void SaveFoundToFile()
        {
            string savePath = Path.Join(Application.streamingAssetsPath,saveDir);
            DirectoryInfo di = new DirectoryInfo(savePath);

            try
            {
                if (!di.Exists)
                {
                    di.Create();
                    RLMGLogger.Instance.Log("The main directory was created successfully.", MESSAGETYPE.INFO);
                }

                ItemsJSON results = new ItemsJSON(found);
                string json = JsonUtility.ToJson(results);

                string filename = currentTeam.teamName + ".json";
                string filepath = Path.Join(savePath,filename);
                File.WriteAllText(filepath, json);
            }
            catch (Exception e)
            {
                RLMGLogger.Instance.Log(String.Format("Failed to save item counts: {0}.",e.ToString()), MESSAGETYPE.ERROR);
            }
        }

        public void SaveFoundToServer()
        {
            if (Client.instance != null && Client.instance.team != null && Client.instance.team.MoonshotTeamData != null)
            {
                Client.instance.team.MoonshotTeamData.huntNumFound = found.Count;
                ClientSend.SendStationDataToServer(); 
            }
        }


        public void DeleteAllFoundFromFile()
        {
            string savePath = Path.Join(Application.streamingAssetsPath,saveDir);
            DirectoryInfo di = new DirectoryInfo(savePath);
            try
            {
                if (di.Exists)
                {
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete(); 
                    }
                }
            }
            catch (Exception e)
            {
                RLMGLogger.Instance.Log(String.Format("Failed to delete item counts: {0}.",e.ToString()), MESSAGETYPE.ERROR);
            }
        }

        public void UpdateTeamDidActivity()
        {
            if (Client.instance != null && Client.instance.team != null && Client.instance.team.MoonshotTeamData != null)
            {
                Client.instance.team.MoonshotTeamData.didHuntActivity = true;
                ClientSend.SendStationDataToServer();
            }

        }

    }


}


