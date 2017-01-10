using UnityEngine;

using System.Collections;



public class ThrowerController : EnemyController
{
	private Rigidbody2D body;
	private Animator anim;
	private PlayerController player;
	private bool canAttack;
	private bool canChasePlayer;


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
	}

	void MovementCheck()
	{
		anim.SetBool("grounded", IsGrounded(groundCheck));
		float velocity = Mathf.Abs(body.velocity.x);
		float direction = player.transform.position.x - body.transform.position.x;

		if (awareRadius.GetComponent<Collider2D>().IsTouching(player.GetComponent<Collider2D>())
			&& !strikeRadius.GetComponent<Collider2D>().IsTouching(player.GetComponent<Collider2D>())
			)
		{
			if (canChasePlayer)
			{
				Move(anim, body, direction, velocity);
			}
			else
			{
				float orgSpeed = speed;
				speed = 0f;
				Move(anim, body, direction, velocity);
				anim.SetBool("isRunning", false);
				speed = orgSpeed;
			}
			/*
			//Jump
			if (jumpButton.Pressed() || Input.GetKey(KeyCode.Space))
			{
				if (anim.GetBool("grounded"))
				{
					anim.SetBool("grounded", false);
					forceY = jumpForce;
					body.velocity = new Vector2(body.velocity.x, forceY);
				}
			}*/
			//End of Jump
		}
		else
		{
			anim.SetBool("isRunning", false);
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
		//Throw
		if (awareRadius.GetComponent<Collider2D>().IsTouching(player.GetComponent<Collider2D>())
			&& !isThrowing && !canChasePlayer)
		{
			anim.SetBool("isThrowing", true);
			ThrowKunai(anim, body, projective, firingPoint, 0);
		}
		else
		{
			anim.SetBool("isThrowing", false);
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
	void OnCollisionEnter2D(Collision2D target)
	{
		if (target.collider.tag == "Enemy")
		{
			canChasePlayer = true;
		}
	}
	void OnCollisionExit2D(Collision2D target)
	{
		if (target.collider.tag == "Enemy")
		{
			canChasePlayer = false;
		}
	}
}