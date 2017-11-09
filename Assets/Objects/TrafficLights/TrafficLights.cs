using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLights : MonoBehaviour {

	public enum LightType
	{
		RED,
		YELLOW,
		GREEN
	}

	public SpriteRenderer redLight;
	public SpriteRenderer yellowLight;
	public SpriteRenderer greenLight;
	public Car.Direction carDirection;

	private float time = 0;
	public LightType lightType = LightType.RED;

	// Use this for initialization
	void Start () {
		updateLight ();
	}
	
	// Update is called once per frame
	void Update () {
		lightsController ();
	}

	void lightsController(){
		time += Time.deltaTime;

		switch (lightType) {
		case LightType.RED:
			{
				if (time >= 4) {
					time = 0;
					lightType = LightType.GREEN;
					updateLight ();
				}
				break;
			}
		case LightType.YELLOW:
			{
				if (time >= 1) {
					time = 0;
					lightType = LightType.RED;
					updateLight ();
				}
				break;
			}
		case LightType.GREEN:
			{
				if (time >= 5) {
					time = 0;
					lightType = LightType.YELLOW;
					updateLight ();
				}
				break;
			}
		}
	}

	void updateLight(){
		redLight.color = Color.white;
		yellowLight.color = Color.white;
		greenLight.color = Color.white;
		switch (lightType) {
		case LightType.RED:
			{
				redLight.color = Color.red;
				break;
			}
		case LightType.YELLOW:
			{
				yellowLight.color = Color.yellow;
				break;
			}
		case LightType.GREEN:
			{
				greenLight.color = Color.green;
				break;
			}
		}
	}
}
