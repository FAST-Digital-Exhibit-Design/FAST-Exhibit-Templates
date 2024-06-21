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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class QuestionScreen : Screen
{
    [SerializeField]
    private ProgressManager _progressManager;
    private bool _answerPhase = false;


    public static int CurrentQuestionIndex
    {
        get
        {
            return _currentQuestionIndex;
        }
        set
        {
            _currentQuestionIndex = value;
        }
    }

    private static int _currentQuestionIndex = -1;

    private bool _currentQuestionIsImage = false;

    [SerializeField]
    private GameObject _videoPlayerImage;
    [SerializeField]
    private bool _allPlayersAnswered;

    public QuizPlayer[] ActivePlayers;

    private AnswerData[] _currentAnswerData;

    public override void KeyPressRecieved(int playerIndex, int answerIndex)
    {
        QuizPlayer currentPlayer = ScreenManager.QuizPlayers[playerIndex];

        //if answers can be submitted and the player is active
        if (_answerPhase && currentPlayer.PlayerActive)
        {
           currentPlayer.SetAnswer(answerIndex, _currentAnswerData.Length, _currentQuestionIsImage);
           _allPlayersAnswered = ActivePlayers.All(x => x.PlayerAnswered == true);
        }
    }

    protected override void OnEnable()
    {
        _answerPhase = false;
        ActivePlayers = ScreenManager.QuizPlayers.Where(x => x.PlayerActive == true).ToArray();
        QuizSettings settings = FAST.Application.settings.quizSettings;
        _currentQuestionIndex++;
        _currentAnswerData = settings.questionData[_currentQuestionIndex].answers;
        _currentQuestionIsImage = settings.questionData[_currentQuestionIndex].type == "image";
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        foreach (QuizPlayer player in ActivePlayers)
        {
            foreach (ImageFromFile selection in player.AnswerSelections)
            {
                selection.gameObject.SetActive(false);
            }
            foreach(ImageFromFile imageSelection in player.ImageAnswerSelections)
            {
                imageSelection.gameObject.SetActive(false);
            }
            player.IncorrectImage.SetActive(false);
            player.CorrectImage.SetActive(false);
            player.MediaPlacemat.SetActive(false);
        }
        base.OnDisable();
    }

    override protected IEnumerator PlayScreenAnimation()
    {
        // Initialize stuff
        foreach (QuizPlayer player in ScreenManager.QuizPlayers) {
            player.ShowQuestion(false, 0);
        }

        QuizSettings settings = FAST.Application.settings.quizSettings;
        audioLUT["Question-Number-Narration.mp3"].baseFileName = settings.questionData[_currentQuestionIndex].name + 
            "-Number-Narration.mp3";
        audioLUT["Question-Number-Narration.mp3"].Load(FAST.Application.language);

        audioLUT["Question-Narration.mp3"].baseFileName = settings.questionData[_currentQuestionIndex].name +
            "-Narration.mp3";
        audioLUT["Question-Narration.mp3"].Load(FAST.Application.language);

        audioLUT["Question-Options-Narration.mp3"].baseFileName = settings.questionData[_currentQuestionIndex].name +
            "-Options-Narration.mp3";
        audioLUT["Question-Options-Narration.mp3"].Load(FAST.Application.language);

        audioLUT["Answer-Narration.mp3"].baseFileName = settings.questionData[_currentQuestionIndex].name +
            "-Answer-Narration.mp3";
        audioLUT["Answer-Narration.mp3"].Load(FAST.Application.language);

        audioLUT["Feedback-Narration.mp3"].baseFileName = settings.questionData[_currentQuestionIndex].name +
            "-Feedback-Narration.mp3";
        audioLUT["Feedback-Narration.mp3"].Load(FAST.Application.language);

        if (settings.questionData[_currentQuestionIndex].type != "text")
        {
            audioLUT["Intro-Narration.mp3"].baseFileName = settings.questionData[_currentQuestionIndex].name +
                "-Intro-Narration.mp3";
            audioLUT["Intro-Narration.mp3"].Load(FAST.Application.language);
        }

        _videoPlayerImage.SetActive(false);
        _allPlayersAnswered = false;

        //show intro for image/video questions
        if (settings.questionData[_currentQuestionIndex].type != "text")
        {
            bool isVideo = settings.questionData[_currentQuestionIndex].type == "video";

            foreach (QuizPlayer player in ActivePlayers)
            {    
                if (isVideo)
                {
                    player.MediaPlacemat.SetActive(true);
                }
                player.ShowIntro(true, _currentQuestionIndex, isVideo);
            }
            
            audioPlayer.Play(new AudioClip[] { audioLUT["Intro-Narration.mp3"].audioClip });

            yield return new WaitWhile(() => audioPlayer.IsRunning);

            foreach (QuizPlayer player in ActivePlayers)
            {
                player.ShowIntro(false, _currentQuestionIndex, isVideo);
            }

            //if the wuestion type is video, play video
            if(settings.questionData[_currentQuestionIndex].type == "video")
            {
                //only one video should play audio, so start the concurrent "mute" videos
                for(int i = 0; i < ActivePlayers.Length; i++)
                {
                    bool mute = i > 0;
                    if (mute)
                    {
                        StartCoroutine(ActivePlayers[i].PlayVideo(_currentQuestionIndex, mute));
                    }
                }
                //and wait on the one playing audio (always the first active player)
                yield return ActivePlayers[0].PlayVideo(_currentQuestionIndex, false);
            }

            foreach (QuizPlayer player in ActivePlayers)
            {
                player.MediaPlacemat.SetActive(false);
            }
        }

        foreach (QuizPlayer player in ActivePlayers)
        {
            player.ShowQuestion(true, _currentQuestionIndex);
        }
        yield return null;

        // Play the audio
        audioPlayer.Play(new AudioClip[] {
            audioLUT["Question-Number-Narration.mp3"].audioClip,
            audioLUT["Question-Narration.mp3"].audioClip,
        });
        yield return new WaitWhile(() => audioPlayer.IsRunning);

        //play answer option audio
        _answerPhase = true;
        audioPlayer.Play(new AudioClip[] { audioLUT["Question-Options-Narration.mp3"].audioClip });
        foreach(QuizPlayer player in ActivePlayers)
        {
            player.ButtonBox.SetActive(true);
            if (settings.questionData[_currentQuestionIndex].options.Length < 4)
            {
                player.MiddleButtonBlocker.SetActive(true);
            }
        }
        yield return new WaitWhile(() => audioPlayer.IsRunning);

        //allow players to answer question
        //if all players answer, at end of narration, show timer
        //else wait for full timer
        yield return QuestionTimer();
        yield return AnswerTimer();

        // Narrates the answers
        foreach (QuizPlayer player in ActivePlayers) {
            player.AnswerArea.gameObject.SetActive(true);
            player.QuestionArea.gameObject.SetActive(false);
        }
        audioPlayer.Play(new AudioClip[] { audioLUT["Answer-Narration.mp3"].audioClip });
        yield return new WaitWhile(() => audioPlayer.IsRunning);

        //read out which players got the question correct, or if all were correct/incorrect
        QuizPlayer[] correctPlayers = ActivePlayers.Where(x => x.PlayerCorrect).ToArray();
        if(correctPlayers.Length > 0)
        {
            if(correctPlayers.Length == ActivePlayers.Length)
            {
                audioPlayer.Play(new AudioClip[] { audioLUT["AllCorrect-Narration.mp3"].audioClip,
                    audioLUT["Praise-Narration.mp3"].audioClip});
            }
            else
            {
                audioPlayer.ClearPlaylist();
                for(int i = 0; i < correctPlayers.Length; i++) 
                {
                    //if there is more than one player correct and we are on the last player
                    if(correctPlayers.Length > 1 && i == correctPlayers.Length - 1)
                    {
                        audioPlayer.AddToPlaylist(audioLUT["And-Narration.mp3"].audioClip);
                    }
                    audioPlayer.AddToPlaylist(audioLUT[correctPlayers[i].Name + "-Narration.mp3"].audioClip);
                }
                audioPlayer.AddToPlaylist(audioLUT["Correct-Narration.mp3"].audioClip);
                audioPlayer.AddToPlaylist(audioLUT["Praise-Narration.mp3"].audioClip);
                audioPlayer.Play();
            }
        }
        else
        {
            audioPlayer.Play(new AudioClip[] { audioLUT["Incorrect-Narration.mp3"].audioClip });
        }

        yield return new WaitWhile(() => audioPlayer.IsRunning);

        //show question feedback
        foreach (QuizPlayer player in ActivePlayers)
        {
            player.FeedbackArea.gameObject.SetActive(true);
            foreach (GameObject button in player.Buttons)
            {
                button.SetActive(false);
            }
            player.ButtonBox.SetActive(false);
            if (settings.questionData[_currentQuestionIndex].options.Length < 4)
            {
                player.MiddleButtonBlocker.SetActive(false);
            }
        }
        audioPlayer.Play(new AudioClip[] { audioLUT["Feedback-Narration.mp3"].audioClip});
        yield return new WaitWhile(() => audioPlayer.IsRunning);

        yield return _progressManager.ShowProgress(ActivePlayers);

        audioLUT["PercentProgress-Narration.mp3"].baseFileName = "PercentProgress-Narration-" + ProgressManager.Progress + ".mp3";
        audioLUT["PercentProgress-Narration.mp3"].Load(FAST.Application.language);
        audioPlayer.Play(new AudioClip[] { audioLUT["PercentProgress-Narration.mp3"].audioClip });
        yield return new WaitWhile(() => audioPlayer.IsRunning);

        if (ProgressManager.IsAtHalfwayPoint)
        {
            ScreenManager.ChangeScreen("Midpoint");
        }
        else if (ProgressManager.AtWin || _currentQuestionIndex == settings.questionData.Length - 1)
        {
            EndScreen.Win = ProgressManager.AtWin;
            ScreenManager.ChangeScreen("End");
        }
        else
        {
            ScreenManager.ChangeScreen("Question");
        }
    }

    private IEnumerator QuestionTimer()
    {
        audioPlayer.Play(new AudioClip[] { audioLUT["QuestionTimer-SoundEffect.mp3"].audioClip });
        yield return new WaitWhile(() => audioPlayer.IsRunning && !_allPlayersAnswered);
        audioPlayer.Stop();
        audioPlayer.ClearPlaylist();
    }

    private IEnumerator AnswerTimer()
    {
        VideoPlayer.Prepare();
        yield return new WaitUntil(() => VideoPlayer.isPrepared);

        VideoPlayer.Play();
        yield return null;
        _videoPlayerImage.SetActive(true);
        yield return new WaitWhile(() => VideoPlayer.isPlaying);

        _videoPlayerImage.SetActive(false);

        _answerPhase = false;

        if (ActivePlayers.All(x => x.PlayerAnswered == false))
        {
            _progressManager.ResetGame();
            ResetGame();
            ScreenManager.ChangeScreen("Title");
        }
        else
        {

            foreach (QuizPlayer player in ActivePlayers)
            {
                player.PlayerCorrect = CheckAnswer(player, player.CurrentlySelectedAnswer.ToArray(), _currentQuestionIndex);
                string selectedAnswer = player.CurrentlySelectedAnswer.Count > 0 ? player.CurrentlySelectedAnswer[0].ToString() : "None";
                Debug.Log("<b>" + player.Name + " SELECTED ANSWERS " + selectedAnswer + ", ARE THEY CORRECT? "
                    + player.PlayerCorrect + "</b>");
                player.ShowCorrectOrIncorrect(true);
                player.ShowScore();
            }
        }
    }

    public void ResetGame()
    {
        foreach (QuizPlayer player in ActivePlayers)
        {
            player.TurnPlayerOnOff(false);
            player.ShowQuestion(false, 0);
            player.PlayerActive = false;
        }
        _currentQuestionIndex = -1;
    }

    private bool CheckAnswer(QuizPlayer player, int[] answerIndex, int questionIndex)
    {
        QuizSettings settings = FAST.Application.settings.quizSettings;
        int[] correctAnswers = settings.questionData[questionIndex].answers.Select(x => (x.slot-1)).ToArray();

        if (correctAnswers.OrderBy((int x) => x).SequenceEqual(answerIndex.OrderBy((int x) => x)))
        {
            _progressManager.AtLeastOneCorrect = true;
            return true;
        }
        else
        {
            return false;
        }
    }

#if UNITY_EDITOR
    public override void NextScreen()
    {
        ScreenManager.ChangeScreen("Question");
        foreach(QuizPlayer player in ActivePlayers)
        {
            player.CurrentScore++;
        }
    }
#endif
}
