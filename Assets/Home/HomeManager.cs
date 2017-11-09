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

	//	Speed
	public InputField speedField;

	//	Choose training data 1
	public InputField tfDirectory1;
	public string pathFile1 = "";
	public Button btnBrowser1;

	//	Choose training data 2
	public InputField tfDirectory2;
	public string pathFile2 = "";
	public Button btnBrowser2;

	//	Choose training data 3
	public InputField tfDirectory3;
	public string pathFile3 = "";
	public Button btnBrowser3;

	public TextAsset t1;
	public TextAsset t2;
	public TextAsset t3;

	//	Button Run
	public Button btnRun;

	// Call first
	void Awake () {

		//	Setup dropdown lisetener
		carPosDropdown.onValueChanged.AddListener (delegate {
			onCarPosDropdownChanged(carPosDropdown);
		});

		//	Setup buttons browser
		btnBrowser1.onClick.AddListener(delegate {
			onChooseDirectory(DirectoryType.DIRECTORY1);
		});

		btnBrowser2.onClick.AddListener(delegate {
			onChooseDirectory(DirectoryType.DIRECTORY2);
		});

		btnBrowser3.onClick.AddListener(delegate { 
			onChooseDirectory(DirectoryType.DIRECTORY3);
		});

		//	Setup button RUN
		btnRun.onClick.AddListener(delegate {
			onRunTheCar();
		});

	}

	// Use this for initialization
	void Start () {
		pathFile1 = tfDirectory1.text = "Assets/Resources/Test1.txt";
		pathFile2 = tfDirectory2.text = "Assets/Resources/Test2.txt";
		pathFile3 = tfDirectory3.text = "Assets/Resources/Test3.txt";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//	Dropdown setup
	private void onCarPosDropdownChanged (Dropdown target) {
		print (target.value);
	}

	// 	On Run the car
	private void onRunTheCar()
	{

		if (!validateFields ())
			return;

		int carPos = carPosDropdown.value;
		int speed =  System.Convert.ToInt32(speedField.text);

		PlayerPrefs.SetInt("CarPos", carPos);
		PlayerPrefs.SetInt("Speed", speed);
		PlayerPrefs.SetString("Path1", pathFile1);
		PlayerPrefs.SetString("Path2", pathFile2);
		PlayerPrefs.SetString("Path3", pathFile3);

		SceneManager.LoadScene("SystemTraining");	
	}

	private bool validateFields()
	{
		if (speedField.text.Length == 0 || !IsDigitsOnly (speedField.text)) {
			showAlert ("Speed error!", "Speed is just number");
			return false;
		}

		if (pathFile1 == "" || pathFile2 == "" || pathFile3 == "") {
			showAlert ("Train data paths error!", "Enter fully 3 paths.");
			return false;
		}

		return true;
	}

	//	On browser
	private void onChooseDirectory (DirectoryType type)
	{
		string path = chooseFile ();
		//	Cut path
		string shortPath = path;
		string [] pathElems = path.Split("/"[0]);
		if (pathElems.Length > 3) {
			shortPath = pathElems [pathElems.Length - 3] + "/" + pathElems [pathElems.Length - 2] + "/" + pathElems [pathElems.Length - 1];
		}

		switch (type) {
		case DirectoryType.DIRECTORY1:
			{
				pathFile1 = path;
				tfDirectory1.text = shortPath;
			}
			break;
		case DirectoryType.DIRECTORY2:
			{
				pathFile2 = path;
				tfDirectory2.text = shortPath;
			}
			break;
		default:
			{
				pathFile3 = path;
				tfDirectory3.text = shortPath;
			}
			break;
		}	
	}

	//	Choose a file
	private string chooseFile()
	{
		#if UNITY_EDITOR
		string path = EditorUtility.OpenFilePanel("Choose training data:", "", "txt");
		if (path.Length != 0) {
			return path;
		}
		#endif
		return "";
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

	private void showAlert(string title, string message)
	{
		#if UNITY_EDITOR
		EditorUtility.DisplayDialog (title,
			message, "OK", "Cancel");
		#endif
	}

	//	Open File Panel
	// PlayerPrefs.SetInt("SavedNumber", numberToSave);
	// number = PlayerPrefs.GetInt("SavedNumber");
}
