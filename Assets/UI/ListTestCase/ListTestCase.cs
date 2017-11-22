using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class ListTestCase : MonoBehaviour {

	public GameObject content;

	// Use this for initialization
	void Start () {
		//	Init
		initialListTestCase ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//	Initial List Test Case
	void initialListTestCase()
	{	
		//	Load test cases from file
		string path = PlayerPrefs.GetString("Path4");
		List<string> testCases = loadTestCasesFromFile (path);

		//	Load test cases
		float heightOfCaseBtn = 30f;
		float spacing = 4f;
		int numOfTestCase = testCases.Count;
		float heightOfContent = (heightOfCaseBtn + spacing) * numOfTestCase + spacing;

		for (int i = 0; i < numOfTestCase; i++) {
			TestCaseData testCaseData = new TestCaseData (testCases [i]);
			float y = (heightOfContent / 2f) - ((heightOfCaseBtn + spacing) * i + spacing + heightOfCaseBtn/2f);
			GameObject testCaseObjc = Instantiate (Resources.Load("Prefap/TestCase", typeof(GameObject))) as GameObject;
			testCaseObjc.GetComponent<RectTransform> ().SetParent (content.transform);
			testCaseObjc.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, y);
			testCaseObjc.GetComponent<TestCase> ().setTestCaseData(testCaseData);
		}

		//	Set height of content
		RectTransform rt = content.GetComponent<RectTransform> ();
		rt.sizeDelta = new Vector2 (0f, heightOfContent);
	}

	//	Load test cases from file
	List<string> loadTestCasesFromFile(string path)
	{
		List<string> testCases = new List<string> ();

		StreamReader reader =  new StreamReader(path);
		string text = reader.ReadLine ();
		while (text != null) {
			//	Add a test case
			testCases.Add (text);

			//	Read next line
			text = reader.ReadLine ();
		}

		return testCases;
	}
}
