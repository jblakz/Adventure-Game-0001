using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHBsFrame : MonoBehaviour
{

	private HealthBar healthBar1;
	private HealthBar healthBar2;
	private HealthBar healthBar3;
	private EnemyController[] allEnemies;

	// Use this for initialization
	void Start()
	{
		healthBar1 = GameObject.Find("Enemies Info/Enemy Health Bar 1").GetComponent<HealthBar>();
		healthBar2 = GameObject.Find("Enemies Info/Enemy Health Bar 2").GetComponent<HealthBar>();
		healthBar3 = GameObject.Find("Enemies Info/Enemy Health Bar 3").GetComponent<HealthBar>();
		allEnemies = FindObjectsOfType<EnemyController>();
		ConfigureAllHealthBars();
    }

	// Update is called once per frame
	void Update()
	{
		UpdateHealthBar(healthBar1);
		UpdateHealthBar(healthBar2);
		UpdateHealthBar(healthBar3);
		TrimmingBars();
	}
	void ConfigureOwner(HealthBar healthBar)
	{
		foreach (var enemy in allEnemies)
		{
			if (!enemy.healthDisplayed && enemy.enabled)
			{
				healthBar.owner = enemy;
				healthBar.owner.healthDisplayed = true;
				break;
			}
		}
	}
	void UpdateHealthBar(HealthBar healthBar)
	{
		if (healthBar.owner != null)
		{
			if (!healthBar.owner.enabled)
			{
				foreach (var enemy in allEnemies)
				{
					enemy.healthDisplayed = false;
				}
				ConfigureAllHealthBars();
			}
		}
	}
	void TrimmingBars()
	{
		if (healthBar1.owner == healthBar2.owner)
			healthBar2.owner = null;
		if (healthBar1.owner == healthBar3.owner
			||healthBar2.owner == healthBar3.owner)
			healthBar3.owner = null;
	}
	public void ConfigureAllHealthBars()
	{
		ConfigureOwner(healthBar1);
		ConfigureOwner(healthBar2);
		ConfigureOwner(healthBar3);
	}
}