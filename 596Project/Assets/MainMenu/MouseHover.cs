using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MouseHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    private TMP_Text textmesh;
    [SerializeField] private bool isPlay;
    [SerializeField] private bool isQuit;
    [SerializeField] private Image BlackScreen;
    [SerializeField] private Animator anim;

    void Start() { // colour is default white
        textmesh = GetComponent<TMP_Text>();
        textmesh.color = Color.white;
    }

    // set colour to grey when hovering
    public void OnPointerEnter(PointerEventData eventData) { textmesh.color = Color.grey * 1.2f; }
    public void OnPointerExit(PointerEventData eventData) { textmesh.color = Color.white; }

    // activates when button is pressed
    public void OnPointerClick(PointerEventData eventData) {
        if (isPlay) {
            Debug.Log("Loading Level 1 scene");
            StartCoroutine(Fading("Attack"));
        }
        if (isQuit) {
            Debug.Log("Quitting application");
            Application.Quit(); 
        }
    }

    // manage fade transition
    IEnumerator Fading(string scene) {
        anim.SetBool("fade", true);
        yield return new WaitUntil(()=>BlackScreen.color.a==1);
        SceneManager.LoadScene(scene);
    }
}
