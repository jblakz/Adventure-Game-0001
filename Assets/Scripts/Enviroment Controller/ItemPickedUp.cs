using UnityEngine;
using System.Collections;

public class ItemPickedUp : MonoBehaviour
{

	public int number;

	void OnTriggerEnter2D(Collider2D target)
	{
		if (target.GetComponent<PlayerController>() == null)
			return;
		KunaiManager.AddNumbers(number);
		Destroy(gameObject);
	}
}
