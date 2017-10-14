using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    // Movements
    public float speed = 4f;

    // Player collider
    private Rigidbody2D rb2d;
    private Vector2 movement;

    // Animations
    private Animator playerAnimation;

    // Initial map
    public GameObject initialMap;

    // Attacking
    CircleCollider2D attackCollider;
    // Slash attack
    public GameObject slashPrefab;


    // Aura
    Aura aura;

    bool movePrevent;


	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();

        attackCollider = transform.GetChild(0).GetComponent<CircleCollider2D>();
        attackCollider.enabled = false;

        aura = transform.GetChild(1).GetComponent<Aura>();
        // Executing SetBount method from CameraController.cs
        Camera.main.GetComponent<CameraController>().SetBound(initialMap);
    }
	
	// Update is called once per frame
	void Update () {
        movement = PlayerMovement();

        if (HasUpdatePlayerAnimation()) {
            ChangePlayerPositionView(movement);
            changePlayerAnimationToWalking();
        }
        else {
            ChangePlayerAnimationToIdle();
        }

        SwordAttack();

        SlashAttack();

        PreventMovement();


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
    private Vector2 PlayerMovement() {
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

    // Set animation when space keyword is pressed
    private void SwordAttack() {
        AnimatorStateInfo stateInfo = playerAnimation.GetCurrentAnimatorStateInfo(0);
        bool isAttacking = stateInfo.IsName("Player_attack");

        if (Input.GetKeyDown("space") && !isAttacking) {
            playerAnimation.SetTrigger("attacking");
        }

        if (HasUpdatePlayerAnimation()) {
            attackCollider.offset = new Vector2(movement.x / 2, movement.y /2);
        }

        if (isAttacking) {
            float playBackTime = stateInfo.normalizedTime;

            if (playBackTime > 0.33 && playBackTime < 0.66) {
                attackCollider.enabled = true;
            } else {
                attackCollider.enabled = false;
            }
        }
    }

    private void SlashAttack() {
        AnimatorStateInfo stateInfo = playerAnimation.GetCurrentAnimatorStateInfo(0);
        bool isLoading = stateInfo.IsName("Player_slash");

        if (Input.GetKeyDown(KeyCode.LeftAlt)) {
            playerAnimation.SetTrigger("loading");
            aura.StartAura();
            
        } else if (Input.GetKeyUp(KeyCode.LeftAlt)) {
            playerAnimation.SetTrigger("attacking");

            if (aura.IsLoaded()) {
                float[] anglePosition = GetAnglePosition();

                float angle = Mathf.Atan2(
                    anglePosition[0],
                    anglePosition[1]
                ) * Mathf.Rad2Deg;

                // Creamos la instancia de Slash
                GameObject slashObj = Instantiate(slashPrefab, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));

                // Asignamos el movimiento
                Slash slash = slashObj.GetComponent<Slash>();
                slash.movement.x = playerAnimation.GetFloat("PositionX");
                slash.movement.y = playerAnimation.GetFloat("PositionY");
            }

            aura.StopAura();

            // Wait a few seconds to keep moving
            StartCoroutine(EnableMovementAfter(0.4f));
        }

    }

    private float[] GetAnglePosition() {
        return new float[] {
            playerAnimation.GetFloat("PositionY"),
            playerAnimation.GetFloat("PositionX")
        };
    }
    // When player is loading the power we can't move
    void PreventMovement() {
        if (movePrevent) {
            movement = Vector2.zero;
        }
    }

    // Waiting to move again
    IEnumerator EnableMovementAfter(float seconds) {
        yield return new WaitForSeconds(seconds);
        movePrevent = false;
    }

    // End Private methods
}
