using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : Figure
{
	public float speed;
	public float jumpForce;
	public float maxVelocity;

	public Transform firingPoint;
	public Transform groundCheck;
	public float groundCheckRadius = 0.1f;
	public LayerMask groundLayer;
	public GameObject closeAttack;
	public GameObject projective;
	//Inputs
	static VirtualJoystick joystick;
	static JumpButton jumpButton;
	static AttackButton attackButton;
	static ThrowButton throwButton;

	//[SerializeField]
	private Rigidbody2D body;
	private Animator anim;
	private bool isDead;
	static LevelController levelController;

	void Awake()
	{
		body = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		levelController = FindObjectOfType<LevelController>();
		health = maxHealth;

		//Inputs Declaration
		joystick = GameObject.Find("Canvas/Buttons/JoystickBG").GetComponent<VirtualJoystick>();
		jumpButton = GameObject.Find("Jump").GetComponent<JumpButton>();
		attackButton = GameObject.Find("Attack").GetComponent<AttackButton>();
		throwButton = GameObject.Find("Throw").GetComponent<ThrowButton>();
	}

	void FixedUpdate()
	{
		MovementCheck();
		AttackCheck();
		DeathCheck();
	}

	void MovementCheck()
	{
		float forceX = 0f;
		float forceY = 0f;
		float velocity = Mathf.Abs(body.velocity.x);
		float direction = joystick.Horizontal();

		anim.SetBool("grounded",
			Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer));

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
		else if (direction < 0)
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
		else
		{
			anim.SetBool("isRunning", false);
		}
		//End of Left - Right

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
		body.AddForce(new Vector2(forceX, 0));
	}
	void AttackCheck()
	{
		//Melee
		if ((attackButton.Pressed() || Input.GetKey(KeyCode.Z))
			&& !isAttacking
			)
		{
			if (anim.GetBool("grounded"))
				anim.Play("Attack");
			else anim.Play("JumpAttack");
			float direction = 2*body.transform.localScale.x;
			closeAttack.GetComponent<StrikeAttackController>().attacker = this;
			Vector3 position = new Vector3(firingPoint.position.x - direction, firingPoint.position.y);
			Instantiate(closeAttack, position, firingPoint.rotation);
		}
		else
		{
			anim.SetBool("isAttacking", false);
		}
		
		//Throw
		if (!isThrowing &&
			KunaiManager.number > 0)
		{
			if ((throwButton.Pressed() || Input.GetKey(KeyCode.X)))
			{
				anim.Play("Throw");
				projective.GetComponent<KunaiThrowController>().thrower = this;
				Instantiate(projective, firingPoint.position, firingPoint.rotation);
				KunaiManager.AddNumbers(-1);
			}
		}
		else
			anim.SetBool("isThrowing", false);
	}
	void DeathCheck()
	{
		if (body.position.y < -4.5f)
			health = 0;
		if (health <= 0 && !isDead)
		{
			isDead = true;
			levelController.ResetLevel();
		}
		if (body.IsTouching(levelController.currentCheckpoint.GetComponent<Collider2D>()))
			isDead = false;
		anim.SetBool("isDead", isDead);
	}

	//Coroutine
	/*public IEnumerator AttackDelayCo()
	{
		if (firePoint.GetComponent<Collider2D>().enabled)
		{
			yield return new WaitForSeconds(0.2f);
			firePoint.GetComponent<Collider2D>().enabled = false;
		}
		else
		{
			yield return new WaitForSeconds(1f);
			firePoint.GetComponent<Collider2D>().enabled = true;
		}
	}*/
}
