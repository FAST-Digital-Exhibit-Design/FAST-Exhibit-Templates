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

public class SummaryScreen : ActivityScreen
{
    [Header("Images")]
    [SerializeField]
    private Image observeImage;
    [SerializeField]
    private Image objectImage;
    [SerializeField]
    private Image[] infoImages = new Image[3];

    private ImageFromFile objectImageFromFile;
    private ImageFromFile[] infoImagesFromFile = new ImageFromFile[3];

    override protected void Awake()
    {
        base.Awake();
        objectImageFromFile = objectImage.GetComponent<ImageFromFile>();
        for (int i = 0; i < infoImages.Length; i++) {
            infoImagesFromFile[i] = infoImages[i].GetComponent<ImageFromFile>();
        }
    }

    override protected IEnumerator PlayScreenAnimation()
    {
        // Load and initialize
        for (int i = 0; i < infoImages.Length; i++) {
            audioLUT[$"Feature-{i+1}-Narration.mp3"].baseFileName = string.Format("Feature-{0}-{1}-Narration.mp3",
                i+1, screenManager.teaserName);
            audioLUT[$"Feature-{i+1}-Narration.mp3"].Load(FAST.Application.language);
            if (audioLUT[$"Feature-{i+1}-Narration.mp3"].audioClip == null) {
                audioLUT[$"Feature-{i + 1}-Narration.mp3"].audioClip = AudioClipExtensions.CreatePauseClip(0.01f);
            }
        }

        observeImage.CrossFadeAlpha(0f, 0f, false);

        objectImageFromFile.baseFileName = string.Format("Summary-Image-{0}.png", screenManager.teaserName);
        objectImageFromFile.Load(FAST.Application.language);
        objectImage.CrossFadeAlpha(0f, 0f, false);

        for (int i = 0; i < infoImages.Length; i++) {
            infoImagesFromFile[i].baseFileName = $"Summary-Feature-{i+1}-{screenManager.teaserName}.png";
            infoImagesFromFile[i].Load(FAST.Application.language);
            infoImages[i].CrossFadeAlpha(0f, 0f, false);
        }

        // Animate
        yield return null;

        observeImage.CrossFadeAlpha(1f, 0.5f, false);
        objectImage.CrossFadeAlpha(1f, 0.5f, false);
        audioPlayer.Play(new AudioClip[] { audioLUT["Summary-Observe-Narration.mp3"].audioClip });
        yield return new WaitWhile(() => audioPlayer.IsRunning);

        for (int i = 0; i < infoImages.Length; i++) {
            infoImages[i].CrossFadeAlpha(1f, 0.5f, false);

            audioPlayer.Play(new AudioClip[] { audioLUT[$"Feature-{i+1}-Narration.mp3"].audioClip });
            yield return new WaitWhile(() => audioPlayer.IsRunning);
        }

        yield return new WaitForSecondsRealtime(1f);

        screenManager.ChangeScreen("start");
    }
}
