using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class VictoryScreen : MonoBehaviour
{

    // transition
    [SerializeField] private Image blackscreen;
    [SerializeField] private Animator anim;

    public static VictoryScreen Instance;

    [SerializeField]
    public GameObject _gameCanvas;

    [SerializeField] private GameObject[] _menu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // make victory screen inactive
        foreach (GameObject panel in _menu)
        {
            panel.SetActive(false);
        }



    }

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartVictoryScreen()
    {

        // reappear all objects
        foreach (GameObject panel in _menu)
        {
            panel.SetActive(true);

        }
    }

    public void LoadShopScene()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        anim.SetBool("fade", true);
        yield return new WaitUntil(() => blackscreen.color.a == 1);
        SceneManager.LoadScene("Shop (Nick)");
    }
}
