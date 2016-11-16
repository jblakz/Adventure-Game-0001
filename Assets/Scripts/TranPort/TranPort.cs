using UnityEngine;
using System.Collections;

public class TranPort : MonoBehaviour {

    public float moveSpeed;
    public bool moveLeft;
    public bool moveRight;  
    public Rigidbody2D tranport;

    public Transform leftWallCheck;
    public Transform rightWallCheck;
    public float wallCheckRadius;
    public LayerMask whatIsWall;
    private bool leftWalled;
    private bool rightWalled;
    // Use this for initialization
    void Start () {
        tranport = GetComponent<Rigidbody2D>();
        moveLeft = true;
    }
	
	// Update is called once per frame
	void Update () {
        leftWalled = Physics2D.OverlapCircle(leftWallCheck.position, wallCheckRadius, whatIsWall);
        rightWalled = Physics2D.OverlapCircle(rightWallCheck.position, wallCheckRadius, whatIsWall);
        if (leftWalled)
        {

            moveRight = true;

        }
        if (rightWalled)
        {
            moveLeft = true;
        }
        
        if (moveRight)
        {
            tranport.velocity = new Vector2(moveSpeed, tranport.velocity.y);
            moveRight = false;

        }
        if(moveLeft)
        {
            tranport.velocity = new Vector2(-moveSpeed, tranport.velocity.y);
            moveLeft = false;
        }
    }
}
