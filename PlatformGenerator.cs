using UnityEngine;
using System.Collections;

public class PlatformGenerator : MonoBehaviour {


	public GameObject[] Platforms;
	public Vector3 SpawnPoint;
	private int RandomPlatform;

	private GameObject Clone;
	private int Probability;

	private float Timer;
	private float SpawnWait;
	private float SpawningWait;

	private bool Spawning;
	private bool spawnCombo;

	public bool GameOn;

	public GameObject LandGroup;
	public EnemyGenerator enemyGenerator;

	private Transform EndSpawn;

	public bool dojoSpawn;
	public bool towerSpawn;
	public bool bridgeSpawn;
	public bool pitfallSpawn;



	// Use this for initialization
	void Start () {


			GameOn = false;
			spawnCombo = false;

			Timer = Random.Range(1,10);

			SpawnWait = 30f;
			SpawningWait = 1000f;

			dojoSpawn = false;

	}

	// Update is called once per frame
	void LateUpdate () {

		//if(GameOn)
		//{
			//SpawnCheck();
			//print(Timer + " current platform timer");
		//}

	}

	void SpawnCheck()
	{

		Probability = Random.Range(0,100);
		RandomPlatform = Random.Range(1, Platforms.Length);
		//print(Probability + " probability");
		if (Timer <= 0f)
		{
			if(Probability >= 0 && Probability < 30)
			{
				Timer = SpawnWait;
			}
			else if(Probability >= 30 && Probability <= 100)
			{

					Timer = SpawningWait;
					if(!Spawning && !dojoSpawn)
					{
						Spawning = true;
						enemyGenerator.GameOn = false;
						Spawn(Platforms[RandomPlatform]);
						Timer = SpawnWait;
					}

				}

			}
			Timer -= Time.deltaTime;
		}

		void Spawn(GameObject platform)
		{

			Clone = Instantiate(platform, new Vector3(SpawnPoint.x, SpawnPoint.y, SpawnPoint.z), Quaternion.identity) as GameObject;
			Clone.transform.parent = LandGroup.transform;
			//Destroy(Clone,10f);

			Spawning = false;
			enemyGenerator.SpawnPoint = this.gameObject.transform.GetChild(0).position;
			StartCoroutine(Waiting(3));

		}

		void SpawnCombo(GameObject platform)
		{

			if(spawnCombo)
			{
				for(int i = 0; i < Random.Range(1,4); i++)
				{
					if(i > 0)
					{
						SpawnPoint = new Vector3(SpawnPoint.x + 20f, SpawnPoint.y + 35f, SpawnPoint.z);
					}

					Spawn(platform);
				}
				spawnCombo = false;
				Spawning = false;
				//Timer = SpawnWait;
			}
		}

		public void SpawnDojo()
		{
			Spawn(Platforms[0]);
			Timer = SpawnWait;
			dojoSpawn = false;

		}

		public void SpawnTower()
		{
			//Spawn(Platforms[1]);
			spawnCombo = true;
			SpawnCombo(Platforms[1]);
			Timer = SpawnWait;
			towerSpawn = false;
		}

		public void SpawnBridge()
		{
			Spawn(Platforms[2]);
			Timer = SpawnWait;
			bridgeSpawn = false;
		}

		public void SpawnPitfall()
		{
			Spawn(Platforms[3]);
			Timer = SpawnWait;
			pitfallSpawn = false;
		}

		private IEnumerator Waiting(float seconds)
		{
			yield return new WaitForSeconds(seconds);

			enemyGenerator.GameOn = true;
		}
}
