using UnityEngine;
using System.Collections;

public class GruntController : EnemyController  {

	/*public SpriteRenderer indicator;
	public Transform strikeRadius;
	public Transform firingPoint;
	public Transform awareRadius;
	public Transform groundCheck;
	public GameObject closeAttack;
	public GameObject projective;
	public GameObject damageEffect;
	
	public Vector3 spawnPosition;*/
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

			Move(anim, body, direction, velocity);
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
			}
			//End of Jump
			*/
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
		/*//Throw
		if (discoverPoint.GetComponent<Collider2D>().IsTouching(player.GetComponent<Collider2D>())
			&& !isThrowing)
		{
			anim.SetBool("isThrowing", true);
			kunai.GetComponent<KunaiThrowController>().thrower = this;
			Instantiate(kunai, attackRange.position, attackRange.rotation);
        }
		else
		{
			anim.SetBool("isThrowing", false);
		}*/
		
	}
	void DeathCheck()
	{
		if (health <= 0 || body.position.y < -4.5f)
		{
			Instantiate(damageEffect,body.transform.position,body.transform.rotation);
			anim.SetBool("isDead", true);
			enabled = false;
			Die();
		}
	}
}
