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
using UnityEngine.Video;
using UnityEngine.UI;
using System.Linq;

public class IntroScreen : Screen
{
    [SerializeField]
    private GameObject[] _introImages;

    [SerializeField]
    private GameObject _videoPlayerImage;

    [SerializeField]
    private Animator _progressBar;


    override protected IEnumerator PlayScreenAnimation()
    {
        // Initialize screen elements
        _videoPlayerImage.SetActive(false);
        foreach(GameObject go in _introImages)
        {
            go.SetActive(false);
        }

        QuizPlayer[] activePlayers = ScreenManager.QuizPlayers.Where(x => x.PlayerActive == true).ToArray();
        foreach (QuizPlayer player in activePlayers) {
            player.TurnPlayerOnOff(false);
            player.PlayerJoinName.SetActive(false);
            player.ShowCorrectOrIncorrect(false);
        }

        // 1. Play intro video
        VideoPlayer.Prepare();
        yield return new WaitUntil(() => VideoPlayer.isPrepared);

        VideoPlayer.Play();
        _videoPlayerImage.SetActive(true);
        yield return new WaitWhile(() => VideoPlayer.isPlaying);

        //2.Hides intro video
        _videoPlayerImage.SetActive(false);

        //3.enables player placemats
        foreach(QuizPlayer player in activePlayers)
        {
            player.TurnPlayerOnOff(true);
        }
        //4.plays 3 opening messages
        for(int i = 0; i < _introImages.Length; i++)
        {
            _introImages[i].SetActive(true);
            //show progress bar at second intro image
            if (i == 1)
            {
                _progressBar.Play("Enter");
            }
            audioPlayer.Play(new AudioClip[] { audioLUT[$"Welcome-Narration-{i+1}.mp3"].audioClip });
            yield return new WaitWhile(() => audioPlayer.IsRunning);
            _introImages[i].SetActive(false);
        }
        //5.loads next screen
        ScreenManager.ChangeScreen("Question");
    }

#if UNITY_EDITOR
    public override void NextScreen()
    {
        QuizPlayer[] activePlayers = ScreenManager.QuizPlayers.Where(x => x.PlayerActive == true).ToArray();
        foreach (QuizPlayer player in activePlayers) {
            player.TurnPlayerOnOff(true);
        }

        _progressBar.Play("Idle");

        ScreenManager.ChangeScreen("Question");
    }
#endif
}
