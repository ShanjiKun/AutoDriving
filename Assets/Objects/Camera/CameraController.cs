using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public enum Type
	{
		FAR,
		NEAR
	}

	public Type type;
	public GameObject car;

	private Camera mainCamera;
	private Camera nearCamera;

	// Use this for initialization
	void Start () {

		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
		nearCamera = GameObject.FindGameObjectWithTag ("NearCamera").GetComponent<Camera>();

		mainCamera.enabled = true;
		nearCamera.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		switch (type) {
		case Type.FAR:
			{
				mainCamera.enabled = true;
				nearCamera.enabled = false;
				break;
			}
		case Type.NEAR:
			{
				mainCamera.enabled = false;
				nearCamera.enabled = true;

				Vector3 pos = car.transform.position;
				nearCamera.transform.position = new Vector3(pos.x, pos.y, -10);

//				Vector3 rotate = car.transform.eulerAngles;
//				nearCamera.transform.eulerAngles = rotate;

				break;
			}
		}
	}
}
