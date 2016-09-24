using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	public float Health;
	public float MaxHealth;

	public float CurrentHealth;
	//public Image HealthBar;


	// Use this for initialization
	void Awake () {

		Health = 4f;
		MaxHealth = 4f;

	}

	// Update is called once per frame
	void Update () {

	}


	public void DealDamage(float damage)
	{
		Health -= damage;
		CurrentHealth = (Health / MaxHealth);
		CurrentHealth = Mathf.Clamp01(CurrentHealth);

		//HealthBar.fillAmount = CurrentHealth;
	}

}
