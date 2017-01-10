using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class LevelController : MonoBehaviour
{

	public Transform currentCheckpoint;
	public GameObject damageParticle;
	public GameObject respawnParticle;
	public static EnemyController focusedEnemy;
	public float respawnDelay = 0.5f;

	public AudioClip clickSound;
	public AudioClip readyVoice;
	public AudioClip gameOverVoice;
	public AudioClip missionCompleteVoice;

	public static float gravity = 3f;

	private CameraFollow mainCam;
	private LevelExit levelExit;
	private TipPaneController tipPane;
	private PauseMenuController pauseMenu;
	private PlayerController player;
    private EnemyController[] allEnemies;
	private KunaiPackController[] kunais;

	// Use this for initialization
	void Start()
	{
		mainCam = FindObjectOfType<CameraFollow>();
		levelExit = FindObjectOfType <LevelExit>();
		player = FindObjectOfType<PlayerController>();
		player.GetComponent<Rigidbody2D>().gravityScale = gravity;

		tipPane = FindObjectOfType<TipPaneController>();

		pauseMenu = FindObjectOfType<PauseMenuController>();
		pauseMenu.TogglePauseMenu(false);

		allEnemies = FindObjectsOfType<EnemyController>();

		//Play sounds
		if (GetComponent<AudioSource>().isPlaying)
			GetComponent<AudioSource>().Stop();
		GetComponent<AudioSource>().PlayOneShot(readyVoice);
	}

	// Update is called once per frame
	void Update()
	{
		CheckFocusedEnemy();
	}

	private void CheckFocusedEnemy()
	{
		foreach (var enemy in allEnemies)
			enemy.GetComponentInChildren<HealthBar>().ToggleOn(false);
		if (focusedEnemy != null)
		{
			focusedEnemy.GetComponentInChildren<HealthBar>().ToggleOn(true);
			if (!focusedEnemy.isAlive)
				focusedEnemy.GetComponentInChildren<HealthBar>().ToggleOn(false);
		}
	}
	#region Reset Level
	public void ResetLevel()
	{
		//Play sound
		if (GetComponent<AudioSource>().isPlaying)
			GetComponent<AudioSource>().Stop();
		GetComponent<AudioSource>().PlayOneShot(gameOverVoice);

		StartCoroutine(ResetLevelCo());

	}

	public IEnumerator ResetLevelCo()
	{
		StartCoroutine(PlayerDieCo());
		player.GetComponent<Animator>().SetBool("isDead", false);
		yield return new WaitForSeconds(respawnDelay);
		//Respawn Player
		player.Respawn(gravity, currentCheckpoint.position, respawnParticle);
		//Respawn Enemies
		RespawnEnemies();
		//Reset Kunai System
		ResetKunaiSystem();
		//reset camera
		mainCam.minX = 1f;
		mainCam.maxX = levelExit.transform.position.x - 5f;
		//Reset Tip Pane
		StartCoroutine(tipPane.CloseMessageCo(0f));

	}
	public IEnumerator PlayerDieCo()
	{
		yield return new WaitForSeconds(respawnDelay);
		player.Die();
	}
	private void RespawnEnemies()
	{
		foreach (var obj in allEnemies)
		{
			EnemyController enemy = obj.GetComponent<EnemyController>();
            enemy.Respawn(gravity, enemy.spawnPosition, null);
			if (enemy.lockingRadius != null)
			{
				enemy.GetComponent<LockPlayerController>().LockPlayer(false);
                enemy.GetComponent<LockPlayerController>().enabled = false;
			}
		}
	}
	private void ResetKunaiSystem()
	{
		KunaiManager.Reset();
		kunais = FindObjectsOfType<KunaiPackController>();
		foreach (var kunai in kunais)
		{
			if (kunai.isDropped)
				Destroy(kunai.gameObject);
			else kunai.Enable(true);
		}
	}
	#endregion
}
