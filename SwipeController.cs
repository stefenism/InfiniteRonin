using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwipeController : MonoBehaviour {

	private static SwipeController instance;

	public Color c1 = Color.yellow;
	public Color c2 = Color.red;

	//private GameObject lineGO;
	//private LineRenderer lineRenderer;
	//private int i = 0;

	private Vector2 StartPoint;
	private Vector2 EndPoint;

	public PlayerController playerController;

	public GameObject trailPrefab;

	private Vector2 touchOrigin = -Vector2.one;

	// Use this for initialization
	void Awake()
	{
		instance = this;
	}

	void Start () {

		//SetupLine();


	}

	// Update is called once per frame
	void Update () {

		CreateLine();

	}

	/*
	void SetupLine()
	{

		/* Create Empty "Line" GameObject to house our swipe


		lineGO = new GameObject("Line");
		lineGO.AddComponent<LineRenderer>();
		lineRenderer = lineGO.GetComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Mobile/Particles/Additive"));
		lineRenderer.SetColors(c1,c2);
		lineRenderer.SetWidth(0.3f, 0);
		lineRenderer.SetVertexCount(0);
	}
	*/

	void CreateLine()
	{

		/* Actually Create Said "Line" GameObject and Display it on the screen */


		if (Input.touchCount > 0)
		{
			Touch touch = Input.touches[0];

			if (touch.phase == TouchPhase.Began)
			{
				touchOrigin = touch.position;
			}

			else if (touch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
			{
				Vector2 touchEnd = touch.position;
				float x = touchEnd.x - touchOrigin.x;
				float y = touchEnd.y - touchOrigin.y;
				touchOrigin.x = -1;

				//swipe to the right
				if (Mathf.Abs(x) > Mathf.Abs(y) && x > 0)
				{
					playerController.Thrustbool = true;
				}
				//swipe to the left
				else if (Mathf.Abs(x) > Mathf.Abs(y) && x < 0)
				{
					//something needs to be done here
				}
				//swiping up
				else if (Mathf.Abs(y) > Mathf.Abs(x) && y > 0)
				{
					playerController.High = true;
				}
				//swiping down
				else if (Mathf.Abs(y) > Mathf.Abs(x) && y < 0)
				{
					playerController.Low = true;
				}
				//tapping
				else if (Mathf.Abs(y) == Mathf.Abs(x))
				{
					Vector3 position = Camera.main.ScreenToWorldPoint(touch.position);

					//then do something at the desired point
					playerController.Tapped = true;
					playerController.JumpBool = true;
				}

				else
				{
					playerController.Thrustbool = false;
					playerController.High = false;
					playerController.Low = false;
					playerController.JumpBool = false;
				}
			}
		}
		/*
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);

			// -- Tap: quick touch and release

			if (touch.phase == TouchPhase.Ended && touch.tapCount == 1)
			{
				Vector3 position = Camera.main.ScreenToWorldPoint(touch.position);

				//then do something at the desired point
				playerController.Tapped = true;
			}


			// -- Drag

			else
			{

				if (touch.phase == TouchPhase.Began)
				{
					//Store this new value
					if (trails.ContainsKey(i) == false)
					{
						Vector3 position = Camera.main.ScreenToWorldPoint(touch.position);
						position.z = 0;
						StartPoint = position;

						GameObject trail = MakeTrail(position);

						if (trail != null)
						{
							Debug.Log(trail);
							trails.Add(i, trail);
						}

					}
				}


				if (touch.phase == TouchPhase.Moved)
				{
					//Move the trail
					if(trails.ContainsKey(i))
					{
						GameObject trail = trails[i];

						//Camera.main.ScreenToWorldPoint(touch.position);
						Vector3 position = Camera.main.ScreenToWorldPoint(touch.position);
						position.z = 0;


						trail.transform.position = position;
					}
				}

				if (touch.phase == TouchPhase.Ended)
				{
					//Clear known trails
					if (trails.ContainsKey(i))
					{
						GameObject trail = trails[i];

						Vector3 position = Camera.main.ScreenToWorldPoint(touch.position);
						EndPoint = position;

						// Let the trail fade out
						//lineRenderer.SetVertexCount(0);
						Destroy(trail, .1f);
						trails.Remove(i);

						DetermineSlash(DetermineSwipeAngle(StartPoint, EndPoint));
					}
				}

				playerController.Tapped = false;
			}

			//StartPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			*/

			// ******************************************************************//
			//				This is Old Stuff
			/*
			if(touch.phase == TouchPhase.Moved)
			{
				lineRenderer.SetVertexCount(i + 1);
				Vector3 mPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 15);
				lineRenderer.SetPosition(i, Camera.main.ScreenToWorldPoint(mPosition));
				i++;
			}

			if(touch.phase == TouchPhase.Ended)
			{
				/* Remove Line

				EndPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);


				lineRenderer.SetVertexCount(0);
				i = 0;
			}
			*/


	}

	float DetermineSwipeAngle(Vector3 Start, Vector3 End)
	{
		float Angle = Mathf.Atan2(End.y - Start.y, End.x - Start.x) * 180 / Mathf.PI;

		print(Angle + " angle");
		return Angle;
	}

	void DetermineSlash(float Angle)
	{
		if(Angle < 45f && Angle > -45f)
		{
			playerController.Thrustbool = true;
		}
		else if (Angle >= 45f && Angle < 90f)
		{
			playerController.High = true;
		}
		else if (Angle <= -45 && Angle > -90)
		{
			playerController.Low = true;
		}
		else
		{
			playerController.Thrustbool = false;
			playerController.High = false;
			playerController.Low = false;
		}
	}

	//<summary>
	//Create a new trail at the given position
	//</summary>
	public static GameObject MakeTrail(Vector3 position)
	{
		GameObject trail = Instantiate(instance.trailPrefab) as GameObject;
		trail.transform.position = position;

		return trail;
	}
}
