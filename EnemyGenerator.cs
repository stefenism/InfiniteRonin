using UnityEngine;
using System.Collections;

public class EnemyGenerator : MonoBehaviour {

	public GameObject[] Enemies;
	public Vector3 SpawnPoint;

	private GameObject Clone;
	private int Probability;

	private float SpawnWait;
	private float SpawnComboWait;
	private float SpawningWait;

	private int RandomEnemy;

	private float Timer;

	private bool SpawnCombo;
	private bool Spawning;

	public GameObject LandGroup;

	public bool GameOn;

	public PlayerController player;

	private float lastSpawnDistance;
	public float nextSpawn;

	// Use this for initialization
	void Start () {

		GameOn = false;

		lastSpawnDistance = 0f;
		nextSpawn = 10f;

		Timer = Random.Range(1,10);

		SpawnWait = 1.5f;
		SpawnComboWait = .5f;
		SpawningWait = 1000f;

		SpawnCombo = false;
		Spawning = false;

	}

	// Update is called once per frame
	void LateUpdate () {

		if(GameOn)
		{
			SpawnCheck();
		}


	}


	void SpawnCheck()
	{
		Probability = Random.Range(0,100);

		if(player.distance > lastSpawnDistance + nextSpawn)
		{
			if(Probability >= 0 && Probability < 30)
			{
				lastSpawnDistance = player.distance;
			}
			else if(Probability >= 30 && Probability <= 100)
			{
				if(Probability >= 30 && Probability < 50f)
				{
					lastSpawnDistance = player.distance;
					SpawnCombo = true;
					Spawning = true;
					if(SpawnCombo)
					{
						SpawnComboAttack();
					}
				}
				else if(Probability >= 50 && Probability <= 100)
				{
					lastSpawnDistance = player.distance;
					if(!SpawnCombo && !Spawning)
					{
						Spawning = true;
						Spawn();
						//Timer = SpawnWait;
					}
				}
			}
		}
	}


	void Spawn()
	{
		RandomEnemy = Random.Range(0, Enemies.Length);

		Clone = Instantiate(Enemies[RandomEnemy], new Vector3(SpawnPoint.x, SpawnPoint.y, SpawnPoint.z), Quaternion.identity) as GameObject;
		Clone.transform.parent = LandGroup.transform;
		//Destroy(Clone,10f);

		Spawning = false;

	}

	void SpawnComboAttack()
	{

		if(SpawnCombo)
		{
			for(int i = 0; i < Random.Range(2,5); i++)
			{
				if(i > 0)
				{
					SpawnPoint = new Vector3(SpawnPoint.x + 6f, SpawnPoint.y, SpawnPoint.z);
				}

				Spawn();
			}
			SpawnCombo = false;
			Spawning = false;
			//Timer = SpawnWait;
		}

		}

		private IEnumerator Waiting(float seconds)
		{
			yield return new WaitForSeconds(seconds);

			Timer = SpawnWait;
		}
	/*
	void SpawnCheck()
	{

		Probability = Random.Range(0,100);
		//print(Probability + " probability");
		if (Timer <= 0f)
		{
			if(Probability >= 0 && Probability < 30)
			{
				Timer = SpawnWait;
			}
			else if(Probability >= 30 && Probability <= 100)
			{

				if(Probability >= 30f && Probability < 50f)
				{
					Timer = SpawningWait;
					SpawnCombo = true;
					Spawning = true;
					if(SpawnCombo)
					{
						SpawnComboAttack();
					}
					//Timer = SpawningWait;
					//StartCoroutine(Waiting(3f));



				}
				else if(Probability >= 50f && Probability <= 100f)
				{
					Timer = SpawningWait;
					if(!SpawnCombo && !Spawning)
					{
						Spawning = true;
						Spawn();
						Timer = SpawnWait;
					}

				}

			}
		}

		Timer -= Time.deltaTime;
	}
	*/


}
