using UnityEngine;
using System.Collections;

public class SillhouetteShader : MonoBehaviour {


	public SpriteRenderer renderer;
	public bool inDojo;
	private SpriteRenderer thisRenderer;
	private Sprite sprite;
	public int sortingOrderLayer = 1;

	// Use this for initialization
	void Start () {

		thisRenderer = GetComponent<SpriteRenderer>();
		//sprite = renderer.sprite;
		//renderer.color = Color.black;

	}

	// Update is called once per frame
	void Update () {

		DisplaySprite();

		if(inDojo)
		{
			thisRenderer.enabled = true;
			renderer.enabled = false;
		}
		else
		{
			thisRenderer.enabled = false;
			renderer.enabled = true;
		}
	}

	void DisplaySprite()
	{
		sprite = renderer.sprite;
		thisRenderer.sprite = sprite;
		thisRenderer.color = Color.black;

		thisRenderer.sortingLayerName = "ForeGround";
		thisRenderer.sortingOrder = 1;
	}
}
