using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    // Target
    Transform target;
    Vector2 velocity;
    public float smoothTime = 3f;

    // Limits
    float topLeftX, topLeftY, bottomRigthX, bottomRigthY;

    // Use this for initialization
    void Awake() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start() {
        Screen.SetResolution(800, 800, true);
    }


    // Update is called once per frame
    void Update() {
        if (!Screen.fullScreen || Camera.main.aspect != 1) {
            SetScreenResolution();
        }

        if (Input.GetKey("escape")) {
            OnQuitGame();
        }

        float positionX = Mathf.Round(
            Mathf.SmoothDamp(transform.position.x, target.position.x, ref velocity.x, smoothTime
        ) * 100) / 100;

        float positionY = Mathf.Round(
            Mathf.SmoothDamp(transform.position.y, target.position.y, ref velocity.y, smoothTime
            ) * 100) / 100;

        transform.position = new Vector3(
            Mathf.Clamp(positionX, topLeftX, bottomRigthX),
            Mathf.Clamp(positionY, bottomRigthY, topLeftY),
            transform.position.z
        );
    }

    private void SetScreenResolution() {
        Screen.SetResolution(800, 800, true);
    }

    private void OnQuitGame() {
        Application.Quit();
    }

    public void SetBound(GameObject map) {
        Tiled2Unity.TiledMap config = map.GetComponent<Tiled2Unity.TiledMap>();
        float cameraSize = Camera.main.orthographicSize;

        topLeftX = map.transform.position.x + cameraSize;
        topLeftY = map.transform.position.y - cameraSize;

        bottomRigthX = map.transform.position.x + config.NumTilesWide - cameraSize;
        bottomRigthY = map.transform.position.y - config.NumTilesHigh + cameraSize;

        FastMove();
    }

    public void FastMove() {
        transform.position = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z
        );
    }
}
