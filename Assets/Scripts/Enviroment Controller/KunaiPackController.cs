using UnityEngine;
using System.Collections;

public class KunaiPackController : MonoBehaviour
{
	public bool isDropped;
	public int number;
	public float coolDown;
	public AudioClip pickupSound;

	private bool isEnabled;
	private float timer;
	void Awake()
	{
		isEnabled = true;
		timer = coolDown;
	}
	void FixedUpdate()
	{
		if (!isDropped)
			TimerCountdown();
    }
	void OnTriggerEnter2D(Collider2D target)
	{
		if (target.GetComponent<PlayerController>() == null)
			return;
		KunaiManager.AddNumbers(number);

		//pick up sound
		if (target.GetComponent<AudioSource>().isPlaying)
			target.GetComponent<AudioSource>().Stop();
		target.GetComponent<AudioSource>().PlayOneShot(pickupSound);
		if (!isDropped)
			Enable(false);
		else Destroy(gameObject);
	}
	public void Enable(bool sw)
	{
		isEnabled = (sw) ? true : false;
		GetComponent<Renderer>().enabled = (sw) ? true : false;
		GetComponent<Collider2D>().enabled = (sw) ? true : false;
	}
	private void TimerCountdown()
	{
		if (!isEnabled)
		{
			if (timer >= 0)
				timer -= Time.deltaTime;
			else
			{
				Enable(true);
				timer = coolDown;
			}
		}
	}
}
