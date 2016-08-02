using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundGenerator : MonoBehaviour {


	private float StartingX;
	private float StartingY;
	private float DistZ;

	private Vector3 CurrentPoint;
	private Vector3 NewPoint;

	private Vector3 LeftBorder;
	private Vector3 RightBorder;

	private LineRenderer Line;

	private Camera gameCamera;
	//public CameraFollow cameraFollow;


	//reference to the mesh that I will generate
	private GameObject Mesh = null;
	private Mesh mesh = null;
	private MeshFilter filter = null;
	private MeshCollider meshCollider = null;
	private MeshRenderer meshRenderer = null;

	//the points along the top of the mesh
	//private Vector3[] points = null;
	private List<Vector3> points = new List<Vector3>();

	private Vector3 CurrentMeshPoint;
	private Vector3 NewMeshPoint;

	//lista for all the vertices and triangles of the mesh
	private List<Vector3> vertices = new List<Vector3>();
	private List<int> triangles = new List<int>();
	private List<Vector2> uvs = new List<Vector2>();

	private List<Vector3> colliderVertices = new List<Vector3>();
	private List<int> colliderTriangles = new List<int>();
	private int colliderCount;


	private float tUnitW = 314f;
	private float tUnitH = 1024f;

	private ForeGroundGenerator foreGroundGenerator;

	public Vector3 Marker;
	public Vector3 LaneTwoMarker;
	public EnemyGenerator enemyGenerator;
	public PlatformGenerator platformGenerator;

	public Color MeshColor;
	public GameObject LandGroup;

	public PlayerController player;

	public Texture2D textureTiles;
	public Material GroundTileMaterial;

	public float xSmoothing = .25f;
	public float xWavelength = 8f;

	public float ySmoothing = .25f;
	public float yWaveLength = 2f;

	private int pointPatternLength = 0;

	private bool Gap;
	private bool JumpRamp;
	private bool Flat;
	private bool towerFlat;
	private bool bridgeGap;

	private float Timer;
	private float PatternChangeWait = 12f;
	private float PatternChanging = 1000f;




	// Use this for initialization
	void Awake () {

		gameCamera = Camera.main;

		foreGroundGenerator = GetComponent<ForeGroundGenerator>();


		ChangeBorders();

		//points = new Vector3[4];

		//points[0] = new Vector3(LeftBorder.x - 20f, LeftBorder.y, 0);
		CurrentMeshPoint = new Vector3(LeftBorder.x - 15f, LeftBorder.y, 0f);
		NewMeshPoint = PointsGenerator();
		MeshNitialize();

		Gap = false;
		JumpRamp = false;
		Flat = false;
		towerFlat = false;
		bridgeGap = false;
		Timer = PatternChangeWait;


		/*
		StartingX = LeftBorder.x - 15f;
		print(LeftBorder.x + " leftborder.x");
		StartingY = LeftBorder.y - 5f;

		Initialize();
		*/
	}

	// Update is called once per frame
	void LateUpdate () {

		ChangeBorders();

		/*
		if(StartingX < (RightBorder.x + 20f))
		{

			Initialize();
		}
		print(StartingX + " StartingX");
		*/
		//print(RightBorder.x + 20f + " RightBorder.x");

		if(points[points.Count -1].x < (RightBorder.x + 100f))
		{
			MeshNitialize();
		}
		PatternCounter();
	}

	void ChangeBorders()
	{
		DistZ = gameCamera.transform.InverseTransformPoint(transform.position).z;
		LeftBorder = gameCamera.ViewportToWorldPoint(new Vector3(0,0,DistZ));
		//LeftBorder = gameCamera.transform.InverseTransformPoint(LeftBorder);
		RightBorder = gameCamera.ViewportToWorldPoint(new Vector3(1,0,DistZ));

		foreGroundGenerator.LeftBorder = LeftBorder;
		foreGroundGenerator.RightBorder = RightBorder;
		//RightBorder = gameCamera.transform.InverseTransformPoint(RightBorder);
	}

	void Initialize()
	{

		CurrentPoint = new Vector3(StartingX,StartingY,DistZ);
		NewPoint = new Vector3(StartingX + 1f, StartingY, DistZ);// + Random.Range(-1,1), DistZ);
		CreateLine();
		CurrentPoint = NewPoint;
		StartingX += .25f;
		StartingY = StartingY + Random.Range(-.25f, .25f);
		NewPoint = new Vector3(StartingX,StartingY,DistZ);
	}

	public void MeshNitialize()
	{
		//points = new Vector3[4];
		//points[0] = NewMeshPoint;


		vertices.Clear();
		triangles.Clear();

		CreatePoints();
		//this may not go here...must test

		CreateMesh();


		//CreateMesh();

		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		//mesh.uv = uvs.ToArray();

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		AddColliderToMesh();

		NewMeshPoint = points[points.Count -1];


	}

	void CreatePoints()
	{
		//points.Clear();

		//amount of points per section
		points = new List <Vector3> {Vector3.up,Vector3.up,Vector3.up,Vector3.up};


		for(int i = 0; i < points.Count ; i++)
		{
			//Generate 4 random points

			//set current point and save it for the next point
			points[i] = CurrentMeshPoint;
			//AddTerrainPoint(points[i]);

			Marker = new Vector3(this.CurrentMeshPoint.x, this.CurrentMeshPoint.y, this.CurrentMeshPoint.z);
			enemyGenerator.SpawnPoint = new Vector3(Marker.x -10f, Marker.y + 10f, Marker.z);
			platformGenerator.SpawnPoint = new Vector3(Marker.x + 10f, Marker.y + 20f, Marker.z);
			foreGroundGenerator.yPoint = Marker.y - 1.5f;
			if(this.transform.root.name == "PhysicalGroundGenerator")
			{
				player.PhysicalLandGroupHeight = Marker;
			}


			if(i != points.Count -1)
			{
				CurrentMeshPoint = NewMeshPoint;
				NewMeshPoint = PointsGenerator();
			}


		}

		int resolution = 20;
		for (int i = 0; i < resolution; i++)
		{
			float t = (float)i / (float)(resolution - 1);
			//get the point on our curve using the 4 points generated above
			Vector3 p = CalculateBezierPoint(t, points[0], points[1], points[2], points[3]);
			AddTerrainPoint(p);
		}
	}



	void CreateLine()
	{
		Line = new GameObject("Line").AddComponent<LineRenderer>();
		Line.gameObject.tag = "Ground";
		Line.material = new Material(Shader.Find("Diffuse"));
		Line.SetVertexCount(2);
		Line.SetWidth(10f,10f);
		Line.SetColors(Color.black, Color.black);
		Line.useWorldSpace = true;

		Line.SetPosition(0, CurrentPoint);
		Line.SetPosition(1, NewPoint);

		AddCollider();

	}

	public void CreateMesh()
	{
		//gameobject to house the mesh
		Mesh = new GameObject("Mesh");
		//mesh = Mesh.AddComponent<MeshRenderer>();

		//meshfilter for the uv shit
		filter = Mesh.AddComponent<MeshFilter>();
		mesh = filter.mesh;
		mesh.Clear();

		//meshrenderer to show the stuff
		meshRenderer = Mesh.AddComponent<MeshRenderer>();
		meshRenderer.material = GroundTileMaterial;
		meshRenderer.material.color = MeshColor;//(Color.black, Color.black);

		Mesh.transform.parent = LandGroup.transform;
		Mesh.layer = 8;

		//meshcollider for the player
		//meshCollider = Mesh.AddComponent<MeshCollider>();

		//Mesh.transform.position = new Vector3(CurrentMeshPoint.x, LeftBorder.y, 0f);
		//AddColliderToMesh();
		//tag for collisions
		Mesh.gameObject.tag = "Ground";

		//Destroy(Mesh,10f);


	}



	public void AddTerrainPoint(Vector3 point)
	{
		//Create Corresponding point along the bottom
		vertices.Add(new Vector3(point.x, -25f,0f));
		//then add our top point
		vertices.Add(point);
		if (vertices.Count >=4)
		{
			//we have completed a new quad, create 2 triangles
			int start = vertices.Count - 4;
			triangles.Add(start + 0);
			triangles.Add(start + 1);
			triangles.Add(start + 2);
			triangles.Add(start + 1);
			triangles.Add(start + 3);
			triangles.Add(start + 2);
		}

		Vector2 uvsIndex = new Vector2(Random.Range(0,2), Random.Range(0,1));
		uvs.Add(new Vector2 (tUnitW * uvsIndex.x, tUnitH * uvsIndex.y + 1f));
		uvs.Add(new Vector2 (tUnitW * uvsIndex.x + 1f, tUnitH * uvsIndex.y + 1f));
		uvs.Add(new Vector2 (tUnitW * uvsIndex.x + 1f, tUnitH * uvsIndex.y));
		uvs.Add(new Vector2 (tUnitW * uvsIndex.x, tUnitH * uvsIndex.y));

		GenerateCollider(point.x,point.y);
	}

	void AddCollider()
	{
		BoxCollider collider = new GameObject("collider").AddComponent<BoxCollider>();
		collider.transform.parent = Line.transform;

		float lineLength = Vector3.Distance(CurrentPoint, NewPoint);
		collider.size = new Vector3(lineLength, 10f, 1f);

		Vector3 midPoint = (CurrentPoint + NewPoint) / 2;
		collider.transform.position = midPoint;

		float Angle = (Mathf.Abs(CurrentPoint.y - NewPoint.y) / Mathf.Abs(CurrentPoint.x - NewPoint.x));
		if((CurrentPoint.y < NewPoint.y && CurrentPoint.x > NewPoint.x) || (NewPoint.y < CurrentPoint.y && NewPoint.x > CurrentPoint.x))
        {
            Angle *= -1;
        }
		Angle = Mathf.Rad2Deg * Mathf.Atan(Angle);
		collider.transform.Rotate (0, 0, Angle);
	}

	void AddColliderToMesh()
	{
		meshCollider = Mesh.AddComponent<MeshCollider>();

		//GenerateCollider((int)x, (int)y);

		Mesh newMesh = new Mesh();
		newMesh.vertices = colliderVertices.ToArray();
		newMesh.triangles = colliderTriangles.ToArray();
		meshCollider.sharedMesh = newMesh;

		colliderVertices.Clear();
		colliderTriangles.Clear();
		colliderCount = 0;


	}



	void GenerateCollider(float x, float y)
	{
		colliderVertices.Add(new Vector3 (x, y, 1));
		colliderVertices.Add(new Vector3 (x + 2, y, 1));
		colliderVertices.Add(new Vector3 (x + 2, y, -1));
		colliderVertices.Add(new Vector3 (x, y, -1));

		colliderTriangles.Add(colliderCount * 4);
		colliderTriangles.Add((colliderCount * 4) + 1);
		colliderTriangles.Add((colliderCount * 4) + 3);
		colliderTriangles.Add((colliderCount * 4) + 1);
		colliderTriangles.Add((colliderCount * 4) + 2);
		colliderTriangles.Add((colliderCount * 4) + 3);

		colliderCount++;
	}

	Vector3 PointsGenerator()
	{
		Vector3 newPatternPoint = NewMeshPoint;
		//print(Gap + " Gap");
		//print(JumpRamp + " JumpRamp");
		//print(Flat + " flat");
		if(Gap)
		{
			newPatternPoint = new Vector3(CurrentMeshPoint.x + xWavelength * xSmoothing  + Random.Range(4f,8f) , CurrentMeshPoint.y + Random.Range(-1f,1f), 0f);
			for(int i = 0; i < Random.Range(3,5); i++)
			{
				points.Add(new Vector3(newPatternPoint.x, newPatternPoint.y + 100f, 0f));
			}

			platformGenerator.SpawnPoint = new Vector3(newPatternPoint.x + 12f, newPatternPoint.y - 20f, 0f);

			platformGenerator.pitfallSpawn = true;
			platformGenerator.SpawnPitfall();
			//points = new List <Vector3>();
			Gap = false;
			Timer = PatternChangeWait;

			StartCoroutine(Waiting(5f));
		}
		else if(JumpRamp)
		{
			if(pointPatternLength < 6)
			{
				newPatternPoint = new Vector3(CurrentMeshPoint.x + xWavelength * xSmoothing, CurrentMeshPoint.y + 1.75f, 0f);
				pointPatternLength += 1;
			}
			else
			{
				Gap = true;
				JumpRamp = false;
				pointPatternLength = 0;
				Timer = PatternChangeWait;
			}
		}
		else if(Flat)
		{
			if(pointPatternLength < 25f)
			{
				newPatternPoint = new Vector3(CurrentMeshPoint.x + xWavelength * xSmoothing, CurrentMeshPoint.y, 0f);
				pointPatternLength += 1;
				platformGenerator.SpawnPoint = new Vector3(newPatternPoint.x + 40f, newPatternPoint.y + 20f, 0f);
				if(pointPatternLength == 1)
				{
					platformGenerator.dojoSpawn = true;
					platformGenerator.SpawnDojo();

				}

			}
			else
			{
				Flat = false;
				pointPatternLength = 0;
				Timer = PatternChangeWait;
			}
		}
		else if(towerFlat)
		{
			if(pointPatternLength < 5f)
			{
				newPatternPoint = new Vector3(CurrentMeshPoint.x + xWavelength * xSmoothing, CurrentMeshPoint.y, 0f);
				pointPatternLength += 1;
				platformGenerator.SpawnPoint = new Vector3(newPatternPoint.x + 10f, newPatternPoint.y + 20f, 0f);
				if(pointPatternLength == 1)
				{

					platformGenerator.towerSpawn = true;
					platformGenerator.SpawnTower();
				}

			}
			else
			{
				towerFlat = false;
				pointPatternLength = 0;
				Timer = PatternChangeWait;
			}
		}

		else if(bridgeGap)
		{
			newPatternPoint = new Vector3(CurrentMeshPoint.x + xWavelength * xSmoothing  + Random.Range(4f,8f) , CurrentMeshPoint.y + Random.Range(-1f,1f), 0f);
			for(int i = 0; i < Random.Range(6,8); i++)
			{
				points.Add(new Vector3(newPatternPoint.x, newPatternPoint.y + 100f, 0f));
			}

			platformGenerator.SpawnPoint = new Vector3(newPatternPoint.x + 17f, newPatternPoint.y + 20f, 0f);

			platformGenerator.bridgeSpawn = true;
			platformGenerator.SpawnBridge();
			//points = new List <Vector3>();
			bridgeGap = false;
			Timer = PatternChangeWait;

			StartCoroutine(Waiting(5f));
		}
		else
		{
			newPatternPoint = new Vector3(CurrentMeshPoint.x + xWavelength * xSmoothing, Mathf.Cos(Random.Range(0, 90)) + CurrentMeshPoint.y * ySmoothing, 0f);
		}

		return newPatternPoint;
	}

	void PatternCounter()
	{

		//print(Timer + " pattercounter timer");
		if(Timer <= 0f)
		{
			int patternProbability = Random.Range(0,100);
			if(patternProbability >= 0 && patternProbability < 20)
			{
				JumpRamp = true;
				Timer = PatternChanging;
			}
			else if(patternProbability >= 20 && patternProbability < 40)
			{
				Gap = true;
				enemyGenerator.GameOn = false;
				Timer = PatternChanging;
			}
			else if(patternProbability >= 40 && patternProbability < 60)
			{

				int probs = Random.Range(0,100);

				if(probs >= 0 && probs < 50)
				{
					Flat = true;
					Timer = PatternChanging;
				}
				else if(probs >= 50 && probs < 100)
				{
					towerFlat = true;
					Timer = PatternChanging;
				}

			}
			else if(patternProbability >= 60 && patternProbability < 80)
			{
				bridgeGap = true;
				Timer = PatternChanging;
			}
			else
			{
				JumpRamp = false;
				Gap = false;
				Flat = false;
			}
		}
		Timer -= Time.deltaTime;
	}



	public Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		float u = 1 - t;
		float tt = t * t;
		float uu = u * u;
		float uuu = uu * u;
		float ttt = tt * t;

		Vector3 p = uuu * p0;
		p += 3 * uu * t * p1;
		p += 3 * u * tt * p2;
		p += ttt * p3;

		return p;
	}

	private IEnumerator Waiting(float seconds)
	{
		yield return new WaitForSeconds(seconds);

		enemyGenerator.GameOn = true;
	}
}
