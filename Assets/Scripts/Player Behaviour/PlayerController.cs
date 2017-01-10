using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : Figure
{

	public Transform firingPoint;
	public Transform groundCheck;
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
		isAlive = true;
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
		float velocity = Mathf.Abs(body.velocity.x);
		float direction = joystick.Horizontal();
		
		anim.SetBool("grounded", IsGrounded(groundCheck));

		//Move method in parent class
		Move(anim, body, direction, velocity);
		//Jump method in parent class
		if (jumpButton.Pressed() || Input.GetKey(KeyCode.Space))
		{
			Jump(anim, body);
		}
		//End of Jump
	}
	void AttackCheck()
	{
		//Melee
		if ((attackButton.Pressed() || Input.GetKey(KeyCode.Z))
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
		if (!isThrowing &&
			KunaiManager.number > 0)
		{
			if ((throwButton.Pressed() || Input.GetKey(KeyCode.X)))
			{
				ThrowKunai(anim, body, projective, firingPoint, -1);
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
