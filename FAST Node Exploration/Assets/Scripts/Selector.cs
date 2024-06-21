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
using FAST;
using UnityEngine.Video;
using UnityEngine.Events;

namespace NodeExploration
{
    public class Selector : MonoBehaviour
    {
        public int defaultNodeIndex = 0;
        public int eventsPerSecond = 8;
        [Range(.25f, 3f)]
        public float waitDuration = 1f;
        public AudioSourceFromFile clickSource;
        public AudioPlayer narrationAudioPlayer;
        public CanvasGroupFade storyCanvas;
        public VideoPlayerFromFile storyPlayerFromFile;
        public VideoPlayer storyPlayer;
        public UnityEvent OnSelectionChanged;

        private Coroutine movingRoutine;
        private Coroutine resetRoutine;
        private AudioSource clickAudioSource;
        private bool _debounce;

        private ActivitySettings settings;


        private void Start()
        {
            settings = FAST.Application.settings as ActivitySettings;
            eventsPerSecond = settings.dialEventsPerSecond;
            clickAudioSource = clickSource.GetComponent<AudioSource>();

            LanguageManager.OnLanguageNarrationStart += () => {
                LanguageNarrationStart();
            };

            LanguageManager.OnLanguageNarrationDone += () => { 
                LanguageNarrationDone();
            };

            if (FAST.Application.settings.serialConnectionSettings.Count > 0)
            {
                SerialConnection rotarySerial = FAST.Application.serialConnections[0];
                rotarySerial.onDataReceivedEvent += OnSerialEvent;
            }
        }

        private void OnSerialEvent(string data)
        {
            if (data == "+")
            {
                Next();
            }
            else if (data == "-")
            {
                Previous();
            }
        }

        private int _selectedCached = -1;

        public void ResetSelection()
        {
            if (resetRoutine != null) {
                StopCoroutine(ResetSelectionRoutine());
                resetRoutine = null;
            }
            
            resetRoutine = StartCoroutine(ResetSelectionRoutine());
        }

        private IEnumerator ResetSelectionRoutine()
        {
            Node.Nodes[0].Select();
            _selectedCached = defaultNodeIndex;

            storyPlayerFromFile.baseFileName = $"{Node.Nodes[0].name}-Story.mp4";
            storyPlayerFromFile.Load(FAST.Application.language);

            storyPlayer.Prepare();
            while (!storyPlayer.isPrepared) {
                yield return null;
            }

            storyPlayer.Play();
            storyPlayer.frame = (long)(storyPlayer.frameCount);
            storyCanvas.IsVisible = true;
        }

        private void DialEventRateLimiter()
        {
            _debounce = true;
            Wrj.Utils.DeferredExecution(1f / eventsPerSecond, () => _debounce = false);
        }

        public void SetSelected(float input)
        {
            if (_debounce) return;
            var tickers = Node.Nodes;
            input = Mathf.Clamp01(input);
            int index = Mathf.RoundToInt(Mathf.Lerp(0, tickers.Length - 1, input));
            if (index != _selectedCached)
            {
                _selectedCached = index;
                clickAudioSource.PlayOneShot(clickAudioSource.clip);
                tickers[index].Select();
                StartWait();
                DialEventRateLimiter();
            }
        }

        public void Next()
        {
            if (_debounce) return;
            int index = _selectedCached + 1;
            var tickers = Node.Nodes;
            index = index % tickers.Length;
            _selectedCached = index;
            clickAudioSource.PlayOneShot(clickAudioSource.clip);
            tickers[index].Select();
            StartWait();
            DialEventRateLimiter();
            OnSelectionChanged.Invoke();
        }

        public void Previous()
        {
            if (_debounce) return;
            int index = _selectedCached - 1;
            var tickers = Node.Nodes;
            if (index < 0) index = tickers.Length - 1;
            index = index % tickers.Length;
            _selectedCached = index;
            clickAudioSource.PlayOneShot(clickAudioSource.clip);
            tickers[index].Select();
            StartWait();
            DialEventRateLimiter();
            OnSelectionChanged.Invoke();
        }

        private void StopWait()
        {
            if (movingRoutine != null) {
                StopCoroutine(movingRoutine);
                movingRoutine = null;
            }
        }

        private void StartWait()
        {
            StopWait();
            movingRoutine = StartCoroutine(MovingRoutine());
        }

        private void LanguageNarrationStart()
        {
            StopWait();
            narrationAudioPlayer.Stop();
            storyPlayer.Stop();
        }

        private void LanguageNarrationDone()
        {
            StartWait();
            Node.Nodes[_selectedCached].Select();
        }

        public IEnumerator MovingRoutine()
        {
            storyCanvas.IsVisible = false;
            storyPlayer.Stop();
            yield return new WaitForSecondsRealtime(waitDuration);

            // Narrate the node name
            narrationAudioPlayer.Play(new[]{ Node.Selected.nameAudioFromFile.audioClip });

            // Animate or narrate the node content
            storyPlayerFromFile.baseFileName = $"{Node.Selected.name}-Story.mp4";
            storyPlayerFromFile.Load(FAST.Application.language);

            storyPlayer.Prepare();
            while (!storyPlayer.isPrepared) {
                yield return null;
            }

            yield return new WaitWhile(() => narrationAudioPlayer.IsRunning);

            movingRoutine = null;

            storyCanvas.IsVisible = true;
            storyPlayer.Play();
            while (storyPlayer.isPlaying)
            {
                yield return null;
            }
		}
    }
}