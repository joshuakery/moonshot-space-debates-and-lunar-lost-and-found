using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.EventSystems;
using CharterRules.GameStateModule;
using rlmg.logging;

namespace CharterRules.QuestionsLoaderModule
{
    [System.Serializable]
    public class ScaleAffect
    {
        public float meter1;
        public float meter2;

        public float meter3;

        public ScaleAffect(float nmeter1, float nmeter2, float nmeter3)
        {
            meter1 = nmeter1;
            meter2 = nmeter2;
            meter3 = nmeter3;
        }
    }

    [System.Serializable]
    public class Option
    {
        public string argument;
        public string resolution;
        public ScaleAffect scaleAffect;
        public string arguerImageFilename;
        public Texture2D arguerImageTex;

        public Option(string nargument, string nresolution, ScaleAffect nscaleAffect)
        {
            argument = nargument;
            resolution = nresolution;
            scaleAffect = nscaleAffect;
        }
    }

    [System.Serializable]
    public class Options
    {
        public Option optionA;
        public Option optionB;

        public Options(Option noptionA, Option noptionB)
        {
            optionA = noptionA;
            optionB = noptionB;
        }
    }

    [System.Serializable]
    public class QuestionData
    {
        public Options options;
        public string situation;

        public string onBallot;
        public string title;

        public QuestionData(Options noptions, string nsituation, string nonBallot, string ntitle)
        {
            options = noptions;
            situation = nsituation;
            onBallot = nonBallot;
            title = ntitle;
        }
    }
    public class QuestionsLoader : ContentLoader
    {
        public GameState gameState;

        public string imageDir = "Arguer_Images";

        private Texture2D GetTexture2DFromPath(string path)
        {
            Texture2D tex = new Texture2D(2,2);

            try
            {
                byte[] bytes = File.ReadAllBytes(path);
                tex.LoadImage(bytes);
            }
            catch (Exception e)
            {
                RLMGLogger.Instance.Log(String.Format("Failed to read item images: {0}.",e.ToString()), MESSAGETYPE.ERROR);
            }

            return tex;
        }
        protected override IEnumerator PopulateContent(string contentData)
        {
            Dictionary<string,QuestionData> questionJson = JsonConvert.DeserializeObject<Dictionary<string,QuestionData>>(contentData);

            if (questionJson == null)
                yield break;

            string dirPath = Path.Join(Application.streamingAssetsPath,imageDir);

            foreach (KeyValuePair<string,QuestionData> kvp in questionJson)
            {
               QuestionData questionData = kvp.Value;

                string optionAArguerImagePath = Path.Join(dirPath,questionData.options.optionA.arguerImageFilename);
                questionData.options.optionA.arguerImageTex = GetTexture2DFromPath(optionAArguerImagePath);

                string optionBArguerImagePath = Path.Join(dirPath,questionData.options.optionB.arguerImageFilename);
                questionData.options.optionB.arguerImageTex = GetTexture2DFromPath(optionBArguerImagePath);
            }

            gameState.allQuestionData = questionJson;

            //create shuffled list of keys for question order
            List<string> keys = new List<string>(questionJson.Keys);
            keys.Shuffle();
            gameState.allQuestionDataOrder = keys.ToArray();

            gameState.Reset(); //redundant?

            yield break;
        }
    }
}

