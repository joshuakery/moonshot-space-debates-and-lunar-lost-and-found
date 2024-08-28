using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwaitingServerScreen : MonoBehaviour
{
    public GenericWindow1 genericWindow;

    private void Awake()
    {
        if (genericWindow == null) { genericWindow = GetComponent<GenericWindow1>(); }
    }

    private void Start()
    {
        if (genericWindow != null)
        {
            genericWindow.Open();
            genericWindow.uiTweener.sequenceManager.CompleteCurrentSequence();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            if (genericWindow.isOpen) { genericWindow.Close(); }
            else { genericWindow.Open(); }
        }
    }

    private void OnEnable()
    {
        if (Client.instance != null)
        {
            Client.instance.onStartRound += StartRound;
            Client.instance.onResumeRound += ResumeRound;
            Client.instance.onStopMission += StopMission;
        }
    }

    private void OnDisable()
    {
        if (Client.instance != null)
        {
            Client.instance.onStartRound -= StartRound;
            Client.instance.onResumeRound -= ResumeRound;
            Client.instance.onStopMission -= StopMission;
        }
    }

    private void StartRound(string _teamName, float _roundDuration, float _roundBufferDuration, int _round, string _JsonTeamData)
    {
        if (genericWindow != null) { genericWindow.Close(); }
    }

    private void ResumeRound(string _teamName, float _roundDurationRemaining, float _roundBufferDurationRemaining, MissionState _missionState, int _round,  string _JsonTeamData)
    {
        if (genericWindow != null) { genericWindow.Close(); }
    }

    private void StopMission()
    {
        if (genericWindow != null) { genericWindow.Open(); }
    }


}
