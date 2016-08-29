using UnityEngine;
using System.Collections;

public class SpearmanBehaviour : MonoBehaviour {

	private PlayerController player;
	public LayerMask layerMask;

	public GameObject rayOrigin;
	public GameObject spear;
	private BoxCollider spearLlider;

	public float seenRange;
	public float chargeRange;
	public float runRange;
	public float speed;

	private bool run;
	private bool charge;
	private bool seen;
	private bool Dying;

	private Rigidbody rigidbody;
	private SprayBlood sprayBlood;
	private BoxCollider boxCollider;
	private SphereCollider sphereCollider;
	private PlaySounds playSounds;
	private AudioSource audioSource;

	public SillhouetteShader sillhouetteShader;
	public BoxCollider attackBoxCollider;

	private Animator anim;

	// Use this for initialization
	void Start () {

		//access them functions and components
		rigidbody = GetComponent<Rigidbody>();
		sprayBlood = GetComponent<SprayBlood>();
		boxCollider = GetComponent<BoxCollider>();
		anim = GetComponent<Animator>();
		sphereCollider = GetComponent<SphereCollider>();
		playSounds = GetComponent<PlaySounds>();
		audioSource = GetComponent<AudioSource>();

		player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

		seen = false;
		run = false;
		charge = false;
		Dying = false;

		seenRange = Random.Range(50f, 65f);

		//spearLlider = spear.GetComponent<BoxCollider>();

	}

	// Update is called once per frame
	void Update () {

		if(!player.IsDead)
		{
			TargetDetect();
		}


		if(run)
		{
			StartRunning();
		}

		if(charge)
		{
			Charge();
		}

		if(seen)
		{
			Ready();
		}


	}


	void TargetDetect(){


		Vector3 direction = (player.transform.position - rayOrigin.transform.position);
		//ray is from, to, distance
		RaycastHit hit;
		Ray targetRay = new Ray(rayOrigin.transform.position, direction);

		Debug.DrawRay(rayOrigin.transform.position, direction);


		if (Physics.Raycast(targetRay, out hit, seenRange, layerMask))
		{

			print("current distance from spearman " + hit.distance);
			//now check for distance
			if(hit.distance <= seenRange && hit.distance >= runRange)
			{
				//trigger ready anim
				seen = true;
			}
			if(hit.distance <= runRange && hit.distance >= chargeRange)
			{
				run = true;
				//StartRunning(direction);
			}
			if(hit.distance <= chargeRange)
			{
				charge = true;
				//Charge(direction);
			}
		}
	}

	void Ready()
	{
		anim.SetTrigger("Ready");
	}

	void StartRunning()
	{
		rigidbody.velocity = new Vector3(-speed, rigidbody.velocity.y, 0f);
		run = false;
		anim.SetBool("CanSee", true);
	}

	void Charge()
	{
		rigidbody.AddForce((-Vector3.right * (speed + speed)), ForceMode.VelocityChange);
		charge = false;
		anim.SetBool("Charging", true);
		anim.SetBool("CanSee", false);
	}


	void OnTriggerEnter(Collider collider)
	{
		if(collider.gameObject.tag == "Sword" || collider.gameObject.tag == "ClearScreen")
			{

				playSounds.PlaySound(0);
				playSounds.PlaySound(2);
				player.kills += 1;
				print("kills: " + player.kills);
				Dying = true;
				boxCollider.enabled = false;
				sphereCollider.enabled = false;
				rigidbody.isKinematic = true;
				attackBoxCollider.enabled = false;
				//anim.SetBool("dead",true);
				StartCoroutine(PlaySoundWait(1.5f));
				//Destroy(this.gameObject);
				sprayBlood.FireBloodParticles();
			}
			else if(collider.gameObject.tag == "Platform" || collider.gameObject.tag == "Pitfall")
			{
				Dying = true;
				boxCollider.enabled = false;
				sphereCollider.enabled = false;
				rigidbody.isKinematic = true;
				attackBoxCollider.enabled = false;
				//anim.SetBool("dead", true);
				sprayBlood.FireBloodParticles();
				Destroy(this.gameObject);
			}

			else if(collider.gameObject.tag == "Dojo")
			{
				sillhouetteShader.inDojo = true;
			}
	}


	void OnTriggerExit(Collider collision)
	{
		if(collision.gameObject.tag == "Dojo")
		{
			sillhouetteShader.inDojo = false;
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Destroyer")
		{
			Destroy(this.gameObject);
		}
	}

	private IEnumerator PlaySoundWait(float Time)
    {
    	yield return new WaitForSeconds(Time);
    	Destroy(this.gameObject);
    }

}
