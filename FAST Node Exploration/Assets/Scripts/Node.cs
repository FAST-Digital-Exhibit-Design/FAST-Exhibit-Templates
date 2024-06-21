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

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FAST;

namespace NodeExploration
{
    public class Node : MonoBehaviour
    {
        // public float flipCutoff = 790f;
        public RectTransform _2dRect;
        [NaughtyAttributes.OnValueChanged("Place")]
        public CoordinateDegrees coordinateDegrees;
        public RectTransform baseImage;
        public ImageFromFile pinSelectedImageFromFile;
        public ImageFromFile pinUnselectedImageFromFile;
        public AudioClipFromFile nameAudioFromFile;
        private float xMin => 0f;
        private float xMax => Mathf.Abs(baseImage.rect.width);
        private float yMin => Mathf.Abs(baseImage.rect.height) * -1f;
        private float yMax => 0f; 

        private static List<Node> _nodes;
        private static Node _selected = null;
        public static Node[] Nodes => _nodes.ToArray();
        public static Node Selected => _selected;

        

        private void Awake() {
            Unselect();
            if (_nodes == null) _nodes = new List<Node>();
            _nodes.Add(this);
        }
        void Start()
        {
            Place();
            Unselect();
        }

        public void Select()
        {
            if (_selected != null) _selected.Unselect();
            _selected = this;
            transform.SetAsLastSibling();
            NodeInfoImage.Instance.SetSprite($"{name}-Info.png");
            pinSelectedImageFromFile.gameObject.SetActive(true);
            pinUnselectedImageFromFile.gameObject.SetActive(false);
        }
        public void Unselect()
        {
            pinSelectedImageFromFile.gameObject.SetActive(false);
            pinUnselectedImageFromFile.gameObject.SetActive(true);
            _selected = null;
        }
        void Place()
        {
            if (coordinateDegrees.latitude == 0f && coordinateDegrees.longitude == 0f) return;
            _2dRect.anchorMin = _2dRect.anchorMax = _2dRect.pivot = new Vector2(0f, 1f);
            Coordinate coord = coordinateDegrees.ConvertToRadians();
            var uv = coord.ToUV();
            var uvRemap = new Vector2(Mathf.Lerp(xMin, xMax, uv.x), Mathf.Lerp(yMin, yMax, uv.y));
            _2dRect.anchoredPosition = uvRemap;
        }

        private void OnDestroy()
        {
            _nodes.Remove(this);
        }
    }
}