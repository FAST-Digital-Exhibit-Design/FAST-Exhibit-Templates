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

public class HintScreen : ActivityScreen
{
    [Header("Images")]
    [SerializeField]
    private Image backgroundImage;
    [SerializeField]
    private Image incorrectImage;
    [SerializeField]
    private Image questionImage;
    [SerializeField]
    private Image thumbnailImage;
    [SerializeField]
    private Image observeImage;
    [SerializeField]
    private Image[] infoImages = new Image[3];
    [SerializeField]
    private Image promptImage;


    private ImageFromFile questionImageFromFile;
    private ImageFromFile thumbnailImageFromFile;
    private ImageFromFile[] infoImagesFromFile = new ImageFromFile[3];

    override protected void Awake()
    {
        base.Awake();
        questionImageFromFile = questionImage.GetComponent<ImageFromFile>();
        thumbnailImageFromFile = thumbnailImage.GetComponent<ImageFromFile>();
        for (int i = 0; i < infoImages.Length; i++) {
            infoImagesFromFile[i] = infoImages[i].GetComponent<ImageFromFile>();
        }
    }

    override protected void OnEnable()
    {
        screenManager.isShowSummary = false;
        PlayScreen();
    }

    override public void OnScanStart()
    {
        promptImage.enabled = false;
    }

    override public void OnScanDone()
    {
        promptImage.enabled = true;
    }

    override protected IEnumerator PlayScreenAnimation()
    {
        // Load and initialize
        audioLUT["Hint-Incorrect-2-Narration.mp3"].baseFileName = string.Format("Hint-Incorrect-2-{0}-Narration.mp3",
            screenManager.teaserName);
        audioLUT["Hint-Incorrect-2-Narration.mp3"].Load(FAST.Application.language);

        for (int i = 0; i < infoImages.Length; i++) {
            AudioClipFromFile audioClipFromFile = audioLUT[$"Feature-{i + 1}-Narration.mp3"];
            audioClipFromFile.baseFileName = string.Format("Feature-{0}-{1}-Narration.mp3",
                i + 1, screenManager.teaserName);
            if (!audioClipFromFile.IsAssetAvailable(FAST.Application.language)) {
                audioClipFromFile.baseFileName = "";
            }
            audioClipFromFile.Load(FAST.Application.language);
            if (audioClipFromFile.audioClip == null) {
                audioClipFromFile.audioClip = AudioClipExtensions.CreatePauseClip(0.01f);
            }
        }

        backgroundImage.CrossFadeAlpha(0f, 0f, false);

        thumbnailImageFromFile.baseFileName = string.Format("Hint-Image-{0}.png", screenManager.teaserName);
        thumbnailImageFromFile.Load(FAST.Application.language);
        thumbnailImage.CrossFadeAlpha(0f, 0f, false);

        incorrectImage.CrossFadeAlpha(0f, 0f, false);

        questionImageFromFile.baseFileName = string.Format("Hint-Reminder-{0}.png", screenManager.teaserName);
        questionImageFromFile.Load(FAST.Application.language);
        questionImage.CrossFadeAlpha(0f, 0f, false);

        observeImage.CrossFadeAlpha(0f, 0f, false);

        for (int i = 0; i < infoImages.Length; i++) {
            infoImagesFromFile[i].baseFileName = $"Hint-Feature-{i + 1}-{screenManager.teaserName}.png";
            if (!infoImagesFromFile[i].IsAssetAvailable(FAST.Application.language)) {
                infoImagesFromFile[i].baseFileName = "";
            }
            infoImagesFromFile[i].Load(FAST.Application.language);
            infoImages[i].CrossFadeAlpha(0f, 0f, false);
        }
        promptImage.enabled = true;
        promptImage.CrossFadeAlpha(0f, 0f, false);

        // Animate
        yield return null;

        yield return new WaitForSecondsRealtime(0.25f);

        backgroundImage.CrossFadeAlpha(1f, 0.5f, false);
        incorrectImage.CrossFadeAlpha(1f, 0.5f, false);
        audioPlayer.Play(new AudioClip[] { audioLUT["Hint-Incorrect-SoundEffect.mp3"].audioClip, audioLUT["Hint-Incorrect-1-Narration.mp3"].audioClip });
        yield return new WaitWhile(() => audioPlayer.IsRunning);

        questionImage.CrossFadeAlpha(1f, 0.5f, false);
        thumbnailImage.CrossFadeAlpha(1f, 0.5f, false);
        audioPlayer.Play(new AudioClip[] { audioLUT["Hint-Incorrect-2-Narration.mp3"].audioClip });
        yield return new WaitWhile(() => audioPlayer.IsRunning);

        observeImage.CrossFadeAlpha(1f, 0.5f, false);
        audioPlayer.Play(new AudioClip[] { audioLUT["Hint-Incorrect-3-Narration.mp3"].audioClip });
        yield return new WaitWhile(() => audioPlayer.IsRunning);

        for (int i = 0; i < infoImages.Length; i++) {
            infoImages[i].CrossFadeAlpha(1f, 0.5f, false);

            audioPlayer.Play(new AudioClip[] { audioLUT[$"Feature-{i + 1}-Narration.mp3"].audioClip });
            yield return new WaitWhile(() => audioPlayer.IsRunning);
        }

        promptImage.CrossFadeAlpha(1f, 0.5f, false);
        audioPlayer.Play(new AudioClip[] { audioLUT["Hint-Incorrect-4-Narration.mp3"].audioClip });
        yield return new WaitWhile(() => audioPlayer.IsRunning);
    }
}
