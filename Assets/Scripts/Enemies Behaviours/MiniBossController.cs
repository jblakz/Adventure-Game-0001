using UnityEngine;

using System.Collections;



public class MiniBossController : EnemyController
{
	private GameObject playerProjective;
	private Rigidbody2D body;
	private Animator anim;
	private PlayerController player;
	private bool canAttack;


	void Awake()
	{
		isAlive = true;
		body = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		healthCanvas = GetComponentInChildren<Canvas>();
		player = FindObjectOfType<PlayerController>();
		health = maxHealth;
		spawnPosition = body.position;
	}

	void FixedUpdate()
	{
		LockingCheck(player);
		MovementCheck();
		AttackCheck();
		DeathCheck();
		FixHealthCanvas();
		playerProjective = GetPlayerProjective();
	}

	void MovementCheck()
	{
		anim.SetBool("grounded", IsGrounded(groundCheck));
		float velocity = Mathf.Abs(body.velocity.x);
		float direction = player.transform.position.x - body.transform.position.x;
		//Running
		if (awareRadius.GetComponent<Collider2D>().IsTouching(player.GetComponent<Collider2D>())
			&& !strikeRadius.GetComponent<Collider2D>().IsTouching(player.GetComponent<Collider2D>())
			)
		{

			Move(anim, body, direction, velocity);
		}
		else
		{
			anim.SetBool("isRunning", false);
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
			MeleeAttack(anim, body, closeAttack, firingPoint);
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
			enabled = false;
			Die();
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
}