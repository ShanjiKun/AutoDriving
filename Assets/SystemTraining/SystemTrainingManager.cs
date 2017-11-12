using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AssemblyCSharp;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SystemTrainingManager : MonoBehaviour {

	public Text txTraining;
	private float timeCount;
	private int times = 0;
	private bool moveToMainScreen;

	// Use this for initialization
	void Start () {
//		string path1 = PlayerPrefs.GetString("Path1");
//		string path2 = PlayerPrefs.GetString("Path2");
//		string path3 = PlayerPrefs.GetString("Path3");

//		bool isTrainingSuccess = AIManager.TrainSystem(path1, path2, path3);
//		if (isTrainingSuccess) {
//			print ("Training data ready!");
//		} else {
//			print ("Training data Failed :(");
//		}
	}
	
	// Update is called once per frame
	void Update () {
		timeCount += Time.deltaTime;
		if (timeCount >= 0.5f && !moveToMainScreen) {
			times++;
			timeCount = 0;
			switch (txTraining.text) {
			case "Training...":
				txTraining.text = "Training.";
				break;
			case "Training.":
				txTraining.text = "Training..";
				break;
			default:
				txTraining.text = "Training...";
				break;
			}

			if (times > 4) {
				moveToMainScreen = true;
				navigateToMainScreen ();
			}
		}
	}

	void navigateToMainScreen()
	{
		txTraining.text = "Training success!";
		#if UNITY_EDITOR
		if (EditorUtility.DisplayDialog ("Training data success!",
			    "Press Run button to start.", "RUN", "Cancel")) {
			SceneManager.LoadScene ("MainScreen");	
		} else {
			print ("You don't want to start");
		}
		#endif
	}
}
