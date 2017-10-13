using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    // Movements
    public float speed = 4f;

    // Collider
    private Rigidbody2D rb2d;
    private Vector2 movement;


	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();	
	}
	
	// Update is called once per frame
	void Update () {
        movement = playerMovement();

	}

    void FixedUpdate() {
        movePlayer(movement);
    }

    // Start Private methods

    // Method to get the horizontal orientation when user press
    // on array keywords or A or D keyword
    // Return:
    //    0: Any keyword is pressed
    //    1: "D" or rigth array keyword is pressed
    //   -1: "A" or left array keyword is pressed
    private float onHorizontalKeywordPressed() {
        return Input.GetAxisRaw("Horizontal");
    }

    // Method to get the vertical orientation when user press
    // on array keywords or W or S keyword
    // Return:
    //    0: Any keyword is pressed
    //    1: "W" or up array keyword is pressed
    //   -1: "S" or bottom array keyword is pressed
    private float onVerticalKeywordPressed()
    {
        return Input.GetAxisRaw("Vertical");
    }

    // Return a vector with the next player movement
    private Vector2 playerMovement() {
        return new Vector2(
            onHorizontalKeywordPressed(),
            onVerticalKeywordPressed()
        );
    }

    private void movePlayer(Vector2 movement) {
        rb2d.MovePosition(rb2d.position + movement * speed * Time.deltaTime);
    }
    // End Private methods
}
