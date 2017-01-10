using UnityEngine;
using System.Collections;

public class ScrollScript : MonoBehaviour {

	public float scrollSpeed;

	// Update is called once per frame
	void Update () {
		GetComponent<Renderer>().material.mainTextureOffset = new Vector2(Time.time * scrollSpeed % 1, 0f);
	}
}
