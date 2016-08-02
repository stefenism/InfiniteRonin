using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {


	public float xMargin = 3f;
	public float yMargin = 1f;
	public float xSmooth = 8f;
	public float ySmooth = 8f;
	public Vector2 maxXAndY;
	public Vector2 minXAndY;

	private Transform player;
	private Transform camera;
	public Vector3 maxCameraPosition;
	public Vector3 minCameraPosition;
	private bool bounds = true;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player").GetComponent<PlayerController>().transform;
		camera = GetComponent<Transform>();
	}

	bool CheckXMargin()
	{
		return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
	}

	bool CheckYMargin()
	{
		return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
	}
	// Update is called once per frame
	void FixedUpdate () {
		TrackPlayer();

		if (bounds)
		{
				Vector3 newPosition = new Vector3(minCameraPosition.x + player.position.x , minCameraPosition.y + player.position.y, camera.position.z);
				//camera.position = new Vector3. (Mathf.Clamp(camera.position.x, minCameraPosition.x + player.position.x, maxCameraPosition.x + player.position.x),
				camera.position = Vector3.Lerp(camera.position,newPosition, .05f);
				//Mathf.Clamp(camera.position.y, minCameraPosition.y + player.position.y, maxCameraPosition.y + player.position.y),
				//Mathf.Clamp(camera.position.z, minCameraPosition.z, maxCameraPosition.z));
		}
	}

	void TrackPlayer()
	{
		float targetX = transform.position.x;
		float targetY = transform.position.y;

		if(CheckXMargin())
		{
			targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);
		}

		if(CheckYMargin())
		{
			targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);
		}

		Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
		Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

		Vector3 targetPosition = new Vector3(targetX, targetY, transform.position.z);

		transform.position = Vector3.Lerp(transform.position, targetPosition, .1f);
	}
}
