using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{

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
        SceneManager.LoadScene("Shop (Nick)");
    }
}
