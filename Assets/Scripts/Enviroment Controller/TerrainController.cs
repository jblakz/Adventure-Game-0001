using UnityEngine;

using System.Collections;



public class TerrainController : MonoBehaviour
{
	private PlayerController player;
	private FinalBossController boss;
	private Collider2D coll;

	void Start()
	{
		player = FindObjectOfType<PlayerController>();
		boss = FindObjectOfType<FinalBossController>();
		coll = GetComponent<Collider2D>();
	}

	void Update()
	{
		if (player.transform.position.y - 0.5f <= transform.position.y)
		{
			if(boss != null)
				if (boss.transform.position.y - 0.5f <= transform.position.y)
					coll.enabled = false;
				else coll.enabled = true;
			else coll.enabled = false;
		}
		else
		{
			coll.enabled = true;
		}
	}

}