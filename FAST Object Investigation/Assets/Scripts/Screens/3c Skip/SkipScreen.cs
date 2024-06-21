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
using FAST;

public class SkipScreen : ActivityScreen
{
    [Header("Images")]
    [SerializeField]
    private Image backgroundImage;
    [SerializeField]
    private Image incorrectImage;
    [SerializeField]
    private Image promptImage;

    override protected IEnumerator PlayScreenAnimation()
    {
        // Load and initialize
        backgroundImage.CrossFadeAlpha(0f, 0f, false);
        incorrectImage.CrossFadeAlpha(0f, 0f, false);
        promptImage.CrossFadeAlpha(0f, 0f, false);

        // Animate
        yield return null;

        yield return new WaitForSecondsRealtime(0.25f);

        backgroundImage.CrossFadeAlpha(1f, 0.5f, false);
        incorrectImage.CrossFadeAlpha(1f, 0.5f, false);
        audioPlayer.Play(new AudioClip[] { audioLUT["Skip-Incorrect-SoundEffect.mp3"].audioClip, audioLUT["Skip-Incorrect-1-Narration.mp3"].audioClip });
        yield return new WaitWhile(() => audioPlayer.IsRunning);

        promptImage.CrossFadeAlpha(1f, 0.5f, false);
        audioPlayer.Play(new AudioClip[] { audioLUT["Skip-Incorrect-2-Narration.mp3"].audioClip });
        yield return new WaitWhile(() => audioPlayer.IsRunning);

        yield return new WaitForSecondsRealtime(0.25f);
        screenManager.ChangeScreen("start");
    }
}
