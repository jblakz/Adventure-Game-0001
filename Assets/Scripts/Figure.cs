using UnityEngine;
using System.Collections;

public abstract class Figure : MonoBehaviour {

	public float health;
	public float maxHealth;
	public float attackDamage;
	public float attackRange;
	public float attackSpeed;   //Number of attack per 1 sec
	public bool healthDisplayed = false;

	public bool isAttacking;
	public bool isThrowing;

	private Collider2D[] colliders;

	public void Die()
	{
		enabled = false;
		GetComponent<Renderer>().enabled = false;
		foreach (var renderers in GetComponentsInChildren<Renderer>())
			renderers.enabled = false;
		ToggleColliders(false);
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		GetComponent<Rigidbody2D>().gravityScale = 0f;
	}
	public void Respawn(float gravity, Vector3 respawnPosition, GameObject respawnParticle)
	{
		health = maxHealth;
		enabled = true;
		healthDisplayed = false;
		GetComponent<Animator>().SetBool("isDead", false);
		transform.position = respawnPosition;
		GetComponent<Renderer>().enabled = true;
		foreach (var renderers in GetComponentsInChildren<Renderer>())
			renderers.enabled = true;
		GetComponent<Rigidbody2D>().gravityScale = gravity;
		ToggleColliders(true);
		if (respawnParticle != null)
			Instantiate(respawnParticle, transform.position, transform.rotation);
	}
	void ToggleColliders(bool toggle)
	{
		colliders = GetComponents<Collider2D>();
		foreach (var col in colliders)
		{
			col.enabled = toggle;
		}
	}
}
