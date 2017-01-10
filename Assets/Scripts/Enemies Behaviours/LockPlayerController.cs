using UnityEngine;

using System.Collections;
using UnityEngine.UI;

public class LockPlayerController : MonoBehaviour
{
	public TipPaneController tipPane;
	public GameObject wallLeft;
	public GameObject wallRight;
	public string msgString;
	public string warningString;
	

	private bool isAlive;
	private GameObject levelExit;
	private CameraFollow mainCam;
	private Transform lockingRadius;
	private PlayerController player;

	void Start()
	{
		lockingRadius = GetComponent<EnemyController>().lockingRadius;
		player = FindObjectOfType<PlayerController>();
		levelExit = FindObjectOfType<LevelExit>().gameObject;
		mainCam = FindObjectOfType<CameraFollow>();
		tipPane = FindObjectOfType<TipPaneController>();
	}
	void Update()
	{
		isAlive = GetComponent<EnemyController>().isAlive;
		if (lockingRadius.GetComponent<Collider2D>().IsTouching(player.GetComponent<Collider2D>())
			&& isAlive)
		{
			LockPlayer(true);
			if (warningString != "")
			{
				tipPane.ShowMessage(warningString, -1f);
            }
		}
		else
		{
			LockPlayer(false);
		}
	}
	public void LockPlayer(bool sw)
	{
		if (wallLeft != null && mainCam != null && levelExit != null)
		{
			wallLeft.GetComponent<Renderer>().enabled = (sw) ? true : false;
			wallLeft.GetComponent<Collider2D>().enabled = (sw) ? true : false;
			mainCam.minX = (sw) ? wallLeft.transform.position.x + 10f : 1f;
		}
		if (wallRight != null && mainCam != null && levelExit != null)
		{
			wallRight.GetComponent<Renderer>().enabled = (sw) ? true : false;
			wallRight.GetComponent<Collider2D>().enabled = (sw) ? true : false;
            mainCam.maxX = (sw) ? wallRight.transform.position.x - 10f : levelExit.transform.position.x - 5f;
		}
		if (levelExit != null)
		{
			levelExit.GetComponent<Renderer>().enabled = (sw) ? false : true;
			levelExit.GetComponent<Collider2D>().enabled = (sw) ? false : true;
			levelExit.GetComponent<LevelExit>().enabled = (sw) ? false : true;
		}
	}
}