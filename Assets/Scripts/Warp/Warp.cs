using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour {
    // Targets
    public GameObject target;
    public GameObject targetMap;

    // Transition effect
    bool start = false;
    bool isFadeIn = false;
    float alpha = 0;
    float fadeTime = 1f;

    // Area name
    GameObject area;

    void Awake() {
        // Hide the image
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            FadeIn();

            yield return new WaitForSeconds(fadeTime);

            // When player is on the warp entrance
            // we have to change their position to the target warp
            collision.transform.position = target.transform.GetChild(0).transform.position;

            // We execute the method SetBound on the CameraController.cs
            Camera.main.GetComponent<CameraController>().SetBound(targetMap);

            FadeOut();

        }
    }

    void OnGUI() {
        if (!start) {
            return;
        }

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.g, alpha);

        Texture2D texture;
        texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.black);
        texture.Apply();

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);

        if (isFadeIn) {
            alpha = Mathf.Lerp(alpha, 1.1f, fadeTime * Time.deltaTime);
        } else {
            alpha = Mathf.Lerp(alpha, -0.1f, fadeTime * Time.deltaTime);

            if (alpha < 0) {
                start = false;
            }
        }
    }

    void FadeIn() {
        start = true;
        isFadeIn = true;
    }

    void FadeOut() {
        isFadeIn = false;
    }
}
