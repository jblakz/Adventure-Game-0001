using UnityEngine;
using System.Collections;

public class KunaiThrowController : MonoBehaviour {

	public float speed;
	public Figure thrower;
	public float knockbackForceX;
	public float knockbackForceY;
	public GameObject damageEffect;
	public float kunaiDamage;

	private Rigidbody2D body;
	private int direction = 1;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D>();
		thrower.GetComponent<Animator>().SetBool("isThrowing", true);
		thrower.isThrowing = true;
		if(thrower.transform.localScale.x < 0)
            direction = -direction;
		speed = direction*speed;
    }
	
	// Update is called once per frame
	void Update () {
		body.velocity = new Vector2(speed, body.velocity.y);
		StartCoroutine("KunaiThrowCo");
    }
	public IEnumerator KunaiThrowCo()
	{
		yield return new WaitForSeconds(0.5f);
		Destroy(gameObject);
		thrower.isThrowing = false;
	}
	void OnTriggerEnter2D(Collider2D target)
	{
		if (target.tag != thrower.tag)
		{
			if (thrower.transform.localScale.x < 0)
				direction = -direction;
			target.GetComponent<Rigidbody2D>().velocity = new Vector2(direction * knockbackForceX, knockbackForceY);
			if (target.tag == "Enemy")
				SupriseEnemy(target);
            Instantiate(damageEffect, target.transform.position, target.transform.rotation);
			
			if (target.GetComponent<Figure>().health < kunaiDamage)
				target.GetComponent<Figure>().health = 0f;
			else target.GetComponent<Figure>().health -= kunaiDamage;

			gameObject.GetComponent<Renderer>().enabled = false;
			gameObject.GetComponent<Collider2D>().enabled = false;
		}
	}
	void SupriseEnemy(Collider2D target)
	{
		EnemyController receiver = target.GetComponent<EnemyController>();
		//Check if thrower and receiver facing same direction
		if (thrower.transform.localScale.x * receiver.transform.localScale.x > 0)
			receiver.ChangeDirection(-direction);
	}
}
