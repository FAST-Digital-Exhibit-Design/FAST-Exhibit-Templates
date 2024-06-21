//=============================================================================
// FAST Quiz Show
//
// A FAST digital exhibit experience that allows for multiplayer
// participation, in-depth exploration of a topic, multiple-choice questions,
// and true/false questions.
//
// Copyright (C) 2024 Museum of Science, Boston
// <https://www.mos.org/>
//
// This software was developed through a grant to the Museum of Science, Boston
// from the Institute of Museum and Library Services under
// Award #MG-249646-OMS-21. For more information about this grant, see
// <https://www.imls.gov/grants/awarded/mg-249646-oms-21>.
//
// This software is open source: you can redistribute it and/or modify
// it under the terms of the MIT License.
//
// This software is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// MIT License for more details.
//
// You should have received a copy of the MIT License along with this software.
// If not, see <https://opensource.org/license/MIT>.
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FAST;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UIElements;

public class JoinScreen : Screen
{
    [SerializeField]
    private ProgressManager _progressManager;
    [SerializeField]
    private GameObject _videoPlayerImage;

    [SerializeField]
    private GameObject[] _playerJoinNarration;

    [SerializeField]
    private GameObject _joinPromptNarration;

    private float _timeBeforeReset = 30f;
    private bool _joinPhaseBegun;
    private bool _endIsRunning;

    private Coroutine _returnTimer;
    private Coroutine _joinTimer;

    private void Start()
    {
        _joinPhaseBegun = false;
        _endIsRunning = false;
    }

    public override void KeyPressRecieved(int playerIndex, int answerIndex)
    {
        if (!ScreenManager.QuizPlayers[playerIndex].PlayerActive)
        {
            if (!_joinPhaseBegun)
            {
                _joinPhaseBegun = true;
                StopCoroutine(_returnTimer);
                _joinTimer = StartCoroutine(JoinScreenAnimation());
            }
            ScreenManager.QuizPlayers[playerIndex].PressToJoin.SetActive(false);
            ScreenManager.QuizPlayers[playerIndex].PlayerActive = true;
            ScreenManager.QuizPlayers[playerIndex].PlayerJoinName.SetActive(true);
            if (audioPlayer.IsRunning)
            {
                audioPlayer.AddToPlaylist(audioLUT[$"Join-Narration-{playerIndex + 1}.mp3"].audioClip);
            }
            else
            {
                audioPlayer.Play(new AudioClip[] { audioLUT[$"Join-Narration-{playerIndex + 1}.mp3"].audioClip });
            }

            if (ScreenManager.QuizPlayers.All(x => x.PlayerActive == true))
            {
                StopCoroutine(_joinTimer);
                if (!_endIsRunning)
                {
                    StartCoroutine(OnJoinTimerEnd());
                }
            }
        }
    }

    override protected IEnumerator PlayScreenAnimation()
    {
        // Initialize screen elements
        _videoPlayerImage.SetActive(false);

        audioPlayer.Play(new AudioClip[] { audioLUT["JoinPrompt-Narration.mp3"].audioClip });

        //turn on press to join message and start title timer
        foreach (QuizPlayer player in ScreenManager.QuizPlayers)
        {
            player.PressToJoin.SetActive(true);
        }
        _returnTimer = StartCoroutine(ReturnToTitleTimer());
        yield return null;
    }

    private protected IEnumerator JoinScreenAnimation()
    {
        VideoPlayer.Prepare();
        yield return new WaitUntil(() => VideoPlayer.isPrepared);

        VideoPlayer.Play();
        yield return null;
        _videoPlayerImage.SetActive(true);
        yield return new WaitWhile(() => VideoPlayer.isPlaying);

        if (!_endIsRunning)
        {
           StartCoroutine(OnJoinTimerEnd());
        }
    }

    private protected IEnumerator ReturnToTitleTimer()
    {
        //idle timer begins
        yield return new WaitForSeconds(_timeBeforeReset);
        //nothing happens, reset to title screen
        ScreenManager.ChangeScreen("Title");
    }

    private protected IEnumerator OnJoinTimerEnd()
    {
        _endIsRunning = true;
        VideoPlayer.Stop();
        _videoPlayerImage.SetActive(false);

        yield return new WaitWhile(()=> audioPlayer.IsRunning);
        Debug.Log("<b>STARTING GAME</b>");
        _endIsRunning = false;

        QuizPlayer[] activePlayers = ScreenManager.QuizPlayers.Where(x=>x.PlayerActive).ToArray();
        _progressManager.DeterminePointsNeededToWin(activePlayers.Length);
        foreach (QuizPlayer player in activePlayers)
        {
            player.ResetScore();
        }
        ScreenManager.ChangeScreen("Intro");
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        foreach (QuizPlayer player in ScreenManager.QuizPlayers)
        {
            player.PressToJoin.SetActive(false);
        }
        _joinPhaseBegun = false;
    }

#if UNITY_EDITOR
    public override void NextScreen()
    {
        QuizPlayer[] activePlayers = ScreenManager.QuizPlayers.Where(x => x.PlayerActive).ToArray();
        _progressManager.DeterminePointsNeededToWin(activePlayers.Length);
        foreach (QuizPlayer player in activePlayers) {
            player.ResetScore();
        }

        if (ScreenManager.QuizPlayers.Any(x => x.PlayerActive == true)) {
          
            ScreenManager.ChangeScreen("Intro");
        }
        else {
            ScreenManager.ChangeScreen("Title");
        }
    }
#endif
}