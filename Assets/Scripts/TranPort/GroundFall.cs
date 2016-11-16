using UnityEngine;
using System.Collections;

public class GroundFall : MonoBehaviour {

    public Rigidbody2D groundFall;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void OnTriggerEnter2D(Collider2D target)
    {
        if(target.name=="Player")
        {
            StartCoroutine("FallDelayCo");
        }
    }
    public IEnumerator FallDelayCo()
    {
        yield return new WaitForSeconds(0.3f);
        groundFall.isKinematic = false;
    }
}
