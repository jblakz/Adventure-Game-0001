using UnityEngine;

using System.Collections;



public class EnemyController : Figure
{
	public SpriteRenderer indicator;
	public Transform strikeRadius;
	public Transform firingPoint;
	public Transform awareRadius;
	public Transform lockingRadius;
	public Transform groundCheck;
	public GameObject closeAttack;
	public GameObject projective;
	public GameObject damageEffect;
	public GameObject droppedItem;
	public Canvas healthCanvas;
	public Vector3 spawnPosition;

	public void LockingCheck(PlayerController player)
	{
		if (lockingRadius != null)
		{
			if (lockingRadius.GetComponent<Collider2D>().IsTouching(player.GetComponent<Collider2D>()))
			{
				GetComponent<LockPlayerController>().enabled = true;
			}
			else
			{
				GetComponent<LockPlayerController>().enabled = false;
			}
		}
	}
	public void DropItem()
	{
		if (droppedItem != null)
		{
			if (droppedItem.GetComponent<KunaiPackController>() != null)
				droppedItem.GetComponent<KunaiPackController>().isDropped = true;
            Instantiate(droppedItem, transform.position, transform.rotation);
		}
	}
	public void FixHealthCanvas()
	{
		if (healthCanvas != null)
		{
			Vector3 tempVector = healthCanvas.transform.localScale;
			if ((transform.localScale.x < 0f && tempVector.x > 0)
				|| (transform.localScale.x > 0f && tempVector.x < 0))
			{
				tempVector.x = -tempVector.x;
				healthCanvas.transform.localScale = tempVector;
			}
		}
	}
}