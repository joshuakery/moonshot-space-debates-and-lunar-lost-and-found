using UnityEngine;
using System.Collections;
using ScavengerHunt.GameStateModule;
using ScavengerHunt.UIManagerModule;
using rlmg.logging;

namespace ScavengerHunt.ClientSubscriberModule
{
    public class ClientSubscriber : MonoBehaviour
    {
        public UIManager uiManager;
        public Timer mainTimer;
        public Timer closeTimer;
        public Timer bufferTimer;

        public GameState gameState;

        public bool useServer;

        public GameEvent SharedConclusionEvent;

        private void OnEnable()
        {
            if (useServer)
            {
                Client.instance.onStartRound += StartRound;
                Client.instance.onPauseMission += PauseMission;
                Client.instance.onUnPauseMission += UnPauseMission;
                Client.instance.onEndMission += EndMission;
                Client.instance.onResumeRound += ResumeRound;
            }
        }

        private void OnDisable()
        {
            if (Client.instance != null)
            {
                Client.instance.onStartRound -= StartRound;
                Client.instance.onPauseMission -= PauseMission;
                Client.instance.onUnPauseMission -= UnPauseMission;
                Client.instance.onEndMission -= EndMission;
                Client.instance.onResumeRound -= ResumeRound;
            }
        }

        public void StartRound(
            string _teamName,
            float _roundDuration,
            float _roundBufferDuration,
            int _round,
            string _JsonTeamData
        )
        {
            StartCoroutine(
                _StartRound(
                    _teamName,
                    _roundDuration,
                    _roundBufferDuration,
                    _round,
                    _JsonTeamData
                    )
                );
        }

        private IEnumerator _StartRound(
            string _teamName,
            float _roundDuration,
            float _roundBufferDuration,
            int _round,
            string _JsonTeamData
        )
        {
            gameState.currentTeam = Client.instance.team.MoonshotTeamData;

            //Reset Timer
            mainTimer.duration = _roundDuration;
            mainTimer.Reset();
            mainTimer.StartCounting();

            //Close Timer - just for animating the station to a close
            closeTimer.duration = _roundDuration + _roundBufferDuration - 1; //close 1 second before next round
            closeTimer.Reset();
            closeTimer.StartCounting();

            //Buffer Timer - just for visual display
            bufferTimer.duration = _roundBufferDuration;
            bufferTimer.Reset();

            yield return StartCoroutine(uiManager.ResetGame());
        }

        public void ResumeRound(
            string _teamName,
            float _roundDurationRemaining,
            float _roundBufferDurationRemaining,
            MissionState _missionState,
            int _round,
            string _JsonTeamData
        )
        {
            StartCoroutine(
                _ResumeRound(
                    _teamName,
                    _roundDurationRemaining,
                    _roundBufferDurationRemaining,
                    _missionState,
                    _round,
                    _JsonTeamData
                   )
            );
        }

        private IEnumerator _ResumeRound(
            string _teamName,
            float _roundDurationRemaining,
            float _roundBufferDurationRemaining,
            MissionState _missionState,
            int _round,
            string _JsonTeamData
        )
        {
            gameState.currentTeam = Client.instance.team.MoonshotTeamData;

            //Reset Timer
            mainTimer.duration = _roundDurationRemaining;
            mainTimer.Reset();
            if (_missionState == MissionState.Running) mainTimer.StartCounting();

            //Close Timer - just for animating the station to a close
            closeTimer.duration = _roundDurationRemaining + _roundBufferDurationRemaining - 1; //close 1 second before next round
            closeTimer.Reset();
            if (_missionState == MissionState.Running) closeTimer.StartCounting();

            //Buffer Timer - just for visual display
            bufferTimer.duration = _roundBufferDurationRemaining;
            bufferTimer.Reset();

            yield return StartCoroutine(uiManager.ResetGame(false));
        }

        private void PauseMission()
        {
            closeTimer.PauseCounting();
            mainTimer.PauseCounting();
            bufferTimer.PauseCounting();
        }

        private void UnPauseMission()
        {
            closeTimer.StartCounting();

            mainTimer.StartCounting();

            if (mainTimer.time <= 0)
                bufferTimer.StartCounting();
        }

        private void EndMission()
        {
            if (SharedConclusionEvent != null)
                SharedConclusionEvent.Raise();
        }
    }
}



