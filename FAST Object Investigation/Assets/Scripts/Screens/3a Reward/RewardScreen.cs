//=============================================================================
// FAST Object Investigation
//
// A FAST digital exhibit experience that allows for exploration of real
// objects or physical models, identification or classification, and
// learning about a variety of items.
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
using UnityEngine.UI;
using UnityEngine.Video;
using FAST;

public class RewardScreen : ActivityScreen
{
    [Header("Video")]
    [SerializeField]
    private VideoPlayer videoPlayer;
    [SerializeField]
    private VideoPlayerFromFile videoPlayerFromFile;
    [SerializeField]
    private RawImage videoImage;

    override protected IEnumerator PlayScreenAnimation()
    {
        // Load and initialize
        videoImage.CrossFadeAlpha(0f, 0f, false);

        videoPlayerFromFile.baseFileName = string.Format("Reward-Video-{0}.mp4", screenManager.teaserName);
        videoPlayerFromFile.Load(FAST.Application.language);
        videoPlayer.Prepare();

        // Animate
        yield return null;
        audioPlayer.Play(new AudioClip[] { audioLUT["Reward-Correct-SoundEffect.mp3"].audioClip });

        yield return new WaitWhile(() => audioPlayer.IsRunning);
        yield return new WaitUntil(() => videoPlayer.isPrepared);
        videoPlayer.Play();

        yield return new WaitForSecondsRealtime(0.1f);
        videoImage.CrossFadeAlpha(1f, 0.5f, false);

        yield return new WaitWhile(() => videoPlayer.isPlaying);


        if (screenManager.isShowSummary) {
            screenManager.ChangeScreen("summary");
        }
        else {
            screenManager.ChangeScreen("start");
        }
    }
}
