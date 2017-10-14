using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour {
    [Tooltip("Wait x seconds before destroy the object")]
    public float waitBeforeDestroy;

    [HideInInspector]
    public Vector2 movement;

    public float speed;

    // Update is called once per frame
    void Update() {
        transform.position += new Vector3(movement.x, movement.y) * speed * Time.deltaTime;
    }

    IEnumerator OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Object") {
            yield return new WaitForSeconds(waitBeforeDestroy);
            Destroy(gameObject);
        } else if (col.tag != "Player" && col.tag != "Attack") {
            Destroy(gameObject);
        }
    }
}
