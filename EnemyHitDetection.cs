using UnityEngine;
using System.Collections;

public class EnemyHitDetection : MonoBehaviour {

	private SprayBlood sprayBlood;

	private Animator anim;
	public string SwordType;
	//private bool CorrectSwordType;

	public bool inAttackRange;

	private PlayerController player;

	private PlaySounds playSounds;
	private AudioSource audioSource;

	private bool Dying;
	private BoxCollider boxCollider;
	private Rigidbody rigidbody;
	public AttackRange attackRange;
	public SillhouetteShader sillhouetteShader;

	// Use this for initialization

	void Awake()
	{
		sprayBlood = GetComponent<SprayBlood>();
		anim = GetComponent<Animator>();
		playSounds = GetComponent<PlaySounds>();
		audioSource = GetComponent<AudioSource>();
		boxCollider = GetComponent<BoxCollider>();
		rigidbody = GetComponent<Rigidbody>();
		//attackRange = GetComponent<AttackRange>();

		Dying = false;

	}
	void Start () {


		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

	}

	// Update is called once per frame
	void Update () {

		if(inAttackRange && !Dying)
		{
			inAttackRange = false;
			anim.SetBool("slashing", true);
			StartCoroutine(AnimTime(1f));
			playSounds.PlaySound(1);

		}

	}

	void OnTriggerEnter(Collider collider)
	{
		if(collider.gameObject.tag == SwordType || collider.gameObject.tag == "ClearScreen")
		{

			playSounds.PlaySound(0);
			playSounds.PlaySound(2);
			player.kills += 1;
			print("kills: " + player.kills);
			Dying = true;
			boxCollider.enabled = false;
			rigidbody.isKinematic = true;
			attackRange.AttackRangeCollider.enabled = false;
			anim.SetBool("dead",true);
			StartCoroutine(PlaySoundWait(1.5f));
			//Destroy(this.gameObject);
			sprayBlood.FireBloodParticles();
		}
		else if(collider.gameObject.tag == "Platform" || collider.gameObject.tag == "Pitfall")
		{
			Dying = true;
			boxCollider.enabled = false;
			rigidbody.isKinematic = true;
			attackRange.AttackRangeCollider.enabled = false;
			anim.SetBool("dead", true);
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
		if(GetComponent<Collider>().gameObject.tag == "Dojo")
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


	private IEnumerator AnimTime(float Time)
    {
    	yield return new WaitForSeconds(Time);
    	anim.SetBool("slashing", false);
    }

    private IEnumerator PlaySoundWait(float Time)
    {
    	yield return new WaitForSeconds(Time);
    	Destroy(this.gameObject);
    }

}
