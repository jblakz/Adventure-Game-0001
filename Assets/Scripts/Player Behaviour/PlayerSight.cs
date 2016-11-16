using UnityEngine;
using System.Collections;

public class PlayerSight : MonoBehaviour {

	EnemyController[] allEnemies;

	// Use this for initialization
	void Start () {
		allEnemies = FindObjectsOfType<EnemyController>();
		foreach (var enemy in allEnemies)
		{
			enemy.withinSight = false;
		}
	}
	
	void OnTriggerEnter2D(Collider2D target)
	{
		if (target.tag == "Enemy")
			if (target.enabled)
				target.GetComponent<EnemyController>().withinSight = true;
	}
	void OnTriggerExit2D(Collider target)
	{
		if (target.tag == "Enemy")
			target.GetComponent<EnemyController>().withinSight = false;
	}
}
