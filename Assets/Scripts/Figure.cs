using UnityEngine;
using System.Collections;

public abstract class Figure : MonoBehaviour {

	public float speed;
	public float jumpForce;
	public float maxVelocity;

	public float health;
	public float maxHealth;
	public float attackDamage;
	public float attackRange;
	public float attackSpeed;   //Number of attack per 1 sec
	public float throwDamage;
	public float throwRange;
	public float throwSpeed;
	public float projectiveSpeed;

	public float groundCheckRadius = 0.1f;

	public bool isAttacking;
	public bool isThrowing;
	public bool isAlive;
	public bool isSummoned;

	public AudioClip footstepSound;
	public AudioClip jumpSound;
	public LayerMask groundLayer;
	public Vector2 knockbackForce;

    private Collider2D[] colliders;

	public void Move(Animator anim, Rigidbody2D body, float direction, float velocity)
	{
		float forceX = 0f;

		//Move Right
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
			tempVector.x = Mathf.Abs(transform.localScale.x);
			transform.localScale = tempVector;

			//play footsteps audio
			if(!GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().PlayOneShot(footstepSound);
		}
		//Move Left
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
			tempVector.x = -Mathf.Abs(transform.localScale.x);
			transform.localScale = tempVector;

			//play footsteps audio
			if (!GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().PlayOneShot(footstepSound);
		}
		else
		{
			anim.SetBool("isRunning", false);

			//Stop footsteps audio
			//if (GetComponent<AudioSource>().isPlaying)
				//GetComponent<AudioSource>().Stop();
		}
		body.AddForce(new Vector2(forceX, 0));
	}
	public void Jump(Animator anim, Rigidbody2D body)
	{
		float forceY = 0f;

		if (anim.GetBool("grounded"))
		{
			anim.SetBool("grounded", false);
			forceY = jumpForce;
			body.velocity = new Vector2(body.velocity.x, forceY);

			//Play jump sound
			if (GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().Stop();
			GetComponent<AudioSource>().PlayOneShot(jumpSound);
		}
	}
	public void MeleeAttack(Animator anim, Rigidbody2D body, GameObject closeAttack, Transform firingPoint)
	{
		if (anim.GetBool("grounded"))
			anim.Play("Attack");
		else anim.Play("JumpAttack");
		float direction = 2 * body.transform.localScale.x;
		closeAttack.GetComponent<StrikeAttackController>().attacker = this;
		Vector3 position = new Vector3(firingPoint.position.x - direction, firingPoint.position.y);
		Instantiate(closeAttack, position, firingPoint.rotation);
	}
	public void ThrowKunai(Animator anim, Rigidbody2D body, GameObject projective, Transform firingPoint, int adjustment)
	{
		if (anim.GetBool("grounded"))
			anim.Play("Throw");
		else anim.Play("JumpThrow");
		projective.GetComponent<KunaiThrowController>().thrower = this;
		Instantiate(projective, firingPoint.position, firingPoint.rotation);
		KunaiManager.AddNumbers(adjustment);
	}
	public void Die()
	{
		isAlive = false;
		GetComponent<Renderer>().enabled = false;
		foreach (var renderer in GetComponentsInChildren<Renderer>())
			renderer.enabled = false;
		EnableColliders(false);
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		GetComponent<Rigidbody2D>().gravityScale = 0f;

		if (tag == "Enemy")
		{
			if (GetComponent<LockPlayerController>() != null)
			{
				//Show message
				LockPlayerController lockController = GetComponent<LockPlayerController>();
				lockController.tipPane.msgShown = false;
				lockController.tipPane.ShowMessage(lockController.msgString, 5f);
				//
			}
			GetComponent<EnemyController>().DropItem();
		}
		//Disable Skills
		EnableSkills(false);
		enabled = false;
	}
	public void Respawn(float gravity, Vector3 respawnPosition, GameObject respawnParticle)
	{
		isAlive = true;
		health = maxHealth;
		enabled = true;
		GetComponent<Animator>().SetBool("isDead", false);
		transform.position = respawnPosition;
		GetComponent<Renderer>().enabled = true;
		foreach (var renderers in GetComponentsInChildren<Renderer>())
			renderers.enabled = true;
		GetComponent<Rigidbody2D>().gravityScale = gravity;
		EnableColliders(true);
		if (respawnParticle != null)
			Instantiate(respawnParticle, transform.position, transform.rotation);
		//Enable Skills
		EnableSkills(true);
	}
	public void ChangeDirection(int direction)
	{
		Vector3 tempVector = transform.localScale;
		tempVector.x = direction * Mathf.Abs(transform.localScale.x);
		transform.localScale = tempVector;
	}
	public bool IsGrounded(Transform groundCheck)
	{
		bool isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
		return isGround;
	}
	#region ColliderLogic
	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Ground")
			groundLayer = LayerMask.GetMask("Ground");
		else if ((coll.gameObject.tag == "Enemy" || coll.gameObject.tag == "Player")
			&& transform.position.y > coll.transform.position.y + 2.1f)
			groundLayer = LayerMask.GetMask("Figure");
	}
	void OnCollisionExit2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Enemy" || coll.gameObject.tag == "Player")
			groundLayer = LayerMask.GetMask("Ground");
	}
	#endregion
	private void EnableColliders(bool sw)
	{
		colliders = GetComponents<Collider2D>();
		foreach (var col in colliders)
		{
			col.enabled = sw;
		}
		Collider2D[] childColliders = GetComponentsInChildren<Collider2D>();
		foreach (var col in childColliders)
		{
			col.enabled = sw;
		}
	}
	private void EnableSkills(bool sw)
	{
		if (GetComponents<Skill>().Length != 0)
		{
			Skill[] skills = GetComponents<Skill>();
			foreach (var skill in skills)
			{
				skill.enabled = (sw) ? true : false;
			}
        }
	}
}
