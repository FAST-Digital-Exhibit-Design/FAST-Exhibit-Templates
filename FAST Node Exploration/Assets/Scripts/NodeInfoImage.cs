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

namespace NodeExploration
{
    public class NodeInfoImage : SingletonWithInstance<NodeInfoImage>
    {
        [SerializeField]
        ImageFromFile imageFromFile;
        [SerializeField]
        CanvasGroupFade group;
        private Coroutine coroutine;
        public void SetSprite(string baseFileName)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(SetOverTime(baseFileName));
        }
        private IEnumerator SetOverTime(string baseFileName)
        {
            group.IsVisible = false;
            yield return new WaitForSeconds(group.OutDuration + group.OutDelay);
            imageFromFile.baseFileName = baseFileName;
            imageFromFile.Load(FAST.Application.language);
            yield return null;
            group.IsVisible = true;
            yield return new WaitForSeconds(group.InDuration + group.InDelay);
            coroutine = null;
        }
    }
}