using UnityEngine;
using System.Collections;

public class JumperaiBehaviour : MonoBehaviour {
	private PlayerController player;
	public LayerMask layerMask;

	public GameObject rayOrigin;
	public GameObject groundDetectOrigin;

	public float seenRange;
	public float jumpRange;
	public float speed;
	public float jumpForce;
	public float jumpTime;
	public float groundDistance;

	public float jumpDuration;

	private float modifier;

	private bool run;
	private bool jump;
	private bool jumping;
	private bool grounded;

	private Rigidbody rigidbody;
	private SprayBlood sprayBlood;
	//private BoxCollider boxCollider;

	// Use this for initialization
	void Start () {

		rigidbody = GetComponent<Rigidbody>();
		sprayBlood = GetComponent<SprayBlood>();
		//boxCollider = GetComponent<BoxCollider>();

		player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

		run = false;
		jump = false;
		grounded = false;

	}

	// Update is called once per frame
	void Update () {

		GroundDetect();
		Debug.Log("grounded: " + grounded);
		TargetDetect();

		if(run)
		{
			StartRunning();
		}

		if(jump)
		{
			//StartCoroutine(jumping());
			jumping = true;
			JumpAttack();
		}

	}



	void GroundDetect(){

		RaycastHit hit;

		Ray landingRay = new Ray(groundDetectOrigin.transform.position, Vector3.down);

		Debug.DrawRay(groundDetectOrigin.transform.position, Vector3.down * groundDistance);

		//if(!player.Grounded)
		//{
			if(Physics.Raycast(landingRay, out hit, groundDistance))
			{
				//print(hit.collider.tag + " collided tag");
				if(hit.collider.tag == "Ground")
				{
					grounded = true;
					//player.anim.SetBool("jumping", false);
				}
			}
			else
			{
				grounded = false;
				//player.anim.SetBool("jumping", true);
			}
	}
	void TargetDetect(){


		Vector3 direction = (player.transform.position - rayOrigin.transform.position);
		//ray is from, to, distance
		RaycastHit hit;
		Ray targetRay = new Ray(rayOrigin.transform.position, direction);

		Debug.DrawRay(rayOrigin.transform.position, direction);


		if(!player.IsDead){
			if (Physics.Raycast(targetRay, out hit, seenRange, layerMask))
			{

				print("current distance from spearman " + hit.distance);
				//now check for distance
				if(hit.distance <= seenRange && hit.distance >= jumpRange)
				{
					run = true;
					//StartRunning(direction);
				}
				if(hit.distance <= jumpRange && grounded)
				{
					jump = true;
					//JumpAttack();
				}
			}
		}

	}


	void StartRunning()
	{
		rigidbody.velocity = new Vector3(-speed, rigidbody.velocity.y, 0f);
		run = false;
	}

	void JumpAttack()
	{
		Debug.Log("jumping now");
		modifier = Random.Range(-2,5);
		if(jumpTime > 0)
		{
			rigidbody.AddForce(Vector3.up * (jumpForce + modifier), ForceMode.VelocityChange);
			jumpTime--;
		}
		else
		{
			jumping = false;
			jump = false;
			jumpTime = jumpDuration;
			//rigidbody.AddForce(-Vector3.up * Physics.gravity.y);// = new Vector3(-speed, Physics.gravity.y, 0f);
		}
	}

	Vector3 BallisticVel(Transform target, float angle)
	{

		Vector3 dir = target.position - rayOrigin.transform.position; //get target position
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

		return (vel + jumpForce) * dir.normalized;
	}


}
