using UnityEngine;
using System.Collections;

public class EnemyController : Figure {
	public float speed;
	public float jumpForce;
	public float maxVelocity;

	public Vector3 spawnPosition;
	public SpriteRenderer indicator;
	public Transform strikeRadius;
	public Transform firingPoint;
	public Transform awareRadius;
	public Transform groundCheck;
	public float groundCheckRadius = 0.1f;
	public LayerMask groundLayer;
	public GameObject closeAttack;
	public GameObject projective;
	public GameObject damageEffect;

	//[SerializeField]
	private Rigidbody2D body;
	private Animator anim;
	private PlayerController player;
	private bool canAttack;


	void Awake()
	{
		body = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		player = FindObjectOfType<PlayerController>();
		health = maxHealth;
		spawnPosition = body.position;
	}

	// Use this for initialization
	void Start()
	{
		anim.SetBool("isDead", false);
	}

	// Update is called once per frame
	void Update()
	{

	}

	void FixedUpdate()
	{
		MovementCheck();
		AttackCheck();
		DeathCheck();
	}

	void MovementCheck()
	{
		anim.SetBool("grounded",
			Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer));
		float forceX = 0f;
		//float forceY = 0f;
		float velocity = Mathf.Abs(body.velocity.x);
		float direction = player.transform.position.x - body.transform.position.x;
		if (awareRadius.GetComponent<Collider2D>().IsTouching(player.GetComponent<Collider2D>())
			&& !strikeRadius.GetComponent<Collider2D>().IsTouching(player.GetComponent<Collider2D>())
			)
		{
			//Left - Right
			if (direction > 0)
			{
				if (velocity < maxVelocity)
				{
					if (anim.GetBool("grounded"))
						forceX = speed;
					else
						forceX = speed * 1.2f;
				}
				anim.SetBool("isRunning", true);
				Vector3 tempVector = transform.localScale;
				tempVector.x = 0.5f;
				transform.localScale = tempVector;

			}
			if (direction < 0)
			{
				if (velocity < maxVelocity)
				{
					if (anim.GetBool("grounded"))
						forceX = -speed;
					else
						forceX = -speed * 1.2f;
				}
				anim.SetBool("isRunning", true);
				Vector3 tempVector = transform.localScale;
				tempVector.x = -0.5f;
				transform.localScale = tempVector;
			}
			//End of Left - Right
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
			body.AddForce(new Vector2(forceX, 0));
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
			if (anim.GetBool("grounded"))
				anim.Play("Attack");
			else
				anim.Play("JumpAttack");
            float direction = 2 * body.transform.localScale.x;
			closeAttack.GetComponent<StrikeAttackController>().attacker = this;
			Vector3 position = new Vector3(firingPoint.position.x - direction, firingPoint.position.y);
			Instantiate(closeAttack, position, firingPoint.rotation);
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
			Die();
		}
	}
}
