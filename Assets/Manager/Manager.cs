using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class Manager : MonoBehaviour {

	public GameObject enemy;

	// Use this for initialization
	void Start () {
//		testBayes ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Camera nearCamera = GameObject.FindGameObjectWithTag ("NearCamera").GetComponent<Camera>();
			Vector3 newPos = nearCamera.ScreenToWorldPoint (Input.mousePosition);
			enemy.transform.position = new Vector2 (newPos.x, newPos.y);
		}
	}

	//	Test bayes
	void testBayes()
	{
//		CustomNaiveBayes bayes = CustomNaiveBayes.sharedDecision1 ();
//		bool isTrainSuccess = bayes.trainSystemWithData ("Text1.txt");
//		print ("Traning state: " + isTrainSuccess);
//		if (!isTrainSuccess)
//			return;
//
//		print ("Test class");
//		for (int i = 0; i < bayes.bclasses.Count; i++) {
//			BClass bclass = bayes.bclasses [i];
//			print (bclass.name + " " + bclass.count);
//		}
//		print ("________________________________________");
//		print ("Test Props");
//		for (int i = 0; i < bayes.properties.Count; i++) {
//			List<Properties> childProps = bayes.properties [i];
//			for (int j = 0; j < childProps.Count; j++) {
//				Properties prop = childProps [j];
//				foreach(KeyValuePair<string, int> item in prop.dictClass)
//					print (prop.name + " " + item.Key + " " + item.Value);
//			}
//		}
//		print ("________________________________________");
//		print ("Test Train Data");
//		print ("________________________________________");
//		print ("Show P off Class: ");
//		foreach (BClass bclass in bayes.bclasses) {
//			print ("P(" + bclass.name + ") = " + bclass.P);
//		}
//		print ("________________________________________");
//		print ("Show P off Properties: ");
//		foreach (List<Properties> childProps in bayes.properties) {
//			foreach (Properties prop in childProps) {
//				foreach (BClass bclass in bayes.bclasses) {
//					print ("P(" + prop.name + "|" + bclass.name + ") = " + prop.getPOfClass (bclass.name));
//				}
//			}
//		}
//
//		print ("________________________________________");
//		print ("Test");
//		print ("Decision: " + bayes.getDecisionWithTest ("P1_1,P2_2,P3_1"));
	}
}
