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
using UnityEngine.Events;
using FAST;

namespace NodeExploration
{
    public class SerialEventBehavior : MonoBehaviour
    {
        [SerializeField]
        OnStringEvent[] onStringRecieved;
        void Start()
        {
            SerialConnection rotarySerial = FAST.Application.settings.serialConnectionSettings[0];
            rotarySerial.onDataReceivedEvent += OnSerialEvent;
        }

        void OnSerialEvent(string data)
        {
            foreach (var item in onStringRecieved)
            {
                if (data == item.value)
                {
                    item.command.Invoke();
                    return;
                }
            }
        }
        [System.Serializable]
        class OnStringEvent
        {
            public string value;
            public UnityEvent command;
        }
    }
}