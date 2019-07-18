using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using System.IO;

[UnityEditor.InitializeOnLoad]
static class UmeSync
{
    static UmeSync()
    {
		UnityEditor.SceneManagement.EditorSceneManager.sceneSaved += OnSceneSaved;
		UnityEditor.SceneManagement.EditorSceneManager.sceneClosed += OnSceneSaved;
		UnityEditor.EditorApplication.projectWindowChanged += SyncRepo; 
		//UnityEditor.EditorApplication.projectChanged += SyncRepo; 
    }

    static void OnSceneSaved(UnityEngine.SceneManagement.Scene scene)
    {
		SyncRepo ();

    }




	static void SyncRepo(){

		ProcessStartInfo pinfo = new ProcessStartInfo();
		pinfo.WindowStyle = ProcessWindowStyle.Hidden;
		pinfo.UseShellExecute = false;
		pinfo.FileName = "python";
		pinfo.Arguments = string.Format ("{0}/umelaunch/repo_sync.py {1} unity", System.Environment.GetEnvironmentVariable("UME_ROOT"), Application.dataPath);
		pinfo.RedirectStandardOutput = true;

		using(Process process = Process.Start(pinfo))
		{
			using(StreamReader reader = process.StandardOutput)
			{
				string result = reader.ReadToEnd();
				UnityEngine.Debug.LogFormat("{0}", result);
			}
		}
	}


}


