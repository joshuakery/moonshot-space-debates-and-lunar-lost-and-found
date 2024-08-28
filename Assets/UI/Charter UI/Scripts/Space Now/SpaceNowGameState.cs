using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using rlmg.logging;
using Newtonsoft.Json;

using CharterRules.QuestionsLoaderModule;
using CharterRules.SpaceNow.QuestionsLoaderModule;

namespace CharterRules.SpaceNow.GameStateModule
{
    //[System.Serializable]
    //public class ProgressJSON
    //{
    //    public Dictionary<string, QuestionVotes> questionVotes;
    //    public ProgressJSON(Dictionary<string, QuestionVotes> _questionVotes)
    //    {
    //        questionVotes = _questionVotes;
    //    }
    //}

    [System.Serializable]
    public class QuestionVotes
    {
        public Dictionary<int, int> votes;
        public int tiesCount;
        public float time;

        public QuestionVotes()
        {
            votes = new Dictionary<int, int>();
            tiesCount = 0;
            time = 0;
        }
    }

    [CreateAssetMenu(fileName = "Space Now GameState", menuName = "Space Now GameState", order = 0)]
    public class SpaceNowGameState : ScriptableObject
    {
        [Header("Questions")]
        public Dictionary<string, QuestionSet> _allQuestionSets;
        public Dictionary<string, QuestionSet> allQuestionSets
        {
            get
            {
                if (_allQuestionSets == null)
                    _allQuestionSets = new Dictionary<string, QuestionSet>();
                return _allQuestionSets;
            }
            set
            {
                _allQuestionSets = value;
            }
        }

        public string currentCategory;

        public QuestionSet currentQuestionSet
        {
            get
            {
                if (allQuestionSets != null &&
                    !System.String.IsNullOrEmpty(currentCategory) &&
                    allQuestionSets.ContainsKey(currentCategory))
                {
                    return allQuestionSets[currentCategory];
                }
                else
                    return new QuestionSet();
            }
        }

        public int currentQuestionIndex;

        public string currentQuestionKey
        {
            get
            {
                if (currentQuestionSet.keyOrder.Count > 0 &&
                    currentQuestionIndex >= 0 &&
                    currentQuestionIndex < currentQuestionSet.keyOrder.Count)
                {
                    return currentQuestionSet.keyOrder[currentQuestionIndex];
                }
                else
                    return null;
            }
        }

        public string nextQuestionKey
        {
            get
            { 
                int nextQuestionIndex = currentQuestionIndex + 1;
                if (nextQuestionIndex >= 0 &&
                    nextQuestionIndex < currentQuestionSet.keyOrder.Count)
                {
                    return currentQuestionSet.keyOrder[nextQuestionIndex];
                }
                else
                    return null;
            }
        }

        public string prevQuestionKey
        {
            get
            {
                int prevQuestionIndex = currentQuestionIndex - 1;
                if (prevQuestionIndex >= 0 &&
                    prevQuestionIndex < currentQuestionSet.keyOrder.Count)
                {
                    return currentQuestionSet.keyOrder[prevQuestionIndex];
                }
                else
                    return null;
            }
        }

        public QuestionData currentQuestion
        {
            get
            {
                if (currentQuestionKey != null &&
                    currentQuestionSet.questions.ContainsKey(currentQuestionKey))
                {
                    return currentQuestionSet.questions[currentQuestionKey];
                }
                else
                    return null;
            }
        }

        public QuestionData nextQuestion
        {
            get
            {
                if (nextQuestionKey != null &&
                    currentQuestionSet.questions.ContainsKey(nextQuestionKey))
                {
                    return currentQuestionSet.questions[nextQuestionKey];
                }
                else
                    return null;
            }
        }

        public QuestionData prevQuestion
        {
            get
            {
                if (prevQuestionKey != null &&
                    currentQuestionSet.questions.ContainsKey(prevQuestionKey))
                {
                    return currentQuestionSet.questions[prevQuestionKey];
                }
                else
                    return null;
            }
        }

        [System.Serializable]
        public class QuestionVote
        {
            public string category;
            public string title;
            public int option;
            public float time;

            public QuestionVote(string _category, string _title, int _option, float _time)
            {
                category = _category;
                title = _title;
                option = _option;
                time = _time;
            }
        }

        public List<QuestionVote> _questionVotes;
        public List<QuestionVote> questionVotes
        {
            get
            {
                if (_questionVotes == null)
                    _questionVotes = new List<QuestionVote>();
                return _questionVotes;
            }
        }


        public int targetNumQuestions = 1;

        [Header("Responses")]
        public string _contentFilename;
        public string contentFilename
        {
            get
            {
                if (System.String.IsNullOrEmpty(_contentFilename))
                    return "responses.json";
                else
                    return _contentFilename;
            }
            set
            {
                _contentFilename = value;
            }
        }

        public Dictionary<string, Dictionary<string, Dictionary<int, int>>> _responses;
        public Dictionary<string, Dictionary<string, Dictionary<int, int>>> responses
        {
            get
            {
                if (_responses == null)
                    _responses = new Dictionary<string, Dictionary<string, Dictionary<int, int>>>();
                return _responses;
            }
            set
            {
                _responses = value;
            }
        }

        public Dictionary<int, int> currentResponses
        {
            get
            {
                if (responses.ContainsKey(currentCategory) &&
                    responses[currentCategory] != null &&
                    currentQuestion != null &&
                    responses[currentCategory].ContainsKey(currentQuestion.title))
                {
                    return responses[currentCategory][currentQuestion.title];
                }
                else
                    return null;
            }
        }

        public int totalCurrentResponses
        {
            get
            {
                if (currentResponses != null)
                {
                    return currentResponses.Sum(kvp => kvp.Value);
                }
                else
                    return 0;
            }
        }


        [Header("Game Events")]
        public GameEvent StartVoting;
        public GameEvent FirstVote;
        public GameEvent EndVoting;

        public void Reset()
        {
            currentQuestionIndex = 0;

            questionVotes.Clear();

            //ShuffleQuestionDataOrder();

            //ResetVotes();
            //ResetScales();
            //RecalcProgress();
        }

        public void Vote(int optionID)
        {
            QuestionVote newVote = new QuestionVote(currentCategory, currentQuestion.title, optionID, 0f);
            questionVotes.Add(newVote);
        }

        public void RecordCurrentResponse()
        {
            QuestionVote currentVote = questionVotes.Last();

            if (!responses.ContainsKey(currentVote.category))
                responses[currentVote.category] = new Dictionary<string, Dictionary<int, int>>();

            if (!responses[currentVote.category].ContainsKey(currentVote.title))
                responses[currentVote.category][currentVote.title] = new Dictionary<int, int>();

            if (!responses[currentVote.category][currentVote.title].ContainsKey(currentVote.option))
                responses[currentVote.category][currentVote.title][currentVote.option] = 0;

            responses[currentVote.category][currentVote.title][currentVote.option] += 1;
            SaveResponses();
        }

        private void SaveResponses()
        {
            try
            {
                string json = JsonConvert.SerializeObject(
                    new Dictionary<string, Dictionary<string, Dictionary<int, int>>>(responses),
                    Newtonsoft.Json.Formatting.Indented
                );

                string filepath = Path.Join(Application.streamingAssetsPath, contentFilename);
                File.WriteAllText(filepath, json);

            }
            catch (Exception e)
            {
                RLMGLogger.Instance.Log(String.Format("The process failed: {0}.", e.ToString()), MESSAGETYPE.ERROR);
            }
        }

    }
}




