using TMPro;
using UnityEngine;

public class GameStateDebugger : MonoBehaviour
{
    [SerializeField] TextMeshPro _gameStateText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _gameStateText.text = "Game State:" + nameof(GameManager.GameState);
    }
}
