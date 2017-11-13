using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AssemblyCSharp;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SystemTrainingManager : MonoBehaviour {

	public Text txTraining;
	public Text state;
	private float timeCount;
	private int times = 0;
	private bool moveToMainScreen;

	// Use this for initialization
	void Start () {
		string isTrainNewData = PlayerPrefs.GetString ("isTrainNewData");

		bool isTrainingSuccess;
		if (isTrainNewData == "YES") {
			string path1 = PlayerPrefs.GetString("Path1");
			string path2 = PlayerPrefs.GetString("Path2");
			string path3 = PlayerPrefs.GetString("Path3");

			isTrainingSuccess = AIManager.TrainSystem(path1, path2, path3);
			if (isTrainingSuccess) {
				state.text = "New training data ready!";
				saveTrainData ();
			} else {
				state.text = "New training data Failed :(";
			}
		} else {
			loadTrainData ();
			if (CustomNaiveBayes.decision1 == null ||
			    CustomNaiveBayes.decision2 == null ||
			    CustomNaiveBayes.decision3 == null) {
				state.text = "Load training data Failed :(";
			} else {
				state.text = "Load training data success!";
			}
		}
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

	void saveTrainData()
	{

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file1 = File.Create (Application.persistentDataPath + "/Train1.bin");
		bf.Serialize(file1, CustomNaiveBayes.sharedDecision1());
		file1.Close();

		FileStream file2 = File.Create (Application.persistentDataPath + "/Train2.bin");
		bf.Serialize(file2, CustomNaiveBayes.sharedDecision2());
		file2.Close();

		FileStream file3 = File.Create (Application.persistentDataPath + "/Train3.bin");
		bf.Serialize(file3, CustomNaiveBayes.sharedDecision3());
		file3.Close();
	}

	void loadTrainData()
	{
		if(File.Exists(Application.persistentDataPath + "/Train1.bin")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/Train1.bin", FileMode.Open);
			CustomNaiveBayes.decision1 = bf.Deserialize(file) as CustomNaiveBayes;
			file.Close();
		}

		if(File.Exists(Application.persistentDataPath + "/Train2.bin")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/Train2.bin", FileMode.Open);
			CustomNaiveBayes.decision2 = bf.Deserialize(file) as CustomNaiveBayes;
			file.Close();
		}

		if(File.Exists(Application.persistentDataPath + "/Train3.bin")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/Train3.bin", FileMode.Open);
			CustomNaiveBayes.decision3 = bf.Deserialize(file) as CustomNaiveBayes;
			file.Close();
		}
	}

	void navigateToMainScreen()
	{
		txTraining.text = "Training success!";
		SceneManager.LoadScene("MainScreen");	
	}
}
