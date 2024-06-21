//=============================================================================
// FAST Quiz Show
//
// A FAST digital exhibit experience that allows for multiplayer
// participation, in-depth exploration of a topic, multiple-choice questions,
// and true/false questions.
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
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using FAST;
using TMPro;


//class for managing progress bar and quiz answers
public class ProgressManager : MonoBehaviour
{
    //indicates if at least one player correctly answered question
    [HideInInspector]
    public bool AtLeastOneCorrect = false;

    [SerializeField]
    private int _baselineWinTotal = 5;
    [SerializeField]
    private static int _dynamicWinTotal;
    [SerializeField]
    private static int _halfWayPoint;
    [SerializeField]
    private static int _totalScore;
    [SerializeField]
    public static int Progress;
    [SerializeField]
    private float _normalizedProgress;
    [SerializeField]
    private float _currentProgressShown;

    [SerializeField]
    private Image progressBarFill;
    [SerializeField]
    private Image progressBarGlow;
    [SerializeField]
    private TextMeshProUGUI _progressText;

    public static bool PastMidpoint = false;

    public static bool IsAtHalfwayPoint
    {
        get
        {
            return _totalScore >= _halfWayPoint && !PastMidpoint;
        }
    }

    public static bool AtWin
    {
        get
        {
            return _totalScore >= _dynamicWinTotal;
        }
    }

    public void DeterminePointsNeededToWin(int activePlayers)
    {
        _progressText.text = "0%";
        _dynamicWinTotal = _baselineWinTotal * activePlayers;
        _halfWayPoint = Mathf.RoundToInt(_dynamicWinTotal / 2);

        Debug.Log("DYNAMIC WIN TOTAL IS " + _dynamicWinTotal + ", HALFWAY POINT IS " + _halfWayPoint);
    }

    public void ResetGame()
    {
        GetComponent<Animator>().Play("Hide");
        _totalScore = 0;
        Progress = 0;
        PastMidpoint = false;
    }
    public IEnumerator ShowProgress(QuizPlayer[] activePlayers)
    {
        if (AtLeastOneCorrect) 
        {
            int prevProgress = Progress;

            List<int> scores = activePlayers.Select(p => p.CurrentScore).ToList();
            _totalScore = scores.Sum();
            Debug.Log("<b>TOTAL SCORE IS " + _totalScore + ", WIN STATE IS " + _dynamicWinTotal + "</b>");
            float percent = Mathf.Clamp01((float)_totalScore / (float)_dynamicWinTotal);
            percent *= 100;
            Progress = Mathf.RoundToInt(percent);
            _normalizedProgress = Progress * 0.01f;
            Debug.Log("<b>PROGRESS IS AT " + Progress + "</b>");

            yield return AnimateProgressBar(progressBarFill.fillAmount, _normalizedProgress, prevProgress, Progress, 1f);
        }
    }

    private IEnumerator AnimateProgressBar(float fillStart, float fillEnd, float textStart, float textEnd, float duration)
    {
        float elapsedTime = 0f;
        float percent = 0f;

        while (elapsedTime < duration) {
            progressBarFill.fillAmount = Mathf.Lerp(fillStart, fillEnd, percent);
            _currentProgressShown = Mathf.Lerp(textStart, textEnd, percent);
            _progressText.text = Mathf.RoundToInt(_currentProgressShown).ToString() + "%";
            elapsedTime += Time.deltaTime;
            percent = elapsedTime / duration;
            yield return null;
        }
        progressBarFill.fillAmount = _normalizedProgress;
        _progressText.text = Progress.ToString() + "%";
    }
}

