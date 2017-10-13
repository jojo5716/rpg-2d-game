using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    // Movements
    public float speed = 4f;

    // Collider
    private Rigidbody2D rb2d;
    private Vector2 movement;

    // Animations
    private Animator playerAnimation;

    // Initial map
    public GameObject initialMap;


	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();

        // Executing SetBount method from CameraController.cs
        Camera.main.GetComponent<CameraController>().SetBound(initialMap);
    }
	
	// Update is called once per frame
	void Update () {
        movement = playerMovement();

        if (HasUpdatePlayerAnimation()) {
            ChangePlayerPositionView(movement);
            changePlayerAnimationToWalking();
        }
        else {
            ChangePlayerAnimationToIdle();
        }

	}

    void FixedUpdate() {
        MovePlayer(movement);
    }

    // Start Private methods

    // Method to get the horizontal orientation when user press
    // on array keywords or A or D keyword
    // Return:
    //    0: Any keyword is pressed
    //    1: "D" or rigth array keyword is pressed
    //   -1: "A" or left array keyword is pressed
    private float OnHorizontalKeywordPressed() {
        return Input.GetAxisRaw("Horizontal");
    }

    // Method to get the vertical orientation when user press
    // on array keywords or W or S keyword
    // Return:
    //    0: Any keyword is pressed
    //    1: "W" or up array keyword is pressed
    //   -1: "S" or bottom array keyword is pressed
    private float OnVerticalKeywordPressed()
    {
        return Input.GetAxisRaw("Vertical");
    }

    // Return a vector with the next player movement
    private Vector2 playerMovement() {
        return new Vector2(
            OnHorizontalKeywordPressed(),
            OnVerticalKeywordPressed()
        );
    }

    // Move the player to a position X, Y
    private void MovePlayer(Vector2 movement) {
        rb2d.MovePosition(rb2d.position + movement * speed * Time.deltaTime);
    }

    // Because the player only move when user press on keyword
    // Return true is player need move
    private bool HasUpdatePlayerAnimation() {
        return movement != Vector2.zero;
    }

    private void ChangePlayerPositionView(Vector2 movement) {
        playerAnimation.SetFloat("PositionX", movement.x);
        playerAnimation.SetFloat("PositionY", movement.y);
    }

    // When user press any array keywords, we change the player animation
    // to simulate is walking
    private void changePlayerAnimationToWalking() {
        playerAnimation.SetBool("walking", true);
    }

    // When user stop pressing any array keyword the player should be stop
    private void ChangePlayerAnimationToIdle() {
        playerAnimation.SetBool("walking", false);
    }
    // End Private methods
}
