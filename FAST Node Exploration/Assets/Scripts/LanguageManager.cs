//=============================================================================
// FAST Node Exploration
//
// A FAST digital exhibit experience to explore nodes in a network. A node is
// selected by turning a rotary dial. The content for the selected node is
// displayed in a panel and animated in a video. 
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
using UnityEngine;
using UnityEngine.Events;
using FAST;

namespace NodeExploration
{
    public class LanguageManager : Singleton<LanguageManager>
    {
        [SerializeField]
        private float buttonDebounce = 3f;
        [SerializeField]
        private AudioPlayer narrationAudioPlayer;
        [SerializeField]
        private AudioClipFromFile languageNarrationClip;
        [SerializeField]
        private CanvasGroupFade languageCanvas;
        private Coroutine languageRoutine;

        public static UnityAction OnLanguageNarrationStart;
        public static UnityAction OnLanguageNarrationDone;
        private static float lastLanguageTime = 0f;

        private void Start()
        {
            lastLanguageTime = Time.time;
        }
        public void SelectedNodeChanged()
        {
            StopNarrateLanguageRoutine();
            narrationAudioPlayer.Stop();
            languageCanvas.IsVisible = false;
        }
        public void NextLanguage() 
        {
            if (Time.time - lastLanguageTime < _instance.buttonDebounce) return;
            lastLanguageTime = Time.time;
            FAST.Application.ChangeLanguage(FAST.Application.ChangeLanguageMode.Next);
            StopNarrateLanguageRoutine();
            languageRoutine = StartCoroutine(NarrateLanguageRoutine());
        }
        private IEnumerator NarrateLanguageRoutine()
        {
            OnLanguageNarrationStart?.Invoke();
            languageCanvas.IsVisible = true;

            narrationAudioPlayer.Play(new[] { languageNarrationClip.audioClip });
            yield return new WaitWhile(() => narrationAudioPlayer.IsRunning);

            languageCanvas.IsVisible = false;
            OnLanguageNarrationDone?.Invoke();
        }
        private void StopNarrateLanguageRoutine()
        {
            if (languageRoutine != null) {
                StopCoroutine(NarrateLanguageRoutine());
                languageRoutine = null;
            }
        }
    }
}