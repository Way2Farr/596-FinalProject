using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField]
    private Camera _cam;
    [SerializeField]
    private float _camYOffset, _camZOffset, _camRotationOffset;

    private Vector3 centerPos;
    private float centerFOV = 0f;
    [SerializeField]
    private float lerpSpeed;

    private void Awake() {
        Instance = this;
    }

    void Start() {
        StartCoroutine(Center()); // gets center camera position on Start
    }

    IEnumerator Center() {
        yield return null; // waits one frame (after Start() has concluded)
        getCenter();
    }

    private void getCenter() {
        centerPos = _cam.transform.position;
        centerFOV = _cam.fieldOfView;
    }

    private void setCenter() {
        _cam.transform.position = Vector3.Lerp(_cam.transform.position, centerPos, Time.deltaTime * lerpSpeed);
        _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, centerFOV, Time.deltaTime * lerpSpeed);
    }

    void Update() { 
        if ((GameManager.Instance.State == GameManager.GameState.ChooseOption) && (centerFOV != 0f)) {
            setCenter();
        }
        if (UnitManager.Instance._startMoving && GameManager.Instance.State == GameManager.GameState.PlayerMove && UnitManager.Instance._startingTile != null && UnitManager.Instance._endTile != null) {
            Vector3 movePosition = new Vector3(UnitManager.Instance.Player.transform.position.x, UnitManager.Instance.Player.transform.position.y + (_camYOffset * 1.5f), _camZOffset);
            _cam.transform.position = Vector3.Lerp(_cam.transform.position, movePosition, Time.deltaTime * lerpSpeed);
            _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, 30f, Time.deltaTime * lerpSpeed);
        }
    }
}
