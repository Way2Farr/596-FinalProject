using System;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _stateText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        GameManager.OnStateChange += GameManagerOnOnStateChange;
    }


    private void OnDestroy()
    {
        GameManager.OnStateChange -= GameManagerOnOnStateChange;
    }
    private void GameManagerOnOnStateChange(GameManager.GameState state)
    {
        throw new NotImplementedException();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
