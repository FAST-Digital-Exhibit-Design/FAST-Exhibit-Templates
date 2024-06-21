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
using UnityEngine.Events;
using FAST;

public class ScannerManager : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public UnityEvent ScanStartEvent;
    public UnityEvent<int> ScanDoneEvent;
    private int objectIndex;

    private bool isTeaser = false;

    public void OnObjectChanged(int index, bool isScan)
    {
        objectIndex = index;

        if (!isScan) {
            if (objectIndex == 0) {
                OnScanDone();
            }
            return;
        }

        if (objectIndex == 0) {
            animator.SetTrigger("Clear");
        }
        else {
            if (isTeaser) {
                animator.SetTrigger("Scan Teaser");
            }
            else {
                animator.SetTrigger("Scan Other");
            }
        }
    }

    public void OnScanStart()
    {
        ScanStartEvent.Invoke();
    }

    public void OnScanDone()
    {
        ScanDoneEvent.Invoke(objectIndex);
        Debug.LogFormat("Object scanned: {0}", objectIndex);
    }

    public void OnChangeScanAudio(bool isTeaser)
    {
        this.isTeaser = isTeaser;
    }
}
