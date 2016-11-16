using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	private Transform player;

	public float minX, maxX;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (player != null)
		{
			Vector3 vector = transform.position;
			vector.x = player.position.x + 3f;
			if (vector.x < minX)
				vector.x = minX;
			if (vector.x > maxX)
				vector.x = maxX;
			transform.position = vector;
		}
	}
}
