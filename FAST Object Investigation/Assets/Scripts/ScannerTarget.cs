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
using FAST;

public class ScannerTarget : MonoBehaviour
{
    [SerializeField]
    private ActivityScreenManager activityManager;
    [SerializeField]
    private MarkerTrackingSystem trackingSystem;
    [SerializeField]
    private bool isDrawTarget;
    [SerializeField]
    private RectTransform targetRectTransform;
    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Vector2 targetCenter;
    [SerializeField]
    private float targetRadius;
    [SerializeField]
    private int currentObjectIndex = 0;
    [SerializeField]
    private int lastObjectIndex = 0;

    [SerializeField]
    private MarkerObject[] markerObjects;

    private bool isAnyIntersections = false;

    private void Start()
    {
        markerObjects = FindObjectsByType<MarkerObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }
    void Update()
    {
        canvasGroup.alpha = isDrawTarget ? 1f : 0f;

        targetCenter = targetRectTransform.localPosition;
        targetRadius = targetRectTransform.sizeDelta.x;

        isAnyIntersections = false;

        for (int i = 0; i < markerObjects.Length; i++) {
            // Ignore objects that aren't being tracked
            if (!markerObjects[i].isTracked) {
                continue;
            }

            // Check each object to see if it is on the pedestal
            foreach (MarkerData objectMarker in markerObjects[i].objectMarkers) {
                // Ignore markers that aren't being tracked
                if (objectMarker.trackingState.Equals(MarkerData.TrackingState.NotTracked)) {
                    continue;
                }
                Vector2 markerPosition = new Vector2(objectMarker.x * trackingSystem.Width, -objectMarker.y * trackingSystem.Height);
                
                // A marker is on the pedestal if it's within a circular region
                float distance = Vector2.Distance(targetCenter, markerPosition);
                //Debug.LogFormat($"[{i}] marker position = ({markerPosition.x}, {markerPosition.y}) | distance = {distance} ");
                bool isIntersect = distance < targetRadius;
                isAnyIntersections |= isIntersect;

                // Multiple markers may be on the pedestal, but only the first one seen after being empty will be scanned
                if (isIntersect && currentObjectIndex == 0) {
                    lastObjectIndex = currentObjectIndex;
                    currentObjectIndex = markerObjects[i].value;
                    break;
                }
            }
        }

        // If there were not intersections, then the pedestal is empty
        if (!isAnyIntersections) {
            lastObjectIndex = currentObjectIndex;
            currentObjectIndex = 0;
        }

        // Only scan if the object on the pedestal is different from the last scan
        if (currentObjectIndex != lastObjectIndex) {
            activityManager.OnObjectChanged(currentObjectIndex);
            lastObjectIndex = currentObjectIndex;
        }
    }
    public void OnToggleUI()
    {
        isDrawTarget = !isDrawTarget;
    }
}
