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

	private PlayerController player;
	public LayerMask layerMask;

	public GameObject arrow;
	public GameObject spawnPoint;
	private GameObject clone;

	private float fireRate = 1f;
	public float fireSpeed = 50f;
	public float range = 75f;
	public float minRange = 27;
	private float shootAngle;

	private bool withinRange;
	private bool shortRange;
	private bool facingRight;
	private bool canShoot;

	private Rigidbody rigidbody;
	private SprayBlood sprayBlood;
	private BoxCollider boxCollider;

	// Use this for initialization
	void Start () {
		withinRange = false;
		shortRange = false;
		facingRight = false;
		canShoot = true;

		player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

		sprayBlood = GetComponent<SprayBlood>();
		boxCollider = GetComponent<BoxCollider>();
		rigidbody = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update () {

		TargetDetect();

		if((withinRange || shortRange) && canShoot)
		{
			if(!player.IsDead){
				FireAtWill();
			}

		}

		LookAtPlayer();

	}


	void TargetDetect()
	{
		RaycastHit hit;
		Ray targetRay = new Ray(spawnPoint.transform.position, player.transform.position - spawnPoint.transform.position);

		Debug.DrawRay(spawnPoint.transform.position, player.transform.position - spawnPoint.transform.position);


		//print("angle from archer to player: " + shootAngle);
		//print("why: " + (player.transform.position.y - spawnPoint.transform.position.y));
		//print("ex: " + (new Vector3 (player.transform.position.x - spawnPoint.transform.position.x, 0f, 0f).magnitude));

		/*
			After Much hardship and deliberation, when the archer is in the tower we want the ronin to be somewhere
			between 45 and 27 distance (hit.distance) for the arrows to seem realistic.  Perhaps on the ground the
			arrows will follow a more direct route.

			This needs to be altered but for now this will do.
		*/

		if(Physics.Raycast(targetRay, out hit, range, layerMask))
		{
			print("hit distance " + hit.distance);
			if((hit.distance <= range) && (hit.distance >= minRange))
			{
				print("In Range");
				shootAngle = determineAngle();
				spawnPoint.transform.eulerAngles = new Vector3(0f,0f,shootAngle - 45f);
				print("shoot Angle " + shootAngle);
			}
		}

	}

	float determineAngle()
	{
		Vector3 targetTransform = player.transform.position;
		Vector3 spawnTransform = spawnPoint.transform.position;

		float y = targetTransform.y - spawnTransform.y;
		targetTransform.y = spawnTransform.y = 0;
		float x = (targetTransform.x - spawnTransform.x);
		float v = fireSpeed;
		float g = Physics.gravity.y;
		float sqrt = (v*v*v*v) - (g * (g * (x * x) + (2 * y * (v * v))));

		if(sqrt <= 0)
		{
			Debug.Log("No Solution, sqrt = " + sqrt);
			withinRange = false;
			shortRange = true;
			return 0;
		}

		sqrt = Mathf.Sqrt(sqrt);
		float calculatedAngle = Mathf.Atan2((v*v) + sqrt, g*x);

		withinRange = true;
		shortRange = false;
		return (calculatedAngle * Mathf.Rad2Deg);

	}

	void FireAtWill()
	{
		Rigidbody cloneRB;

		if(fireRate <= 0f)
		{
			//instantiate an arrow at speed at an arc vector toward player
			clone = Instantiate(arrow, spawnPoint.transform.position, Quaternion.identity) as GameObject;
			cloneRB = clone.gameObject.GetComponent<Rigidbody>();

			if(withinRange)
			{
				cloneRB.AddForce((BallisticVel(player.transform, 45f)), ForceMode.VelocityChange);
			}
			else if(shortRange)
			{
				cloneRB.AddForce((player.transform.position - spawnPoint.transform.position) * fireSpeed, ForceMode.VelocityChange);
			}
			//cloneRB.AddForce((BallisticVel(player.transform, 45f)), ForceMode.VelocityChange);
			//cloneRB.AddForce(-transform.right * transform.localScale.x * fireSpeed, ForceMode.VelocityChange);
			cloneRB.rotation = Quaternion.LookRotation(new Vector3 (0f,0f,shootAngle));
			//reset fireRate;
			fireRate = 1f;
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
		dir.y = 0;
		float dist = dir.magnitude; //get horizontal distance
		float angleRad = angle * Mathf.Deg2Rad;  //convert angle to radians

		dir = new Vector3 (dir.x, dist * Mathf.Tan(angleRad), 0f);
		 //set dir to elevation angle
		dist += height/Mathf.Tan(angleRad);


		//dir = new Vector3 (dir.x, dist * Mathf.Tan(angle), 0f); //set dir to elevation angle
		//dist += height / Mathf.Tan(angle); //correct for small height differences

		//calculate the velocity magnitude
		if(dist < 1)
		{
			dist = 1;
		}
		float sqrt = (dist * Physics.gravity.magnitude) / Mathf.Sin(2 * angleRad);

		if(sqrt < 0)
		{
			sqrt = 1;
		}

		float vel = Mathf.Sqrt(sqrt);

		//float vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * angleRad));
		vel += Random.Range(-5,5);
		print("velocity " + vel);
		//print("arrow velocity " + vel + " arrow direction " + (vel * dir.normalized) + " direction " + dir.normalized);

		return vel * dir.normalized;
	}

	void LookAtPlayer()
	{
		if((player.transform.position.x > transform.position.x) && !facingRight)
		{
			Flip();
		}
	}
	void Flip()
	{
		facingRight = !facingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void OnTriggerEnter(Collider collision)
	{
		if(collision.gameObject.tag == "Sword")
		{
			player.kills += 1;
			print("kills: " + player.kills);
			boxCollider.enabled = false;
			rigidbody.isKinematic = true;
			sprayBlood.FireBloodParticles();
			canShoot = false;
		}
	}
}
