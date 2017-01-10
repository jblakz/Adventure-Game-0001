using UnityEngine;
using System.Collections;

public class KunaiThrowController : MonoBehaviour {

	public Figure thrower;
	public GameObject damageEffect;
	public Vector2 knockbackForce;
	public AudioClip impactSound;

	public float kunaiDamage;
	public float speed;
	private float delay;
	private float range;

	private Rigidbody2D body;
	private float direction = 1f;
	private float distance;

	// Use this for initialization
	void Start () {

		kunaiDamage = thrower.throwDamage;
		range = thrower.throwRange;
		delay = 1f / thrower.throwSpeed;
		speed = thrower.projectiveSpeed;
		if (thrower.knockbackForce != new Vector2())
			knockbackForce = thrower.knockbackForce;

		body = GetComponent<Rigidbody2D>();
		thrower.GetComponent<Animator>().SetBool("isThrowing", true);
		thrower.isThrowing = true;
		if(thrower.transform.localScale.x < 0)
            direction = -direction;
		speed = direction*speed;
    }
	
	// Update is called once per frame
	void Update () {
		if (thrower != null)
			if (direction < 0)
				distance = thrower.transform.position.x - body.transform.position.x;
			else distance = body.transform.position.x - thrower.transform.position.x;
		if (distance > range)
		{
			gameObject.GetComponent<Collider2D>().enabled = false;
			gameObject.GetComponent<Renderer>().enabled = false;
		}
		body.velocity = new Vector2(speed, body.velocity.y);
		StartCoroutine("KunaiThrowCo");
    }
	public IEnumerator KunaiThrowCo()
	{
		yield return new WaitForSeconds(delay);
		Destroy(gameObject);
		thrower.isThrowing = false;
	}
	void OnTriggerEnter2D(Collider2D target)
	{
		if (target.tag != thrower.tag)
		{
			direction = 2 * thrower.transform.localScale.x;
			target.GetComponent<Rigidbody2D>().velocity = new Vector2(direction * knockbackForce.x, knockbackForce.y);
			Instantiate(damageEffect, target.transform.position, target.transform.rotation);
			
			if (target.GetComponent<Figure>().health < kunaiDamage)
				target.GetComponent<Figure>().health = 0f;
			else target.GetComponent<Figure>().health -= kunaiDamage;

			gameObject.GetComponent<Renderer>().enabled = false;
			gameObject.GetComponent<Collider2D>().enabled = false;

			if (target.tag == "Enemy") {
				LevelController.focusedEnemy = target.GetComponent<EnemyController>();
				SupriseEnemy(target);
			}

			//Play impact audio
			if(target.GetComponent<AudioSource>().isPlaying)
				target.GetComponent<AudioSource>().Stop();
			target.GetComponent<AudioSource>().PlayOneShot(impactSound);
		}
	}
	void SupriseEnemy(Collider2D target)
	{
		EnemyController receiver = target.GetComponent<EnemyController>();
		//Check if thrower and receiver facing same direction
		if (thrower.transform.localScale.x * receiver.transform.localScale.x > 0)
			receiver.ChangeDirection(-(int)direction);
	}
}
