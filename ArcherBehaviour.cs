/*************************************************************************

	In this script I'm going to define Archer Behaviour.  First things first
	The archer will always be pointing a Raycast (as long as they are alive)
	At the Player until the Player is within a certain distance of the Archer
	In which case the archer will start shooting UNTIL the Player is a certain
	Distance away or is Destroyed.

	The Arrows will be deflectable(sp?) by the player and will likely stick
	into the ground if they miss the player.

**************************************************************************/

using UnityEngine;
using System.Collections;

public class ArcherBehaviour : MonoBehaviour {

	public PlayerController player;
	public LayerMask layerMask;

	public GameObject arrow;
	public GameObject spawnPoint;
	private GameObject clone;

	private float fireRate = 1f;
	private float fireSpeed = 15f;
	public float range = 25f;
	private float shootAngle = 30f;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		TargetDetect();
	}


	void TargetDetect()
	{
		RaycastHit hit;
		Ray targetRay = new Ray(player.transform.position, spawnPoint.transform.position);

		Debug.DrawRay(player.transform.position, spawnPoint.transform.position);

		if(Physics.Raycast(targetRay, out hit, range, layerMask))
		{
			if(hit.distance <= range)
			{
				FireAtWill();
			}
		}

	}

	void FireAtWill()
	{
		Rigidbody cloneRB;

		if(fireRate <= 0f)
		{
			//instantiate an arrow at speed at an arc vector toward player
			clone = Instantiate(arrow, spawnPoint.transform.position, Quaternion.identity) as GameObject;
			cloneRB = GetComponent<Rigidbody>();

			cloneRB.velocity = BallisticVel(player.transform, shootAngle) * fireSpeed;
			//reset fireRate;
		}
		else
		{
			fireRate -= Time.deltaTime;
		}
	}

	Vector3 BallisticVel(Transform target, float angle)
	{
		Vector3 dir = target.position - spawnPoint.transform.position; //get target position
		float height = dir.y; //get height difference
		float dist = dir.magnitude; //get horizontal distance
		float angleRad = angle * Mathf.Deg2Rad;  //convert angle to radians
		dir = new Vector3 (dir.x, dist * Mathf.Tan(angleRad), 0f); //set dir to elevation angle
		dist += height / Mathf.Tan(angleRad); //correct for small height differences

		//calculate the velocity magnitude
		float vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * angleRad));

		return vel * dir.normalized;
	}
}
