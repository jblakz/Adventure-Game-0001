using UnityEngine;
using System.Collections;

public class PlayerSight : MonoBehaviour {

	public int hbsCounter;
	public HealthBar displayedHealthBar;
	EnemyController[] allEnemies;

	// Use this for initialization
	void Start () {
		allEnemies = FindObjectsOfType<EnemyController>();
        foreach (var enemy in allEnemies)
			enemy.GetComponentInChildren<HealthBar>().ToggleOn(false);
		hbsCounter = 0;
	}
	void Update()
	{

	}
	
	void OnTriggerEnter2D(Collider2D target)
	{
		if (target.tag == "Enemy")
		{
				target.GetComponentInChildren<HealthBar>().ToggleOn(true);
				displayedHealthBar = target.GetComponentInChildren<HealthBar>();
				hbsCounter++;
		}
	}
	void OnTriggerExit2D(Collider2D target)
	{
		if (target.tag == "Enemy")
		{
				target.GetComponentInChildren<HealthBar>().ToggleOn(false);
				hbsCounter--;
		}
	}
}
