using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{

	public Transform currentCheckpoint;
	public GameObject damageParticle;
	public GameObject respawnParticle;
	public float respawnDelay = 0.5f;
	public static float gravity = 3f;

	private PlayerController player;
	
	private EnemyController[] allEnemies;
	private EnemyHBsFrame enemyHBsFrame;
	private HealthBar[] allHealthBar;

	// Use this for initialization
	void Start()
	{
		player = FindObjectOfType<PlayerController>();
		player.GetComponent<Rigidbody2D>().gravityScale = gravity;
		allEnemies = FindObjectsOfType<EnemyController>();
		enemyHBsFrame = FindObjectOfType<EnemyHBsFrame>();
		allHealthBar = FindObjectsOfType<HealthBar>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void ResetLevel()
	{
		StartCoroutine(ResetLevelCo());

	}

	public IEnumerator ResetLevelCo()
	{
		StartCoroutine(PlayerDieCo());
		player.GetComponent<Animator>().SetBool("isDead", false);
		yield return new WaitForSeconds(respawnDelay);
		player.Respawn(gravity, currentCheckpoint.position, respawnParticle);
		KunaiManager.Reset();
		foreach (var enemy in allEnemies)
			enemy.Respawn(gravity, enemy.spawnPosition, null);
		foreach (var hb in allHealthBar)
			hb.Refresh();
		enemyHBsFrame.ConfigureAllHealthBars();
	}
	public IEnumerator PlayerDieCo()
	{
		yield return new WaitForSeconds(respawnDelay);
		player.Die();
	}
}
