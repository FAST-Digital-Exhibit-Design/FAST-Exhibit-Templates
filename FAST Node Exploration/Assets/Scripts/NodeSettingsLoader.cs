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

using System;
using System.IO;
using System.Collections;
using System.Xml.Serialization;
using UnityEngine;
using FAST;

namespace NodeExploration
{
    public class NodeSettingsLoader : StartupLoader
    {
        private string nodeSettingsPath;

        protected override IEnumerator ExecuteLoad()
        {
            string skinPath = Path.Combine(FAST.Application.assetsDirectory, FAST.Application.skin);

            loadingTitle = $"Searching for node settings . . .";
            Debug.Log($"\n{loadingTitle}");
            loadingMessage = $"Search pattern: {Path.Combine(skinPath, "*-Node-settings.xml")}";
            Debug.Log($"{loadingMessage}");
            loadingEvent.Invoke(loadingTitle, loadingMessage);

            string[] paths = Directory.GetFiles(skinPath, "*-Node-settings.xml", SearchOption.AllDirectories);
            if (paths.Length == 0) {
                errorTitle = "File not found!";
                errorMessage = "The node settings file cannot be found." +
                    $"It is expected within the {FAST.Application.skin} folder where this application is installed: " +
                    $"\n\t{FAST.Application.activityDirectory}\n\t\tAssets\n\t\t\t{FAST.Application.skin}\n\t\t\t*-Node-settings.xml";
                Debug.LogError($"\nERROR\n{errorTitle}\n{errorMessage}\n");
                errorEvent.Invoke(errorTitle, errorMessage);
                yield break;
            }
            else if (paths.Length > 1) {
                errorTitle = "Too many files found!";
                errorMessage = $"{paths.Length} node settings files were found. Only the first file will be loaded.";
                Debug.LogWarning($"\nWARNING\n{errorTitle}\n{errorMessage}\n");
            }
            yield return new WaitForSecondsRealtime(loadingMessageDuration);

            nodeSettingsPath = paths[0];
            loadingTitle = "Loading node settings . . .";
            Debug.Log($"\n{loadingTitle}");
            loadingMessage = "File: " + nodeSettingsPath;
            Debug.Log($"{loadingMessage}");
            loadingEvent.Invoke(loadingTitle, loadingMessage);

            if (!ReadNodeSettings()) {
                errorTitle = "File not accessible!";
                errorMessage = "The settings file cannot be read. The XML may be incorrectly formatted or malformed.";
                Debug.LogError($"\nERROR\n{errorTitle}\n{errorMessage}\n");
                errorEvent.Invoke(errorTitle, errorMessage);
                yield break;
            }
            yield return new WaitForSecondsRealtime(loadingMessageDuration);

            successEvent.Invoke();
        }

        private bool ReadNodeSettings()
        {
            bool result = true;
            try {
                Type type = typeof(NodeSettings);
                XmlSerializer serializer;
                FileStream stream;
                serializer = new XmlSerializer(type);
                stream = new FileStream(nodeSettingsPath, FileMode.Open);
                // *NOTE* This creates a new object and changes the node settings reference
                FAST.Application.settings.nodeSettings = serializer.Deserialize(stream) as NodeSettings;
                stream.Close();
            }
            catch (Exception exception) {
                Debug.Log("Couldn't read node settings file.");
                Debug.Log(exception.Message);
                result = false;
            }

            return result;
        }
    }
}