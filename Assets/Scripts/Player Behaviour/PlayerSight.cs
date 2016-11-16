using UnityEngine;
using System.Collections;

public class PlayerSight : MonoBehaviour {

	EnemyController[] allEnemies;

	// Use this for initialization
	void Start () {
		allEnemies = FindObjectsOfType<EnemyController>();
		foreach (var enemy in allEnemies)
		{
			enemy.Die();
			foreach (var collider in enemy.GetComponents<Collider2D>())
				collider.enabled = true;
		}
	}
	
	void OnTriggerEnter2D(Collider2D target)
	{
		if (target.tag == "Enemy")
		{
			if (!target.enabled)
				target.GetComponent<EnemyController>().Respawn(LevelController.gravity, target.GetComponent<EnemyController>().spawnPosition, null);
		}
	}
}
