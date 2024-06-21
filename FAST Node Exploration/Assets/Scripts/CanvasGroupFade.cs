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

using UnityEngine;
using FAST;

namespace NodeExploration
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupFade : MonoBehaviour
    {
        public float InDuration = .5f;
        public float InDelay = 0f;
        public float OutDuration = .25f;
        public float OutDelay = 0f;

        CanvasGroup _canvasGroupRef;
        private Wrj.Utils.MapToCurve _map = Wrj.Utils.MapToCurve.Ease;
        private Wrj.Utils.MapToCurve.Manipulation _procedure;
        private bool _isVisible => canvasGroup.alpha > .5f;
        public bool IsVisible
        {
            set
            {
                if (value == _isVisible) return;

                canvasGroup.interactable = value;
                canvasGroup.blocksRaycasts = value;
                float initVal = canvasGroup.alpha;
                float target = value ? 1f : 0f;
                float duration = value ? InDuration : OutDuration;
                float delay = value ? InDelay : OutDelay;
                if (_procedure != null)
                {
                    _procedure.Stop();
                }
                _procedure = _map.ManipulateFloat(
                    (f) => canvasGroup.alpha = f,
                    initVal,
                    target,
                    duration
                );
            }
            get => _isVisible;
        }
        public void Toggle() => IsVisible = !IsVisible;
        private CanvasGroup canvasGroup
        {
            get 
            {
                if (_canvasGroupRef == null)
                {
                    _canvasGroupRef = GetComponent<CanvasGroup>();
                }
                return _canvasGroupRef;
            }
        }
    }
}