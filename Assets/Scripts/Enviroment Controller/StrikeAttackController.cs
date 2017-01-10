using UnityEngine;
using System.Collections;

public class StrikeAttackController : MonoBehaviour {

	public Figure attacker;
	public GameObject damageEffect;
	public Vector2 knockbackForce;
	public float projectiveSpeed;
	public AudioClip impactSound;

	private float attackDamage;
	private float range;
	private float delay;
	private Rigidbody2D body;
	private float direction = 1f;
	private float distance;

	// Use this for initialization
	void Start()
	{
		attackDamage = attacker.attackDamage;
		range = attacker.attackRange;
		delay = 1f / attacker.attackSpeed;
		if (attacker.knockbackForce != new Vector2())
			knockbackForce = attacker.knockbackForce;

		body = GetComponent<Rigidbody2D>();
		attacker.GetComponent<Animator>().SetBool("isAttacking", true);
		attacker.isAttacking = true;
		if (attacker.transform.localScale.x < 0)
			direction = -direction;
	}

	// Update is called once per frame
	void Update()
	{
		if (attacker != null)
			if (direction < 0)
				distance = attacker.transform.position.x - body.transform.position.x;
			else distance = body.transform.position.x - attacker.transform.position.x;
		if (distance > range)
		{
			gameObject.GetComponent<Collider2D>().enabled = false;
		}
        body.velocity = new Vector2(direction * projectiveSpeed, body.velocity.y);
		StartCoroutine("AttackDelayCo");
	}
	public void OnTriggerEnter2D(Collider2D target)
	{
		if (target.tag != attacker.tag)
		{
			direction = 2 * attacker.transform.localScale.x;
			target.GetComponent<Rigidbody2D>().velocity = new Vector2(direction * knockbackForce.x, knockbackForce.y);
			Instantiate(damageEffect, target.transform.position, target.transform.rotation);
			
			if (target.GetComponent<Figure>().health < attackDamage)
				target.GetComponent<Figure>().health = 0f;
			else target.GetComponent<Figure>().health -= attackDamage;

			gameObject.GetComponent<Collider2D>().enabled = false;

			if (target.tag == "Enemy")
			{
				LevelController.focusedEnemy = target.GetComponent<EnemyController>();
				SupriseEnemy(target);
			}

			//Play impact audio
			if (target.GetComponent<AudioSource>().isPlaying)
				target.GetComponent<AudioSource>().Stop();
			target.GetComponent<AudioSource>().PlayOneShot(impactSound);
		}
	}
	void SupriseEnemy(Collider2D target)
	{
		EnemyController receiver = target.GetComponent<EnemyController>();
		//Check if thrower and receiver facing same direction
		if (attacker.transform.localScale.x * receiver.transform.localScale.x > 0)
			receiver.ChangeDirection(-(int)direction);
	}
	//Co-routine
	public IEnumerator AttackDelayCo()
	{
		yield return new WaitForSeconds(delay);
		attacker.isAttacking = false;
		Destroy(gameObject);
	}
}
