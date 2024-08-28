using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using rlmg.logging;
using Newtonsoft.Json;
using CharterRules.QuestionsLoaderModule;

namespace CharterRules.GameStateModule
{    
    [System.Serializable]
	public class ProgressJSON
	{
        public Dictionary<string,QuestionVotes> questionVotes;
        public ProgressJSON(Dictionary<string,QuestionVotes> _questionVotes)
        {
            questionVotes = _questionVotes;
        }
	}

    [System.Serializable]
    public class QuestionVotes
    {
        public Dictionary<int,int> votes;
        public int tiesCount;
        public float time;

        public QuestionVotes()
        {
            votes = new Dictionary<int,int>();
            tiesCount = 0;
            time = 0;
        }
    }

    //[Serializable]
    //public class Team
    //{
    //    public string directory;
    //    public string name;

    //    public Team (string m_directory, string m_name)
    //    {
    //        directory = m_directory;
    //        name = m_name;
    //    }
    //}

    [CreateAssetMenu(fileName = "GameState", menuName = "GameState", order = 0)]
    public class GameState : ScriptableObject {

        [Header("Connectivity")]
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

        public MoonshotTeamData[] teams;


        [Header("Game Events")]
        public GameEvent StartVoting;
        public GameEvent FirstVote;
        public GameEvent EndVoting;


        [Header("Questions")]

        public Dictionary<string,QuestionData> allQuestionData;
        public string[] allQuestionDataOrder;
        public int currentQuestionIndex;
     
        [SerializeField]
        public Dictionary<string,List<int>> allQuestionVoters;

        public Dictionary<string,QuestionVotes> allQuestionVotes;

        public string saveDir = "Progress";

        public int maxVotesAccepted = 6;
        public int targetNumQuestions = 5;
        public bool tieMode = false;

        [Header("Meters")]
        public float meter1Default;
        public float meter1;
        public float meter2Default;
        public float meter2;

        public float meter3Default;
        public float meter3;

        public List<string> progress;

        public void AddVoter(int ballotID)
        {
            string currentQuestionID = allQuestionDataOrder[currentQuestionIndex];
            if (!allQuestionVoters.ContainsKey(currentQuestionID) ||
                allQuestionVoters[currentQuestionID].Count == 0)
            {
                allQuestionVoters[currentQuestionID] = new List<int>();
                StartVoting.Raise();
            }
            List<int> currentVoters = allQuestionVoters[currentQuestionID];
            currentVoters.Add(ballotID);
        }

        public void Vote(int ballotID, int optionID)
        {
            if (!allQuestionVotes.ContainsKey(allQuestionDataOrder[currentQuestionIndex]) ||
                allQuestionVotes[allQuestionDataOrder[currentQuestionIndex]].votes == null) //if this is the first vote
            {
                allQuestionVotes[allQuestionDataOrder[currentQuestionIndex]] = new QuestionVotes();
            }

            QuestionVotes currentVotes = allQuestionVotes[allQuestionDataOrder[currentQuestionIndex]];

            //check if voter has already voted for this question
            if (!DidVote(ballotID))
            {
                currentVotes.votes.Add(ballotID,optionID);
            }
            // else if (tieMode)
            // {
            //     //allow overwrites in tie mode
            //     currentVotes.votes[ballotID] = optionID;
            // }

            //First Vote Event
            if (allQuestionVotes[allQuestionDataOrder[currentQuestionIndex]].votes.Count == 1)
            {
                FirstVote.Raise();
            }
            
            //End Voting Event - if the number of ballots opened == votes cast
            // if (!tieMode &&
            if (
                (   currentVotes.votes.Count >= allQuestionVoters[allQuestionDataOrder[currentQuestionIndex]].Count ||
                    currentVotes.votes.Count >= maxVotesAccepted
                )
                )
            {
                EndVoting.Raise();
            }
        }

        public bool IsFirstBallot(int ballotID)
        {
            string currentQuestionID = allQuestionDataOrder[currentQuestionIndex];

            if (!allQuestionVoters.ContainsKey(currentQuestionID) ||
                allQuestionVoters[currentQuestionID].Count == 0)
                return false;

            List<int> currentVoters = allQuestionVoters[currentQuestionID];

            if (currentVoters[0] == ballotID)
                return true;
            else
                return false;
        }

        public bool DidVote(int ballotID)
        {
            if (!allQuestionVotes.ContainsKey(allQuestionDataOrder[currentQuestionIndex]) ||
                allQuestionVotes[allQuestionDataOrder[currentQuestionIndex]].votes == null)
                return false;

            QuestionVotes currentVotes = allQuestionVotes[allQuestionDataOrder[currentQuestionIndex]];

            return (currentVotes.votes.ContainsKey(ballotID));
        }

        public int GetVote(int ballotID)
        {
            if (!allQuestionVotes.ContainsKey(allQuestionDataOrder[currentQuestionIndex]) ||
                allQuestionVotes[allQuestionDataOrder[currentQuestionIndex]].votes == null)
                return -1;

            QuestionVotes currentVotes = allQuestionVotes[allQuestionDataOrder[currentQuestionIndex]];

            if (currentVotes.votes.ContainsKey(ballotID))
            {
                int optionID = currentVotes.votes[ballotID];
                return optionID;
            }
            else
                return -1;
        }

        public void ResetVotes()
        {
            allQuestionVoters = new Dictionary<string,List<int>>();
            allQuestionVotes = new Dictionary<string,QuestionVotes>();
        }

        public void ResetVoteForCurrentQuestion()
        {
            ResetVotesForQuestion(allQuestionDataOrder[currentQuestionIndex]);
        }

        public void ResetVoteForCurrentQuestion(bool onlyRemoveVotersWhoDidNotVote)
        {
            ResetVotesForQuestion(allQuestionDataOrder[currentQuestionIndex], onlyRemoveVotersWhoDidNotVote);
        }

        public void ResetVotesForQuestion(string questionID)
        {
            if (allQuestionData != null && allQuestionData.ContainsKey(questionID))
            {
                if (allQuestionVotes != null && allQuestionVotes.ContainsKey(questionID))
                    allQuestionVotes[questionID].votes.Clear();
                else
                    allQuestionVotes[questionID] = new QuestionVotes();
                
                if (allQuestionVoters != null && allQuestionVoters.ContainsKey(questionID))
                    allQuestionVoters[questionID].Clear();
                else
                    allQuestionVoters[questionID] = new List<int>();
            }
        }

        public void ResetVotesForQuestion(string questionID, bool onlyRemoveVotersWhoDidNotVote)
        {
            if (allQuestionData != null && allQuestionData.ContainsKey(questionID))
            {
                //Remove voters first, because this depends on DidVote() looking at the votes
                if (onlyRemoveVotersWhoDidNotVote)
                {
                    if (allQuestionVoters != null && allQuestionVoters.ContainsKey(questionID))
                    {
                        List<int> voters = allQuestionVoters[questionID];
                        List<int> votersWhoVoted = voters.Where(voterID => DidVote(voterID)).ToList();
                        allQuestionVoters[questionID] = votersWhoVoted;
                    }
                    else
                    {
                        allQuestionVoters[questionID] = new List<int>();
                    }
                }
                else
                {
                    if (allQuestionVoters != null && allQuestionVoters.ContainsKey(questionID))
                        allQuestionVoters[questionID].Clear();
                    else
                        allQuestionVoters[questionID] = new List<int>();
                }

                //Secondly, clear the votes
                if (allQuestionVotes != null && allQuestionVotes.ContainsKey(questionID))
                    allQuestionVotes[questionID].votes.Clear();
                else
                    allQuestionVotes[questionID] = new QuestionVotes();
            }
        }

        public void SetTieMode(bool value)
        {
            tieMode = value;
        }

        public int CountVotesForOptionForCurrentQuestion(int option)
        {
            string currentQuestionID = allQuestionDataOrder[currentQuestionIndex];
            return CountVotesForOption(currentQuestionID,option);
        }

        public int CountVotesForOption(string questionID, int option)
        {
            int count = 0;
            if (allQuestionVotes.ContainsKey(questionID) &&
                allQuestionVotes[questionID].votes != null
            )
            {
                QuestionVotes questionVotes = allQuestionVotes[questionID];
                count = questionVotes.votes.Where(kvp => kvp.Value == option).Count();
            }
            return count;
        }

        public int[] GetWinners(string questionID)
        {
            List<int> winners = new List<int>();

            if (allQuestionVotes.ContainsKey(questionID) &&
                allQuestionVotes[questionID].votes != null
            )
            {
                QuestionVotes questionVotes = allQuestionVotes[questionID];
                List<int> optionsVotedFor = questionVotes.votes.Values.Distinct().ToList();

                int greatest = 0;
                foreach (int optionVotedFor in optionsVotedFor)
                {
                    int count = questionVotes.votes.Where(kvp => kvp.Value == optionVotedFor).Count();
                    if (count > greatest)
                    {
                        winners.Clear();
                        winners.Add(optionVotedFor);
                        greatest = count;
                    }
                    else if (count == greatest)
                    {
                        winners.Add(optionVotedFor);
                    }
                }
            }
            
            return winners.ToArray();
        }

        public int CountQuestionsDecided()
        {
            return allQuestionData.Where(kvp =>
                GetWinners(kvp.Key).Length == 1
            ).ToArray().Length;
        }

        public void RecordTime(Timer timer)
        {
            if (allQuestionVotes != null &&
                allQuestionVotes.ContainsKey(allQuestionDataOrder[currentQuestionIndex])
            )
            {
                QuestionVotes currentVotes = allQuestionVotes[allQuestionDataOrder[currentQuestionIndex]];

                currentVotes.time = timer.elapsed;
            }
        }

        public void ResetScales()
        {
            meter1 = meter1Default;
            meter2 = meter2Default;
            meter3 = meter3Default;
        }

        public void RecalcScales()
        {
            // Debug.Log("Recalcing Scales");
            ResetScales();
            if (allQuestionVotes == null) return;
            foreach (string questionID in allQuestionVotes.Keys)
            {
                int[] winners = GetWinners(questionID);

                if (winners.Length == 1)
                {
                    QuestionData questionData = allQuestionData[questionID];

                    ScaleAffect scaleAffect = new ScaleAffect(0,0,0);
                    if (winners[0] == 0)
                    {
                        scaleAffect = questionData.options.optionA.scaleAffect;
                    }
                    else if (winners[0] == 1)
                    {
                        scaleAffect = questionData.options.optionB.scaleAffect;
                    }
                    
                    meter1 += scaleAffect.meter1;
                    meter2 += scaleAffect.meter2;
                    meter3 += scaleAffect.meter3;
                }

            }
        }

        public void RecalcProgress()
        {
            progress = new List<string>();
            foreach (string questionID in allQuestionVotes.Keys)
            {
                int[] winners = GetWinners(questionID);

                if (winners.Length == 1)
                {
                    QuestionData questionData = allQuestionData[questionID];

                    string resolution = "";
                    if (winners[0] == 0)
                    {
                        resolution = questionData.options.optionA.resolution;
                    }
                    else if (winners[0] == 1)
                    {
                        resolution = questionData.options.optionB.resolution;
                    }
                
                    progress.Add(resolution);
                }

            }
        }
        public void SaveProgressToFile()
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

                string json = JsonConvert.SerializeObject(
                    new ProgressJSON(allQuestionVotes),
                    Newtonsoft.Json.Formatting.Indented
                );

                if (currentTeam != null && !System.String.IsNullOrEmpty(currentTeam.teamName))
                {
                    string filename = currentTeam.teamName + ".json";
                    string filepath = Path.Join(savePath, filename);
                    File.WriteAllText(filepath, json);
                }


            }
            catch (Exception e)
            {
                RLMGLogger.Instance.Log(String.Format("The process failed: {0}.",e.ToString()), MESSAGETYPE.ERROR);
            }
        }

        public void SaveProgressToServer()
        {
            if (Client.instance != null && Client.instance.team != null && Client.instance.team.MoonshotTeamData != null)
            {
                Client.instance.team.MoonshotTeamData.charterQsCompleted = CountQuestionsDecided();
                Client.instance.team.MoonshotTeamData.charterMeterDecisions = meter1;
                Client.instance.team.MoonshotTeamData.charterMeterPriorities = meter2;
                Client.instance.team.MoonshotTeamData.charterMeterStrictness = meter3;

                ClientSend.SendStationDataToServer();
            }
        }

        public void DeleteAllProgressFromFile()
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
                RLMGLogger.Instance.Log(String.Format("Failed to delete all progress. The process failed: {0}.",e.ToString()), MESSAGETYPE.ERROR);
            }  
        }

        private void ShuffleQuestionDataOrder()
        {
            if (allQuestionDataOrder != null && allQuestionDataOrder.Length > 0)
            {
                List<string> keys = allQuestionDataOrder.ToList();
                keys.Shuffle();
                allQuestionDataOrder = keys.ToArray();
            }
        }

        public void Reset()
        {
            _defaultTeam = new MoonshotTeamData();
            _defaultTeam.teamName = "Default Team";

            // if (allQuestionDataOrder.Length > 0)
            //     allQuestionDataOrder[currentQuestionIndex] = allQuestionDataOrder[0];
            currentQuestionIndex = 0;

            ShuffleQuestionDataOrder();

            ResetVotes();
            ResetScales();
            RecalcProgress();
        }

        public void UpdateTeamDidActivity()
        {
            if (Client.instance != null && Client.instance.team != null && Client.instance.team.MoonshotTeamData != null)
            {
                Client.instance.team.MoonshotTeamData.didCharterActivity = true;
                ClientSend.SendStationDataToServer();
            }

        }

    }
}


