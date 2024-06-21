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

using FAST;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

[System.Serializable]
public class QuizPlayer
{

    public string Name;

    public TextMeshProUGUI Score;

    #region Objects
    public GameObject PlayerPlacemat;

    public GameObject MediaPlacemat;

    public GameObject PlayerJoinName;

    public GameObject PressToJoin;
    public GameObject PressToStart;

    public GameObject ScoreImage;
    public GameObject CorrectImage;
    public GameObject IncorrectImage;

    public GameObject ButtonBox;

    public GameObject[] Buttons;

    public GameObject MiddleButtonBlocker;

    public ImageFromFile QuestionArea;

    public ImageFromFile FeedbackArea;

    public GameObject[] AnswerBackgrounds;
    public ImageFromFile[] AnswerOptions;
    public ImageFromFile[] ImageAnswerOptions;

    public ImageFromFile[] AnswerSelections;
    public ImageFromFile[] ImageAnswerSelections;

    public ImageFromFile AnswerArea;

    public VideoPlayerFromFile VideoArea;

    [SerializeField]
    private GameObject _videoPlayerImage;

    public ImageFromFile VideoIntroArea;

    public ImageFromFile ImageIntroArea;

    public AudioSource ButtonSFX;
    #endregion

    [HideInInspector]
    public bool PlayerAnswered;

    [HideInInspector]
    public bool PlayerCorrect = false;

    [HideInInspector]
    public bool PlayerActive = false;

    [HideInInspector]
    public List<int> CurrentlySelectedAnswer;

    public int CurrentScore;

    private List<int> _availableAnswers;

    public void ResetScore()
    {
        CurrentScore = 0;
        Score.text = CurrentScore.ToString();
        Score.gameObject.SetActive(false);
    }

    public void TurnPlayerOnOff(bool on)
    {
        PlayerPlacemat.SetActive(on);
        ScoreImage.SetActive(on);
    }

    public void ShowIntro(bool show, int index, bool isVideo)
    {
        if (show)
        {
            QuizSettings settings = FAST.Application.settings.quizSettings;
            if (isVideo)
            {
                VideoIntroArea.baseFileName = settings.questionData[index].name + "-Intro-Image.png";
                VideoIntroArea.Load(FAST.Application.language);
            }
            else
            {
                ImageIntroArea.baseFileName = settings.questionData[index].name + "-Intro-Image.png";
                ImageIntroArea.Load(FAST.Application.language);
            }
        }

        if (isVideo)
        {
            VideoIntroArea.gameObject.SetActive(show);
        }
        else
        {
            ImageIntroArea.gameObject.SetActive(show);
        }
    }

    public IEnumerator PlayVideo(int index, bool mute)
    {
        VideoArea.gameObject.SetActive(true);
        VideoArea.GetComponent<VideoPlayer>().SetDirectAudioMute(0, mute);

        QuizSettings settings = FAST.Application.settings.quizSettings;
        VideoArea.baseFileName = settings.questionData[index].name + "-Video.mp4";
        VideoArea.Load(FAST.Application.language);

        VideoArea.GetComponent<VideoPlayer>().Prepare();
        yield return new WaitUntil(() => VideoArea.GetComponent<VideoPlayer>().isPrepared);

        VideoArea.GetComponent<VideoPlayer>().Play();
        _videoPlayerImage.SetActive(true);
        yield return new WaitWhile(() => VideoArea.GetComponent<VideoPlayer>().isPlaying);
        _videoPlayerImage.SetActive(false);
        VideoArea.gameObject.SetActive(false);
    }

    public void ShowQuestion(bool show, int index)
    {
        CurrentlySelectedAnswer = new List<int> { };
        PlayerAnswered = false;
        PlayerCorrect = false;

        QuizSettings settings = FAST.Application.settings.quizSettings;
        //show question
        if (show)
        {
            QuestionArea.baseFileName = settings.questionData[index].name + "-Image.png";
            QuestionArea.Load(FAST.Application.language);
        }
        QuestionArea.gameObject.SetActive(show);

        //show answer options
        for (int i = 0; i < settings.questionData[index].options.Length; i++)
        {
            int slot = settings.questionData[index].options[i].slot - 1;
            if (show)
            {
                if (settings.questionData[index].type == "image")
                {
                    ImageAnswerOptions[slot].baseFileName = settings.questionData[index].name
                         + "-" + settings.questionData[index].options[i].name + "-Image.png";
                    ImageAnswerOptions[slot].Load(FAST.Application.language);
                    ImageAnswerOptions[slot].gameObject.SetActive(true);
                }
                else
                {
                    AnswerOptions[slot].baseFileName = settings.questionData[index].name
                        + "-" + settings.questionData[index].options[i].name + "-Image.png";
                    AnswerOptions[slot].Load(FAST.Application.language);
                    AnswerOptions[slot].gameObject.SetActive(true);
                }
            }
            else
            {
                ImageAnswerOptions[slot].gameObject.SetActive(false);
                AnswerOptions[slot].gameObject.SetActive(false);
            }

            AnswerBackgrounds[slot].SetActive(show);
        }

        //determine the number of available answers (either 2 or 4, dependent on if true/false question)
        if (show)
        {
            _availableAnswers = new List<int>();
            _availableAnswers = settings.questionData[index].options.Select(x => (x.slot - 1)).ToList();
        }

        // Initializing answer area
        AnswerArea.baseFileName = settings.questionData[index].name + "-AnswerReveal-Image.png";
        AnswerArea.Load(FAST.Application.language);
        AnswerArea.gameObject.SetActive(false);

        //initializing feedback area
        FeedbackArea.baseFileName = settings.questionData[index].name + "-Feedback-Image.png";
        FeedbackArea.Load(FAST.Application.language);
        FeedbackArea.gameObject.SetActive(false);

    }

    public void ShowEndState(bool win)
    {
        QuestionArea.baseFileName = win ? "End-PlayerWin-Image.png" : "End-PlayerLose-Image.png";
        QuestionArea.Load(FAST.Application.language);
        QuestionArea.gameObject.SetActive(true);
    }

    public void TurnOffForEndState()
    {
        FeedbackArea.gameObject.SetActive(false);
        AnswerArea.gameObject.SetActive(false);
        foreach(GameObject go in AnswerBackgrounds)
        {
            go.SetActive(false);
        }
        foreach(ImageFromFile image in AnswerOptions)
        {
            image.gameObject.SetActive(false);
        }
        foreach (ImageFromFile image in ImageAnswerOptions)
        {
            image.gameObject.SetActive(false);
        }
        foreach (ImageFromFile image in ImageAnswerSelections)
        {
            image.gameObject.SetActive(false);
        }
        foreach (ImageFromFile image in AnswerSelections)
        {
            image.gameObject.SetActive(false);
        }
    }

    public void SetAnswer(int answerIndex, int numOfAnswers, bool isImage)
    {
        //if its a true or false question and buttons 2 or 3 are pressed, ignore
        if (!_availableAnswers.Contains(answerIndex))
        {
            return;
        }

        //turn on relevant gameobjects to show answer selection
        Buttons[answerIndex].SetActive(true);
        ButtonSFX.Play();

        if (isImage)
        {
            ImageAnswerSelections[answerIndex].gameObject.SetActive(true);
        }
        else
        {
            AnswerSelections[answerIndex].gameObject.SetActive(true);
        }

        //if the player has already answered with a different response, turn previous selection off
        if (PlayerAnswered)
        {
            //if this is a mutiple-answer question
            if (numOfAnswers > 1)
            {
                if (CurrentlySelectedAnswer.Contains(answerIndex))
                {
                    RemoveAnswer(answerIndex, isImage);
                }
                else
                {
                    CurrentlySelectedAnswer.Add(answerIndex);
                }
            }
            //if this is a single-answer question and the new answer is different
            else if (!CurrentlySelectedAnswer.Contains(answerIndex))
            {
                ChangeAnswer(answerIndex, isImage);
            }
        }
        else //player has not answered yet
        {
            CurrentlySelectedAnswer.Add(answerIndex);
            PlayerAnswered = true;
        }
    }

    //deselect old answer
    private void RemoveAnswer(int oldAnswer, bool isImage)
    {
        Buttons[oldAnswer].SetActive(false);
        if (isImage)
        {
            ImageAnswerSelections[oldAnswer].gameObject.SetActive(false);
        }
        else
        {
            AnswerSelections[oldAnswer].gameObject.SetActive(false);
        }
        CurrentlySelectedAnswer.Remove(oldAnswer);
    }

    private void ChangeAnswer(int newAnswer, bool isImage)
    {
        Buttons[CurrentlySelectedAnswer[0]].SetActive(false);
        if (isImage)
        {
            ImageAnswerSelections[CurrentlySelectedAnswer[0]].gameObject.SetActive(false);
        }
        else
        { 
            AnswerSelections[CurrentlySelectedAnswer[0]].gameObject.SetActive(false); 
        }
        
        CurrentlySelectedAnswer = new List<int> { newAnswer };
    }

    public void ShowCorrectOrIncorrect(bool show)
    {
        if (show)
        {
            if (PlayerCorrect)
            {
                CorrectImage.SetActive(true);
            }
            else
            {
                IncorrectImage.SetActive(true);
            }
        }
        else
        {
            CorrectImage.SetActive(false);
            IncorrectImage.SetActive(false);
        }
    }

    public void ShowScore()
    {
        if (PlayerCorrect)
        {
            Debug.Log(Name + " WAS CORRECT, INCREASE SCORE BY ONE");
            CurrentScore++;
        }
        Score.text = CurrentScore.ToString("00");
        if (CurrentScore > 0)
        {
            Score.gameObject.SetActive(true);
        }
    }
}