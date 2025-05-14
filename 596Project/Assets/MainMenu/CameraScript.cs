using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    [SerializeField] private GameObject background;
    private Camera mainCamera;

    void Start() {
        // instantiate background
        mainCamera = GetComponent<Camera>();
        AdjustCameraToBackground();
    }

    void Update() { 
        // always adjust camera
        AdjustCameraToBackground();
    }

    // function to adjust camera to the background
    void AdjustCameraToBackground()
    {
        SpriteRenderer backgroundSprite = background.GetComponent<SpriteRenderer>();

        // use bounds of background
        Bounds backgroundBounds = backgroundSprite.bounds;
        float backgroundWidth = backgroundBounds.size.x;
        float backgroundHeight = backgroundBounds.size.y;

        // calculate aspect ratios
        float aspectScreen = (float)Screen.width / Screen.height;
        float aspectBackground = backgroundWidth / backgroundHeight;

        // fix orthographic size
        if (aspectScreen >= aspectBackground) mainCamera.orthographicSize = backgroundHeight / 2;
        else mainCamera.orthographicSize = (backgroundWidth / aspectScreen) / 2;

        // reposition camera
        mainCamera.transform.position = new Vector3(
            backgroundBounds.center.x,
            backgroundBounds.center.y,
            mainCamera.transform.position.z
        );
    }
}
