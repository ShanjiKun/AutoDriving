using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomLog : MonoBehaviour {

	public Text content;

	public void log(string text)
	{
		content.text = text + "\n" + content.text;
	}

}
