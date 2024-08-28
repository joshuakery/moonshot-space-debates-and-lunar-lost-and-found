using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.EventSystems;

using CharterRules.QuestionsLoaderModule;
using CharterRules.SpaceNow.GameStateModule;
using rlmg.logging;

namespace CharterRules.SpaceNow.QuestionsLoaderModule
{
    [System.Serializable]
    public class QuestionSet
    {
        public string title;

        public string subtitle;

        public string iconFilename;
        public Texture2D icon;

        public string illustrationFilename;
        public Texture2D illustration;

        public string funFact;

        public Dictionary<string, QuestionData> questions;
        
        public List<string> keyOrder;

        public QuestionSet()
        {
            title = "Empty Question Set";
            iconFilename = "";
            questions = new Dictionary<string,QuestionData>();
            keyOrder = new List<string>();
        }
    }

    public class SpaceNowQuestionsLoader : ContentLoader
    {
        public SpaceNowGameState gameState;

        public string imageDir = "Arguer_Images";

        public string iconDir = "Category_Icons";

        public string illustrationDir = "Category_Illustrations";

        public GameEvent loadedEvent;

        private Texture2D GetTexture2DFromPath(string path)
        {
            Texture2D tex = new Texture2D(2, 2);

            try
            {
                byte[] bytes = File.ReadAllBytes(path);
                tex.LoadImage(bytes);
            }
            catch (Exception e)
            {
                RLMGLogger.Instance.Log(String.Format("Failed to read item images: {0}.", e.ToString()), MESSAGETYPE.ERROR);
            }

            return tex;
        }
        protected override IEnumerator PopulateContent(string contentData)
        {
            Dictionary<string, QuestionSet> questionSets = JsonConvert.DeserializeObject<Dictionary<string, QuestionSet>>(contentData);

            if (questionSets == null)
                yield break;

            string dirPath = Path.Join(Application.streamingAssetsPath, imageDir);

            foreach (KeyValuePair<string, QuestionSet> kvp in questionSets)
            {
                QuestionSet questionSet = kvp.Value;

                //Question Set Icon and Illustration
                if (!System.String.IsNullOrEmpty(questionSet.iconFilename))
                {
                    string iconImagePath = Path.Join(Application.streamingAssetsPath, iconDir, questionSet.iconFilename);
                    questionSet.icon = GetTexture2DFromPath(iconImagePath);
                }

                if (!System.String.IsNullOrEmpty(questionSet.illustrationFilename))
                {
                    string illustrationImagePath = Path.Join(Application.streamingAssetsPath, illustrationDir, questionSet.illustrationFilename);
                    questionSet.illustration = GetTexture2DFromPath(illustrationImagePath);
                }

                //Questions Arguer Images
                foreach (KeyValuePair<string,QuestionData> dataKVP in questionSet.questions)
                {
                    QuestionData questionData = dataKVP.Value;

                    string optionAArguerImagePath = Path.Join(dirPath, questionData.options.optionA.arguerImageFilename);
                    questionData.options.optionA.arguerImageTex = GetTexture2DFromPath(optionAArguerImagePath);

                    string optionBArguerImagePath = Path.Join(dirPath, questionData.options.optionB.arguerImageFilename);
                    questionData.options.optionB.arguerImageTex = GetTexture2DFromPath(optionBArguerImagePath);

                    questionSet.keyOrder.Add(dataKVP.Key);
                }
            }

            gameState.allQuestionSets = questionSets;

            if (loadedEvent != null) { loadedEvent.Raise(); }

            yield break;
        }
    }
}

