using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWalkController : MonoBehaviour {

	public enum MapSideWorkPos
	{
		Top = 1,
		Bottom = 2,
		Left = 3,
		Right = 4
	}

	public SpriteRenderer sprite;
	public BoxCollider2D boxCollider;
	public MapSideWorkPos sidewalkPos;

	// Use this for initialization
	void Start () {
		// Set up layout
//		setupLayout ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// MARK: Handles
	void setupLayout() {

		Vector3 position = new Vector3();
		Vector3	localScale = new Vector3();
		Vector2 size = new Vector2();
		Color color = new Color();

		switch (sidewalkPos) {
			case MapSideWorkPos.Top:
				{
					// Setup transform
					position = new Vector3(0, 5, 0);
					localScale = new Vector3 (14, 0.1f, 1);
					// Setup Box Collider 2D
					size = new Vector2(0.95f, 1.1f);

					color = Color.gray;
					break;
				}
			case MapSideWorkPos.Bottom:
				{
					// Setup transform
					position = new Vector3(0, -5, 0);
					localScale = new Vector3 (14, 0.1f, 1);
					// Setup Box Collider 2D
					size = new Vector2(0.95f, 1.1f);

					color = Color.gray;
					break;
				}
			case MapSideWorkPos.Left:
				{
					// Setup transform
					position = new Vector3(-6.66f, 0, 0);
					localScale = new Vector3 (0.1f, 10, 1);
					// Setup Box Collider 2D
					size = new Vector2(0.95f, 1f);

					color = Color.gray;
					break;
				}
			case MapSideWorkPos.Right:
				{
					// Setup transform
					position = new Vector3(6.66f, 0, 0);
					localScale = new Vector3 (0.1f, 10, 1);
					// Setup Box Collider 2D
					size = new Vector2(0.95f, 1f);

					color = Color.gray;
					break;
				}
			default:
				break;
		}

		// Setup transform
		this.transform.position = position;
		this.transform.localScale = localScale;

		// Setup Box Collider 2D
		this.boxCollider.size = size;

		sprite.color = color;
	}
}
