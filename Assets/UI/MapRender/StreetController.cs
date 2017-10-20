using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography.X509Certificates;

public class StreetController : MonoBehaviour {

	public enum MapStreetPos
	{
		TOP = 1,
		BOTTOM = 2,
		LEFT = 3,
		RIGHT = 4,
		VERTICAL = 5,
		HORIZONTAL = 6
	}

	public enum StreetStyle
	{
		VERTICAL = 1,
		HORIZONTAL = 2
	}


	public MapStreetPos mapStreetPos;
	public StreetStyle streetStyle;

	//	Range
	public float limitPointNorth;
	public float limitPointSouth;
	public float limitPointWest;
	public float limitPointEast;


	//	Awake
	void Awake(){
		initialStreetStyle();
	}

	// Use this for initialization
	void Start () {
		setupLayout();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// MARK: Handles
	void initialStreetStyle() {
		StreetStyle streetStyleNew = StreetStyle.VERTICAL;
		switch (mapStreetPos) 
		{
			case MapStreetPos.TOP:
			case MapStreetPos.HORIZONTAL:
			case MapStreetPos.BOTTOM:
				{
				streetStyleNew = StreetStyle.HORIZONTAL;
					break;
				}
			case MapStreetPos.LEFT:
			case MapStreetPos.VERTICAL:
			case MapStreetPos.RIGHT:
				{
				streetStyleNew = StreetStyle.VERTICAL;
					break;
				}
			default:
				break;
		}
		this.streetStyle = streetStyleNew;
	}

	void setupLayout() {
		Vector3 position = new Vector3 ();
		Vector3 scale = new Vector3 ();

		switch (mapStreetPos) {
			case MapStreetPos.TOP:
				{
					position = new Vector3 (0, 4.15f, 0);
					scale = new Vector3 (13.1f, 1.5f, 1);
					break;
				}
			case MapStreetPos.BOTTOM:
				{
					position = new Vector3 (0, -4.15f, 0);
					scale = new Vector3 (13.1f, 1.5f, 1);
					break;
				}
			case MapStreetPos.HORIZONTAL:
				{
					position = new Vector3 (0, 0, 0);
					scale = new Vector3 (13.1f, 1.5f, 1);
					break;
				}
			case MapStreetPos.VERTICAL:
				{
					position = new Vector3 (0, 0, 0);
					scale = new Vector3 (1.5f, 9.8f, 1);
					break;
				}
			case MapStreetPos.LEFT:
				{
					position = new Vector3 (-5.8f, 0, 0);
					scale = new Vector3 (1.5f, 9.8f, 1);
					break;
				}
			case MapStreetPos.RIGHT:
				{
					position = new Vector3 (5.8f, 0, 0);
					scale = new Vector3 (1.5f, 9.8f, 1);
					break;
				}
			default: break;
		}

		//	Update new position and scale
		this.transform.position = position;
		this.transform.localScale = scale;
	}
}
