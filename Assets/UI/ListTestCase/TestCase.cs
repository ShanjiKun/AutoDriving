using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCase : MonoBehaviour {

	private TestCaseData testCaseData;
	public Text text;
	private Manager manager;
	void Awake()
	{
		manager = GameObject.FindGameObjectWithTag ("ObjectManager").GetComponent<Manager>();
	}

	public void onClick()
	{
		if (testCaseData == null)
			return;
		
		manager.testCaseData = testCaseData;
	}

	public void setTestCaseData(TestCaseData data)
	{
		this.testCaseData = data;
		text.text = "EP:" + data.enemyLane + "  L:" + data.left + "  R:" + data.right + "  F:" + data.front + " B:" + data.behind;
	}
}
