using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCase : MonoBehaviour {

	public string enemyLane;
	public int left;
	public int right;
	public int front;
	public int behind;

	public Manager manager;

	public void onClick()
	{
		TestCaseData data = new TestCaseData ("L", 5, 0, 2, 2);
		manager.testCaseData = data;
	}
}
