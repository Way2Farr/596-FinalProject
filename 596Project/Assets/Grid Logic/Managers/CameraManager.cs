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
    [SerializeField]
    private float zoomOut = 65f;
    [SerializeField]
    private float zoomIn = 20f;
    [SerializeField]
    private float moveAdjust = 1.25f;
    [SerializeField]
    private float lerpSpeed;

    private bool victory = false;
    private bool menu = false;
    private bool playerMoving = false;
    private bool enemyTurn = false;
    //private bool enemyMoving = false;

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
        //centerFOV = _cam.fieldOfView;
    }

    private void setCenter() {
        _cam.transform.position = Vector3.Lerp(_cam.transform.position, centerPos, Time.deltaTime * lerpSpeed);
        _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, zoomOut, Time.deltaTime * lerpSpeed);
    }

    void Update() {
        if (UnitManager.Instance._startMoving && GameManager.Instance.State == GameManager.GameState.PlayerMove && UnitManager.Instance._startingTile != null && UnitManager.Instance._endTile != null) {
            playerMoving = true;
        } else playerMoving = false;

        if (GameManager.Instance.State == GameManager.GameState.EnemyChoose || GameManager.Instance.State == GameManager.GameState.EnemyMove) {
            enemyTurn = true;
        } else enemyTurn = false;

        if ((UnitManager.Instance.Player.MenuShow && GameManager.Instance.State == GameManager.GameState.ChooseOption) || (GameManager.Instance.MovedMessage == true && UnitManager.Instance.Player.MenuShow)) {
            menu = true;
        } else menu = false;

        if (GameManager.Instance.State == GameManager.GameState.Victory) {
            victory = true;
        } else victory = false;

        if (playerMoving) {
            Vector3 movePosition = new Vector3(UnitManager.Instance.Player.transform.position.x, UnitManager.Instance.Player.transform.position.y + (_camYOffset * moveAdjust), _camZOffset);
            _cam.transform.position = Vector3.Lerp(_cam.transform.position, movePosition, Time.deltaTime * lerpSpeed);
            _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, zoomIn, Time.deltaTime * lerpSpeed);
        } else if (enemyTurn) {
            Vector3 movePosition = new Vector3(UnitManager.Instance.Enemy.transform.position.x, UnitManager.Instance.Enemy.transform.position.y + (_camYOffset * 0.85f * moveAdjust), _camZOffset);
            _cam.transform.position = Vector3.Lerp(_cam.transform.position, movePosition, Time.deltaTime * lerpSpeed);
            _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, zoomIn, Time.deltaTime * lerpSpeed);
        } else if (menu) {
            Vector3 movePosition = new Vector3(UnitManager.Instance.Player.transform.position.x * moveAdjust, UnitManager.Instance.Player.transform.position.y + (_camYOffset * moveAdjust), _camZOffset);
            _cam.transform.position = Vector3.Lerp(_cam.transform.position, movePosition, Time.deltaTime * lerpSpeed);
            _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, zoomIn, Time.deltaTime * lerpSpeed);
        } else if (victory) {
            Vector3 movePosition = new Vector3(UnitManager.Instance.Player.transform.position.x * moveAdjust, UnitManager.Instance.Player.transform.position.y + (_camYOffset * moveAdjust), _camZOffset);
            _cam.transform.position = Vector3.Lerp(_cam.transform.position, movePosition, Time.deltaTime * lerpSpeed);
            _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, zoomIn, Time.deltaTime * lerpSpeed);
        } else { 
            setCenter(); 
        }

    }
}
