using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class HomeManager : MonoBehaviour {

	public enum DirectoryType
	{
		DIRECTORY1,
		DIRECTORY2,
		DIRECTORY3
	}

	//	Car position
	public Dropdown carPosDropdown;

	//	Toggle

	//	Speed
	public InputField speedField;

	//	Directory
	public Text directory;

	//	Choose training data 1
	public InputField tfDirectory1;
	public string pathFile1 = "";
	//	Choose training data 2
	public InputField tfDirectory2;
	public string pathFile2 = "";

	//	Choose training data 3
	public InputField tfDirectory3;
	public string pathFile3 = "";

	//	Choose Test case
	public InputField tfDirectory4;
	public string pathFile4 = "";

	//	Button Run
	public Button btnRun;

	//	
	public string parentPath;

	// Call first
	void Awake () {

		//	Setup dropdown lisetener
		carPosDropdown.onValueChanged.AddListener (delegate {
			onCarPosDropdownChanged(carPosDropdown);
		});

		//	Setup button RUN
		btnRun.onClick.AddListener(delegate {
			onRunTheCar();
		});

	}

	// Use this for initialization
	void Start () {
		//	Set parent path
		parentPath = Application.dataPath + "/TrainingData/";
		#if UNITY_EDITOR
		parentPath = "Assets/Resources/";
		#endif

		directory.text = parentPath;

//		pathFile1 = tfDirectory1.text = "Train1.txt";
//		pathFile2 = tfDirectory2.text = "Train2.txt";
//		pathFile3 = tfDirectory3.text = "Train3.txt";
//		pathFile4 = tfDirectory4.text = "TestCase.txt";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//	Dropdown setup
	private void onCarPosDropdownChanged (Dropdown target) {
		print (target.value);
	}

	//	Toggle value changed
	public void toggleChanged(Boolean isOn)
	{
		print (isOn);
	}

	// 	On Run the car
	private void onRunTheCar()
	{

		if (!validateFields ())
			return;

		pathFile1 = tfDirectory1.text;
		pathFile2 = tfDirectory2.text;
		pathFile3 = tfDirectory3.text;
		pathFile4 = tfDirectory4.text;

		int carPos = carPosDropdown.value;
		int speed =  System.Convert.ToInt32(speedField.text);

		PlayerPrefs.SetInt("CarPos", carPos);
		PlayerPrefs.SetInt("Speed", speed);
		if (pathFile1 == "" || pathFile2 == "" || pathFile3 == "" || pathFile4 == "") {
			PlayerPrefs.SetString("isTrainNewData", "NO");
		} else {
			PlayerPrefs.SetString("isTrainNewData", "YES");
			PlayerPrefs.SetString("Path1", parentPath + pathFile1);
			PlayerPrefs.SetString("Path2", parentPath + pathFile2);
			PlayerPrefs.SetString("Path3", parentPath + pathFile3);
			PlayerPrefs.SetString("Path4", parentPath + pathFile4);
		}

		SceneManager.LoadScene("SystemTraining");	
	}

	private bool validateFields()
	{
		if (speedField.text.Length == 0 || !IsDigitsOnly (speedField.text)) {
			return false;
		}

		return true;
	}
		
	private bool IsDigitsOnly(string str)
	{
		foreach (char c in str)
		{
			if (c < '0' || c > '9')
				return false;
		}

		return true;
	}
}
