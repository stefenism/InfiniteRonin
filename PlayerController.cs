﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	private Rigidbody rb;

	[HideInInspector]
	public Animator anim;

	private PlaySounds playSounds;

	private bool Attacking;
	private bool Running;
	private bool Jumping;
	private bool CanvasDisplay;
	private bool CanControl;
	[HideInInspector]
	public bool IsDead;
	public bool invincible;

	private bool CanTap;

	[HideInInspector]
	public bool Tapped;
	[HideInInspector]
	public bool StartTap;
	[HideInInspector]
	public bool Low;
	[HideInInspector]
	public bool Thrustbool;
	[HideInInspector]
	public bool High;
	[HideInInspector]
	public bool JumpBool;

	public GameObject UIPanel;
	public GameObject EndGamePanel;
	public GameObject HUD;

	public float speed;
	public float JumpForce;
	public float jumpTime;
	[HideInInspector]
	public float airJump;
	public BoxCollider slashBox;
	public BoxCollider sliceBox;
	public BoxCollider thrustBox;
	public BoxCollider ClearScreenBox;
	public BoxCollider DownThrustBox;
	public BoxCollider UpThrustBox;

	public GameObject PhysicalLandGroup;
	public GameObject SpiritualLandGroup;
	private GameObject CurrentLandGroup;

	public Vector3 PhysicalLandGroupHeight;
	public Vector3 SpiritualLandGroupHeight;

	public float kills;
	public float distance;

	public Light PhysicalLight;
	public Light SpritualLight;
	//private float Timer;

	public ParticleSystem SlashParticle;
	public ParticleSystem LowSlashParticle;
	public ParticleSystem ThrustParticle;
	public GameObject particleSpawn;
	public EnemyGenerator enemyGenerator;
	public PlatformGenerator platformGenerator;
	public SillhouetteShader sillhouetteShader;
	public PlayerHealth playerHealth;
	private SpriteRenderer renderer;

	public Text KillCount;

	public Transform GroundCheck;
	float GroundRadius = 0.2f;
	public LayerMask WhatIsGround;
	public bool Grounded;

	private float previousPos;
	private bool isMoving;



	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		playSounds = GetComponent<PlaySounds>();
		playerHealth = GetComponent<PlayerHealth>();
		renderer = GetComponent<SpriteRenderer>();

		/*  These are the Variables to reset once you have touch working note on: 6/21/16 */
		Attacking = false;
		Running = false;
		Jumping = false;
		CanvasDisplay = true;
		CanControl = false;
		CanTap = true;
		IsDead = true;
		isMoving = false;
		invincible = false;

		Grounded = false;
		/*  These are the Variables to reset once you have touch working note on: 6/21/16 */
		/*
		Attacking = false;
		Running = true;
		CanvasDisplay = false;
		CanControl = true;
		*/

		rb.velocity = new Vector3(rb.velocity.x + speed,rb.velocity.y,0f);

		CurrentLandGroup = PhysicalLandGroup;

		kills = 0;
		distance = 0;

		anim.SetBool("dead", true);


		previousPos = transform.position.x;




	}



	// Update is called once per frame
	void Update () {

		if(Running)
		{
			Run();
		}


		if(Jumping)
		{
			Jump();
		}


		if(!Attacking)
		{
			if(CanControl)
			{
				Slash();
				Lowslash();
				Thrust();
				Jump();
				DownThrust();
				UpThrust();
			}

		}

		CanvasDisplayer();
		CheckMovement();

		if(isMoving)
		{
			UpdateDistance();
		}

		anim.SetFloat("Yspeed", rb.velocity.y);



		//SwitchWorlds(CurrentLandGroup);

	}

	void Run()
	{
		if(rb.position.z != 0)
		{
			rb.position = new Vector3(rb.position.x, rb.position.y, 0f);
		}
		rb.velocity = new Vector3(speed,rb.velocity.y,0f);
		anim.SetFloat("speed", speed);


	}

	void Slash()
	{
		if(Grounded && (Input.GetKeyDown(KeyCode.W) || High))
		{
			slashBox.enabled = true;
			anim.SetBool("slashing", true);
			SlashForward(2f);

			Vector3 position = particleSpawn.transform.position + new Vector3(-1f,-1.25f,-.1f);
			ParticleSystem localSlashObj = GameObject.Instantiate(SlashParticle, position, SlashParticle.transform.rotation) as ParticleSystem;
			localSlashObj.transform.parent = transform;
			localSlashObj.Play();


			StartCoroutine(SlashTime(.25f));
			StartCoroutine(AnimTime(.001f));
			StartCoroutine(ParticleTime(.2f,localSlashObj));

			playSounds.PlaySound(0);

			High = false;


		}

	}

	void Lowslash()
	{
		if((Input.GetKeyDown(KeyCode.S) || Low) && Grounded)
		{
			sliceBox.enabled = true;
			anim.SetBool("lowslashing", true);
			SlashForward(1f);

			Vector3 position = particleSpawn.transform.position - new Vector3(2.5f, 3f, 0f);
			ParticleSystem localSlashObj = GameObject.Instantiate(LowSlashParticle, position, LowSlashParticle.transform.rotation) as ParticleSystem;
			localSlashObj.transform.parent = transform;
			localSlashObj.Play();

			StartCoroutine(SlashTime(.25f));
			StartCoroutine(AnimTime(.001f));
			StartCoroutine(ParticleTime(.2f,localSlashObj));

			playSounds.PlaySound(2);

			Low = false;
		}
	}

	void Thrust()
	{
		if(Input.GetKeyDown(KeyCode.D) || Thrustbool)
		{
			thrustBox.enabled = true;
			anim.SetBool("thrusting", true);
			SlashForward(8f);

			Vector3 position = particleSpawn.transform.position;// + new Vector3(3f,0,-.1f);
			ParticleSystem localSlashObj = GameObject.Instantiate(ThrustParticle, position, ThrustParticle.transform.rotation) as ParticleSystem;
			localSlashObj.Play();


			StartCoroutine(SlashTime(.25f));
			StartCoroutine(AnimTime(.001f));
			StartCoroutine(ParticleTime(.2f,localSlashObj));

			playSounds.PlaySound(1);

			Thrustbool = false;
		}
	}

	void DownThrust()
	{
		if(!Grounded && (Input.GetKeyDown(KeyCode.S) || Low))
		{
			DownThrustBox.enabled = true;
			//anim.SetBool("downThrust", true);
			anim.SetBool("lowslashing", true);

			SlashDown(75f);

			Vector3 position = particleSpawn.transform.position - new Vector3(2.5f, 3f, 0f);
			ParticleSystem localSlashObj = GameObject.Instantiate(LowSlashParticle, position, LowSlashParticle.transform.rotation) as ParticleSystem;
			localSlashObj.transform.parent = transform;
			localSlashObj.Play();

			StartCoroutine(SlashTime(.3f));
			StartCoroutine(AnimTime(.001f));
			StartCoroutine(ParticleTime(.2f,localSlashObj));

			playSounds.PlaySound(2);

			Low = false;
		}

		if(Grounded)
		{
			DownThrustBox.enabled = false;
		}
	}

	void UpThrust()
	{
		if(!Grounded && (Input.GetKeyDown(KeyCode.W) || High))
		{
			UpThrustBox.enabled = true;
			//anim.SetBool("upThrust", true);
			anim.SetBool("slashing", true);

			//SlashUp(30f);

			Vector3 position = particleSpawn.transform.position - new Vector3(2.5f, 3f, 0f);
			ParticleSystem localSlashObj = GameObject.Instantiate(LowSlashParticle, position, LowSlashParticle.transform.rotation) as ParticleSystem;
			localSlashObj.transform.parent = transform;
			localSlashObj.Play();

			StartCoroutine(SlashTime(.3f));
			StartCoroutine(AnimTime(.001f));
			StartCoroutine(ParticleTime(.2f,localSlashObj));

			playSounds.PlaySound(2);

			High = false;
		}
	}

	void SlashForward(float distance)
	{
		rb.AddForce(Vector3.right * speed * distance, ForceMode.VelocityChange);
		//rb.velocity = new Vector3(rb.velocity.x + distance, rb.velocity.y, 0f);
	}

	void SlashDown(float speed)
	{
		//rb.transform.position = new Vector3(rb.transform.position.x, rb.transform.position.y  - distance, 0f);
		rb.AddForce(Vector3.down * speed, ForceMode.VelocityChange);
	}

	void SlashUp(float speed)
	{
		rb.AddForce(Vector3.up * speed, ForceMode.VelocityChange);
	}

	void Jump()
	{
		if(JumpBool && (Grounded || airJump > 0))
		{
			if((!IsDead && Tapped))
			{

				if(jumpTime > 0f)
				{
					rb.AddForce (transform.up * JumpForce, ForceMode.VelocityChange);
					print("jumptime " + jumpTime);
					jumpTime -= .2f;
				}
				//rb.velocity = new Vector3(rb.velocity.x, JumpForce, 0f);
				else
				{
					JumpBool = false;
					Grounded = false;
				}
				anim.SetBool("jumping", true);

			}

		}

		else if(Grounded)
		{

			if(!IsDead && Input.GetKeyDown(KeyCode.Space))
			{
				rb.velocity = new Vector3(rb.velocity.x, JumpForce, 0f);
				Grounded = false;
				anim.SetBool("jumping", true);
			}
		}
	}

	void CanvasDisplayer()
	{
		if(CanvasDisplay && IsDead)
		{
			if((Input.GetKeyDown(KeyCode.X) || StartTap) && CanTap)
			{
				anim.SetBool("dead", false);
				anim.SetBool("alive", true);
				IsDead = false;
				UIPanel.SetActive(false);
				EndGamePanel.SetActive(false);
				HUD.SetActive(true);

				kills = 0f;
				distance = 0;
				ClearScreenBox.enabled = true;
				StartCoroutine(ReviveWait(1f));

				StartTap = false;

			}

		}
	}

	void UpdateDistance()
	{
		distance += ((rb.velocity.x / speed) / 3);
	}

	void CheckMovement()
	{
		if((int)previousPos < (int)transform.position.x)
		{
			isMoving = true;
		}
		else if((int)previousPos == (int)transform.position.x)
		{
			isMoving = false;
		}
		previousPos = transform.position.x;
	}

	public void JumpButton()
	{
		Tapped = true;
		JumpBool = true;
		jumpTime = 1.2f;
		airJump--;
	}

	public void UnJumpButton()
	{
		Tapped = false;
		JumpBool = false;
	}



	private IEnumerator SlashTime(float Time)
    {
    	Attacking = true;
    	yield return new WaitForSeconds(Time);
    	slashBox.enabled = false;
    	thrustBox.enabled = false;
    	sliceBox.enabled = false;
    	Attacking = false;
			//DownThrustBox.enabled = false;
			UpThrustBox.enabled = false;
    	//anim.SetBool("slashing", false);
    }
    private IEnumerator AnimTime(float Time)
    {
    	yield return new WaitForSeconds(Time);
    	anim.SetBool("slashing", false);
    	anim.SetBool("lowslashing", false);
    	anim.SetBool("thrusting", false);

    }

    private IEnumerator ParticleTime(float Time, ParticleSystem particle)
    {
    	yield return new WaitForSeconds(Time);

    	Destroy(particle);
    }

    private IEnumerator ReviveWait(float Time)
    {
    	yield return new WaitForSeconds(Time);

    	Running = true;
			Jumping = true;
			enemyGenerator.GameOn = true;
			enemyGenerator.lastSpawnDistance = 0f;
			platformGenerator.GameOn = true;
			anim.SetFloat("speed", 1f);
			ClearScreenBox.enabled = false;
			CanControl = true;

			playerHealth.Health = playerHealth.MaxHealth;
    }

		private IEnumerator DeadWait(float Time)
		{
			Running = false;
			Jumping = false;
			enemyGenerator.GameOn = false;
			platformGenerator.GameOn = false;
			anim.SetBool("dead", true);
			EndGamePanel.SetActive(true);
			HUD.SetActive(false);
			CanControl = false;
			CanTap = false;
			IsDead = true;
			KillCount.text = kills + " Samurai Killed";

			yield return new WaitForSeconds(Time);

			CanTap = true;
		}

		private IEnumerator Invincibility(float duration, float blinkTime)
		{
			while(duration > 0f)
			{
				duration -= Time.deltaTime;

				//toggle renderer
				renderer.enabled = !renderer.enabled;

				yield return new WaitForSeconds(blinkTime);
			}

			renderer.enabled = true;
			invincible = false;
		}

    void OnCollisionEnter(Collision collision)
    {

			//print(collision.gameObject.tag + " currentcollision");
			if(!invincible)
			{
				if(collision.gameObject.tag == "Enemy")
	    	{
	    		/*Running = false;
	    		anim.SetBool("dead", true);
	    		EndGamePanel.SetActive(true);
	    		CanControl = false;
	    		IsDead = true;
	    		KillCount.text = kills + " Samurai Killed";
					*/

					playerHealth.DealDamage(1f);
					invincible = true;
					StartCoroutine(Invincibility(1f,0.05f));

					if (playerHealth.Health <= 0f)
					{
						StartCoroutine(DeadWait(3f));
					}
			}


    	}

			if(collision.gameObject.tag == "enemyWeapon")
			{

				if(!invincible)
				{
					playerHealth.DealDamage(1f);
					invincible = true;
					StartCoroutine(Invincibility(2f,0.2f));

					if (playerHealth.Health <= 0f)
					{
						StartCoroutine(DeadWait(3f));
					}
				}

			}

		}

			void OnTriggerEnter(Collider collision)
			{
				if(collision.gameObject.tag == "Dojo")
				{
					sillhouetteShader.inDojo = true;
				}

				else if(collision.gameObject.tag == "Pitfall")
				{
					StartCoroutine(DeadWait(2f));
					rb.transform.position = new Vector3(rb.transform.position.x + 5f, rb.transform.position.y + 25f, 0f);
				}


			}

			void OnTriggerExit(Collider collision)
			{
				if(collision.gameObject.tag == "Dojo")
				{
					sillhouetteShader.inDojo = false;
				}
			}

			/*
			//backup incase I can't figure out how to get player to smoothly walk up angles.
			else if(collision.gameObject.tag == "Platform")
			{
				rb.transform.position = new Vector3 (rb.transform.position.x, rb.transform.position.y + 2f, 0f);
			}
			*/


		/*
		void OnTriggerEnter(Collider collision)
		{
			if(collision.gameObject.tag == "Ground")
			{
				Grounded = true;
			}
		}

		void OnTriggerExit(Collider collision)
		{
			if(collision.gameObject.tag == "Ground")
			{
				Grounded = false;
			}
		}
		*/


}
