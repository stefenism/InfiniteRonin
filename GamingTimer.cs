using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GamingTimer : MonoBehaviour {

	public Text TimeText;
	private string TimeOutput;

	public float StartTime;
	public float TotalTime = 300f;
	public float CurrentRawTime;
	
	private int Minutes;
	private int Seconds;
	private int fraction;


	void Awake()
	{
		StartTime = 0f;
		//powerBar = GameObject.FindGameObjectWithTag("PowerBar").GetComponent<Image>();

	}

	void Update()
	{

		CurrentRawTime = StartTime + Time.time;

		Minutes = (int)CurrentRawTime / 60;
		Seconds = (int)CurrentRawTime % 60;
		fraction = (int)(CurrentRawTime * 100) % 100;

		TimeOutput = string.Format ("{0:00}:{1:00}:{2:000}", Minutes, Seconds, fraction);

		//UpdateTimer();
		//print(TimeOutput + " current time");

		//EndGame();

	}

	//void UpdateTimer()
	//{

	  //  TimeText.text = "Time Left: " + TimeOutput;
	
	//}

	/*public void EndGame()
	{
		
		if(CurrentRawTime <= 0 || StartTime <= 0 || playerHealth.Health <= 0)
		{
			Application.LoadLevel("GameOver");
			CurrentRawTime = 100000f;
			StartTime = 10000f;
			GameOver = true;


			inventoryDisplay.foodList.Clear();
			throwFood.CanControl = false;
			//powerBar.fillAmount = 0f;
			foodThoughts = GameObject.FindGameObjectsWithTag("FoodThought");
			for(int i = 0; i < foodThoughts.Length; i++)
			{
				Destroy(foodThoughts[i]);
			}
			
		}*/
		//CurrentRawTime = 1000000f;
		//GameOver = true;
		
	//}
}