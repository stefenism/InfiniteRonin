using UnityEngine;
using System.Collections;

public class ForeGroundGenerator : MonoBehaviour {

	public GameObject ForeGroundRocks;
	public GameObject Lantern;
	public GameObject Mountains;
	public GameObject farMountains;

	public Vector3 LeftBorder;
	public Vector3 RightBorder;

	public float yPoint;

	private float Marker;
	private float MountainMarker;
	private float farMountainMarker;
	private GameObject ForeGroundSprite = null;
	//private Vector3 RightBorderOffset;
	private GameObject Clone;
	private GameObject LanternClone;
	private GameObject MountainClone;

	public float mountainXOffset = 280f;

	// Use this for initialization
	void Start () {

		//RightBorderOffset = RightBorder + new Vector3(80f, 0f, 0f);
		MountainMarker = -250;
		farMountainMarker = -400f;
		Marker = -48;
		yPoint = yPoint -6f;

	}

	// Update is called once per frame
	void LateUpdate () {

		SpawnForeGround();
		//print("RightBorder " + RightBorder);
		//print("Marker: " + Marker);

	}

	void SpawnForeGround()
	{
		if(Marker < RightBorder.x + 132f)
		{
			//for(int i = 0; i < (RightBorder.x + 180f); i += 48)
			//{
				Clone = Instantiate(ForeGroundRocks, new Vector3(Marker, yPoint + Random.Range(-1,1), -8), Quaternion.identity) as GameObject;

				if(Random.Range(0,100) < 60)
				{
					LanternClone = Instantiate(Lantern, new Vector3(Marker + Random.Range(-15,-20), yPoint + Random.Range(0, -1), -7), Quaternion.identity) as GameObject;
				}
				Marker += 35;
			//}
		}
		if(MountainMarker < RightBorder.x + 500f)
		{
			MountainClone = Instantiate(Mountains, new Vector3(MountainMarker, yPoint + Random.Range(5,6), 2f), Quaternion.identity) as GameObject;
			MountainClone.transform.localScale = new Vector3(6f,6f,1f);

			MountainMarker += 115f;
		}

		if(farMountainMarker < RightBorder.x + 600f)
		{
		//2nd layer
		MountainClone = Instantiate(farMountains, new Vector3(farMountainMarker + mountainXOffset, yPoint + Random.Range(25f,35f), 7f), Quaternion.identity) as GameObject;
		MountainClone.transform.localScale = new Vector3(8f,8f,1f);

		farMountainMarker += 160f;
		}
	}
}
