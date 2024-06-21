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

public class MidpointScreen : Screen
{

    [SerializeField]
    private GameObject _videoPlayerImage;

    [SerializeField]
    private QuestionScreen _questionScreen;

    override protected IEnumerator PlayScreenAnimation()
    {
        _videoPlayerImage.SetActive(false);
        VideoPlayer.Prepare();
        yield return new WaitWhile(() => VideoPlayer.isPrepared);

        foreach (QuizPlayer player in _questionScreen.ActivePlayers)
        {
            player.TurnOffForEndState();
        }

        VideoPlayer.Play();
        _videoPlayerImage.SetActive(true);
        audioPlayer.Play(new AudioClip[] { audioLUT["Midpoint-Narration.mp3"].audioClip });
        yield return new WaitWhile(() => VideoPlayer.isPlaying);
        ProgressManager.PastMidpoint = true;
        ScreenManager.ChangeScreen("Question");
    }

    protected override void OnDisable()
    {
        _videoPlayerImage.SetActive(false);
        base.OnDisable();
    }
}
