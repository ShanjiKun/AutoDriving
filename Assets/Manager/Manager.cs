using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class Manager : MonoBehaviour {

	public GameObject enemy;
	public TestCaseData testCaseData;

	public List<Street> streets;
	public Street selectedStreet;

	//	For test case enemy
	public GameObject enemyTC_L_H;
	public GameObject enemyTC_R_H;
	public GameObject enemyTC_A_V;
	public GameObject enemyTC_B_V;

	void Awake()
	{
		enemyTC_L_H = GameObject.FindGameObjectWithTag ("EnemyTestCase_L_H");
		enemyTC_R_H = GameObject.FindGameObjectWithTag ("EnemyTestCase_R_H");
		enemyTC_A_V = GameObject.FindGameObjectWithTag ("EnemyTestCase_A_V");
		enemyTC_B_V = GameObject.FindGameObjectWithTag ("EnemyTestCase_B_V");

		//	reset position
		resetEnemyTestCases();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
//			Camera nearCamera = GameObject.FindGameObjectWithTag ("NearCamera").GetComponent<Camera>();
//			Vector3 newPos = nearCamera.ScreenToWorldPoint (Input.mousePosition);
//			enemy.transform.position = new Vector2 (newPos.x, newPos.y);
			//	
			detectSelectedStreet();
		}
	}

	void detectSelectedStreet()
	{
		//	Get click pos
		Camera nearCamera = GameObject.FindGameObjectWithTag ("NearCamera").GetComponent<Camera>();
		Vector3 clickPos = nearCamera.ScreenToWorldPoint (Input.mousePosition);

		//	Detect selected street
		foreach (Street street in streets) {
			//	Get pos range of street
			float xMin, xMax, yMin, yMax;
			Vector2 streetPos = street.transform.position;
			if (street.direction == Street.Direction.VERTICAL) {
				xMin = streetPos.x - 0.75f;
				xMax = streetPos.x + 0.75f;
				yMin = streetPos.y - 20f;
				yMax = streetPos.y + 20f;
			} else {
				yMin = streetPos.y - 0.75f;
				yMax = streetPos.y + 0.75f;
				xMin = streetPos.x - 20f;
				xMax = streetPos.x + 20f;
			}

			if (clickPos.x <= xMax && clickPos.x >= xMin && clickPos.y <= yMax && clickPos.y >= yMin) {
				selectedStreet = street;
				break;
			}
		}

		//	Show Enemy test case on selected street
		showEnemyTestCase();

		//	Remove selected street
		selectedStreet = null;
	}

	//	Show enemy test case
	void showEnemyTestCase()
	{	
		if (selectedStreet == null)
			return;
		if (testCaseData == null) {
			print ("Test case null");
			return;
		}

		//	Reset enemy test case pos
		resetEnemyTestCases();

		//	Get position of mouse
		Camera nearCamera = GameObject.FindGameObjectWithTag ("NearCamera").GetComponent<Camera>();
		Vector3 p = nearCamera.ScreenToWorldPoint (Input.mousePosition);

		//	
		if (selectedStreet.direction == Street.Direction.VERTICAL) {
			float x = selectedStreet.transform.position.x;
			float y = p.y;

			Vector2 pos = new Vector2 (x, y);
			if (testCaseData.enemyLane == "L") {
				enemyTC_L_H.transform.position = pos;
				enemyTC_L_H.GetComponent<EnemyTestCase> ().testCaseData = testCaseData;
			} else {
				enemyTC_R_H.transform.position = pos;
				enemyTC_R_H.GetComponent<EnemyTestCase> ().testCaseData = testCaseData;
			}

		} else {
			float x = p.x;
			float y = selectedStreet.transform.position.y;

			Vector2 pos = new Vector2 (x, y);
			if (testCaseData.enemyLane == "L") {
				enemyTC_A_V.transform.position = pos;
				enemyTC_A_V.GetComponent<EnemyTestCase> ().testCaseData = testCaseData;
			} else {
				enemyTC_B_V.transform.position = pos;
				enemyTC_B_V.GetComponent<EnemyTestCase> ().testCaseData = testCaseData;
			}
		}
	}

	//	Enemy test case
	void resetEnemyTestCases()
	{
		Vector2 pos = new Vector2 (-50f, -50f);
		enemyTC_L_H.transform.position = pos;
		enemyTC_R_H.transform.position = pos;
		enemyTC_A_V.transform.position = pos;
		enemyTC_B_V.transform.position = pos;
	}
}
