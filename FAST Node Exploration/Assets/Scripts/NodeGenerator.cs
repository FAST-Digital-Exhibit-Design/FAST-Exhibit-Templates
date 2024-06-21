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
    public class NodeGenerator : MonoBehaviour
    {
        [SerializeField]
        private Node nodePrefab;

        [SerializeField]
        private RectTransform baseImageRectTransform;

        void Awake()
        {
            foreach (var item in FAST.Application.settings.nodeSettings.nodeDataGeo)
            {                
                var newNode = Instantiate(nodePrefab, transform);
                newNode.baseImage = baseImageRectTransform;
                newNode.coordinateDegrees.latitude = item.latitude;
                newNode.coordinateDegrees.longitude = item.longitude;
                newNode.name = item.name;
                newNode.pinSelectedImageFromFile.baseFileName = $"{item.name}-Pin-Selected.png";
                newNode.pinUnselectedImageFromFile.baseFileName = $"{item.name}-Pin.png";
                newNode.nameAudioFromFile.baseFileName = $"{item.name}-Title.wav";
                newNode.gameObject.SetActive(true);
            }
            foreach (var item in FAST.Application.settings.nodeSettings.nodeDataPixel)
            {
                var newMode = Instantiate(nodePrefab, transform);
                newMode.baseImage = baseImageRectTransform;
                newMode.coordinateDegrees = new CoordinateDegrees(0f, 0f);
                newMode._2dRect.anchorMin = Vector2.zero;
                newMode._2dRect.anchorMax = Vector2.zero;
                newMode._2dRect.pivot = Vector2.zero;
                newMode._2dRect.anchoredPosition = new Vector2(item.x, item.y);
                newMode.name = item.name;
                newMode.pinSelectedImageFromFile.baseFileName = $"{item.name}-Pin-Selected.png";
                newMode.pinUnselectedImageFromFile.baseFileName = $"{item.name}-Pin.png";
                newMode.nameAudioFromFile.baseFileName = $"{item.name}-Title.wav";
                newMode.gameObject.SetActive(true);
            }
            Wrj.Utils.DeferredExecution(1f, () =>
            {
                FindObjectOfType<Selector>().ResetSelection();
            });
        }
    }
}
