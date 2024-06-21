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

using System;
using System.IO;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEditor;
using FAST;

public class QuizSettingsLoader : StartupLoader
{
    private string quizSettingsPath;

    protected override IEnumerator ExecuteLoad()
    {
        string skinPath = Path.Combine(FAST.Application.assetsDirectory, FAST.Application.skin);

        loadingTitle = $"Searching for quiz settings . . .";
        Debug.Log($"\n{loadingTitle}");
        loadingMessage = $"Search pattern: {Path.Combine(skinPath, "*-quiz-settings.xml")}";
        Debug.Log($"{loadingMessage}");
        loadingEvent.Invoke(loadingTitle, loadingMessage);

        string[] paths = Directory.GetFiles(skinPath, "*-quiz-settings.xml", SearchOption.AllDirectories);
        if (paths.Length == 0) {
            errorTitle = "File not found!";
            errorMessage = "The quiz settings file cannot be found." +
                $"It is expected within the {FAST.Application.skin} folder where this application is installed: " +
                $"\n\t{FAST.Application.activityDirectory}\n\t\tAssets\n\t\t\t{FAST.Application.skin}\n\t\t\t*-quiz-settings.xml";
            Debug.LogError($"\nERROR\n{errorTitle}\n{errorMessage}\n");
            errorEvent.Invoke(errorTitle, errorMessage);
            yield break;
        }
        else if (paths.Length > 1) {
            errorTitle = "Too many files found!";
            errorMessage = $"{paths.Length} quiz settings files were found. Only the first file will be loaded.";
            Debug.LogWarning($"\nWARNING\n{errorTitle}\n{errorMessage}\n");
        }
        yield return new WaitForSecondsRealtime(loadingMessageDuration);

        quizSettingsPath = paths[0];
        loadingTitle = "Loading quiz settings . . .";
        Debug.Log($"\n{loadingTitle}");
        loadingMessage = "File: " + quizSettingsPath;
        Debug.Log($"{loadingMessage}");
        loadingEvent.Invoke(loadingTitle, loadingMessage);

        if (!ReadQuizSettings()) {
            errorTitle = "File not accessible!";
            errorMessage = "The settings file cannot be read. The XML may be incorrectly formatted or malformed.";
            Debug.LogError($"\nERROR\n{errorTitle}\n{errorMessage}\n");
            errorEvent.Invoke(errorTitle, errorMessage);
            yield break;
        }
        yield return new WaitForSecondsRealtime(loadingMessageDuration);

        successEvent.Invoke();
    }

    private bool ReadQuizSettings()
    {
        bool result = true;
        try {
            Type type = typeof(QuizSettings);
            XmlSerializer serializer;
            FileStream stream;
            serializer = new XmlSerializer(type);
            stream = new FileStream(quizSettingsPath, FileMode.Open);
            // *NOTE* This creates a new object and changes the quiz settings reference
            FAST.Application.settings.quizSettings = serializer.Deserialize(stream) as QuizSettings;
            stream.Close();
        }
        catch (Exception exception) {
            Debug.Log("Couldn't read quiz settings file.");
            Debug.Log(exception.Message);
            result = false;
        }

        return result;
    }
}
