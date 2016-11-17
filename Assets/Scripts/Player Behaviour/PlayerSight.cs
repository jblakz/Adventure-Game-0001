using UnityEngine;
using System.Collections;

public class PlayerSight : MonoBehaviour {
	
	EnemyController[] allEnemies;
	private int hbCounter = 0;

	// Use this for initialization
	void Start () {
		allEnemies = FindObjectsOfType<EnemyController>();
		foreach (var enemy in allEnemies)
		{
			enemy.GetComponentInChildren<HealthBar>().ToggleOn(false);
		}
	}
	
	void OnTriggerEnter2D(Collider2D target)
	{
		if (target.tag == "Enemy")
			if (target.enabled)
			{
				
            }
	}
	void OnTriggerExit2D(Collider2D target)
	{
		if (target.tag == "Enemy")
		{
			
		}
	}
}
