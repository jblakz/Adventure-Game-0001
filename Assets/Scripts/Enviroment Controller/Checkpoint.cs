using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	public LevelController lvController;

	// Use this for initialization
	void Start () {
		lvController = FindObjectOfType<LevelController>();
	}
	
	// Update is called once per frame
	void Update () {

	}
	void OnTriggerEnter2D(Collider2D target)
	{
		lvController.currentCheckpoint = transform;
	}
}
