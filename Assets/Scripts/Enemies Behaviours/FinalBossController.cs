using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System;

public class FinalBossController : EnemyController
{
	public int chaosHealth;
	public int chaosAttackSpeed;
	public GameObject subtitle;
	public Transform standOffRadius;

	private float startAttackSpeed;
	private GameObject playerProjective;
	private Rigidbody2D body;
	private Animator anim;
	private PlayerController player;
	private ParticleSystem emitter;
	private bool canAttack;
	private bool isChaos;
	//Skills
	private Skill[] skills;

	void Awake()
	{
		isAlive = true;
		body = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		healthCanvas = GetComponentInChildren<Canvas>();
		player = FindObjectOfType<PlayerController>();
		startAttackSpeed = attackSpeed;
		health = maxHealth;
		spawnPosition = body.position;
		emitter = GetComponentInChildren<ParticleSystem>();
		if (GetComponents<Skill>().Length != 0)
			skills = GetComponents<Skill>();
	}

	void FixedUpdate()
	{
		LockingCheck(player);
		MovementCheck();
		AttackCheck();
		DeathCheck();
		FixHealthCanvas();
		AutoCastSkills();
		ChaosCheck();
		playerProjective = GetPlayerProjective();
	}

	void MovementCheck()
	{
		anim.SetBool("grounded", IsGrounded(groundCheck));
		float velocity = Mathf.Abs(body.velocity.x);
		float direction = player.transform.position.x - body.transform.position.x;

		if (awareRadius.GetComponent<Collider2D>().IsTouching(player.GetComponent<Collider2D>())
			&& !strikeRadius.GetComponent<Collider2D>().IsTouching(player.GetComponent<Collider2D>())
			&& !standOffRadius.GetComponent<Collider2D>().IsTouching(player.GetComponent<Collider2D>()))
		{

			Move(anim, body, direction, velocity);
		}
		else
		{
			anim.SetBool("isRunning", false);
		}
		if (standOffRadius.GetComponent<Collider2D>().IsTouching(player.GetComponent<Collider2D>())
			&& transform.position.y <= player.transform.position.y
			&& !GetComponent<Collider2D>().IsTouching(player.GetComponent<Collider2D>()))
		{
			Move(anim, body, -direction, velocity);
		}
		if (transform.position.y + 3f < player.transform.position.y
			|| GetComponent<Collider2D>().IsTouching(player.GetComponent<Collider2D>()))
		{
			Jump(anim, body);
		}
		//Dodge projective
		if (playerProjective != null)
		{
			if ((int)awareRadius.transform.position.x == (int)playerProjective.transform.position.x)
				Jump(anim, body);
		}
	}
	void AttackCheck()
	{
		canAttack = strikeRadius.GetComponent<Collider2D>().IsTouching(player.GetComponent<Collider2D>());
		//Melee
		if (canAttack
			&& !isAttacking
			)
		{
			FireballAttack(anim, body, closeAttack, firingPoint);
		}
		else
		{
			anim.SetBool("isAttacking", false);
		}
	}
	void DeathCheck()
	{
		if (health <= 0 || body.position.y < -4.5f)
		{
			Instantiate(damageEffect, body.transform.position, body.transform.rotation);
			anim.SetBool("isDead", true);
			Die();
		}
	}
	private void FireballAttack(Animator anim, Rigidbody2D body, GameObject fireball, Transform firingPoint)
	{
		if (anim.GetBool("grounded"))
			anim.Play("Attack");
		else anim.Play("JumpAttack");
		projective.GetComponent<FireballController>().thrower = this;
		float direction = 2 * body.transform.localScale.x;
		Quaternion rotation = firingPoint.rotation;
		rotation.z = (direction > 0) ? 0f : 180f;
		Instantiate(fireball, firingPoint.position, rotation);
	}
	private void AutoCastSkills()
	{
		foreach (var skill in skills)
		{
			if (skill.canCast)
				skill.Cast();
		}
	}
	private GameObject GetPlayerProjective()
	{
		KunaiThrowController[] projectives = FindObjectsOfType<KunaiThrowController>();
		GameObject obj = null;
		if (projectives.Length != 0)
		{
			foreach (var projective in projectives)
			{
				if (projective.thrower.GetComponent<EnemyController>() == null)
					obj = projective.gameObject;
			}
		}
		return obj;
	}
	private void ChaosCheck()
	{
		if (health <= chaosHealth && health > 0)
		{
			if (!isChaos)
			{
				attackSpeed = chaosAttackSpeed;
				emitter.Play();
				isChaos = true;
			}
		}
		else
		{
			attackSpeed = startAttackSpeed;
			emitter.Stop();
			isChaos = false;
		}
	}
}