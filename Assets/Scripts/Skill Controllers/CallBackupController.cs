using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class CallBackupController : Skill
{
	public Figure ally1;
	public Figure ally2;
	public Figure ally3;
	public Figure ally4;
	public Transform callPoint;
	public GameObject backupItem;
	public float backupNumber;
	public float backupDuration = 15f;
	public float coolDownTimer;
	public float durationTimer;

	private SubtitleController sub;
	private LevelController levelController;
	private List<Figure> alliesList;
	private string castAnimationName;

	void Awake()
	{
		levelController = FindObjectOfType<LevelController>();
		alliesList = new List<Figure>();
		AddAlly(ally1);
		AddAlly(ally2);
		AddAlly(ally3);
		AddAlly(ally4);
		caster = GetComponent<Figure>();
		coolDownTimer = coolDown;
		sub = GetComponentInChildren<SubtitleController>();
		//Skill Infos
		skillName = "Call Backup";
		description = "When cast, call " + alliesList.Count + " allies for "
			+ backupDuration + " seconds at specific position to aid caster on combat";
		castAnimationName = "Throw";
	}
	void FixedUpdate()
	{
		//Cooldown timer
		if (coolDownTimer >= 0)
		{
			canCast = false;
			coolDownTimer -= Time.deltaTime;
		}
		else
		{
			canCast = true;
			coolDownTimer = 0f;
		}
		//Backup duration timer
		if (durationTimer >= 0)
		{
			durationTimer -= Time.deltaTime;
		}
		else
		{
			UnSummonAllies();
			durationTimer = backupDuration;
		}
	}
	public override void Cast()
	{
		if (canCast)
		{
			coolDownTimer = coolDown;
			durationTimer = backupDuration;
			caster.GetComponent<Animator>().Play(castAnimationName);
			//Start casting
			foreach (var ally in alliesList)
			{
				Vector3 pos = (ally.GetComponent<EnemyController>() == null) ?
                   levelController.currentCheckpoint.position : ally.GetComponent<EnemyController>().spawnPosition;
				ally.Respawn(LevelController.gravity, pos, levelController.respawnParticle);
				ally.isSummoned = true;
				if (ally.tag == "Enemy")
					ally.GetComponent<EnemyController>().droppedItem = backupItem;

				string msgString = skillName + " !!!";
				sub.ShowMessage(msgString, 3f);
			}
		}
	}
	public override void EndCasting()
	{
		coolDownTimer = 0f;
	}
	private void AddAlly(Figure ally)
	{
		if (ally != null && alliesList.Count < backupNumber)
		{
			alliesList.Add(ally);
			ally.Die();
		}
	}
	private void UnSummonAllies()
	{
		foreach (var ally in alliesList)
		{
			if (ally.isAlive)
			{
				if (ally.tag == "Enemy")
					ally.GetComponent<EnemyController>().droppedItem = null;
				ally.Die();
			}
		}
	}
}