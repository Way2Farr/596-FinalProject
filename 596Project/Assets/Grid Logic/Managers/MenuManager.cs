using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    [SerializeField] private GameObject[] _menu;
    [SerializeField] private TextMeshProUGUI _stateText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        Instance = this;
        GameManager.OnStateChange += GameManagerOnOnStateChange;
    }


    private void OnDestroy()
    {
        GameManager.OnStateChange -= GameManagerOnOnStateChange;
    }
    private void GameManagerOnOnStateChange(GameManager.GameState state)
    {
        if(state == GameManager.GameState.ChooseOption)
        {
            Debug.Log("Activate menu.");
            foreach(GameObject panel in _menu)
            {
                panel.SetActive(true);
            }
        }
        else
        {
            Debug.Log("Deactivate menu.");
            foreach (GameObject panel in _menu)
            {
                panel.SetActive(false);
            }
        }
        //throw new NotImplementedException();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
