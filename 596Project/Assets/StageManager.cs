using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [SerializeField] public Slider _sliderOne = null;
    [SerializeField] public Slider _sliderTwo = null;
    [SerializeField] public Slider _slider = null;
    [SerializeField] public float _rateOfIncrease = 5f;
    [SerializeField] public Button nextButton = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float elapsedTime=0;
    private bool isFillingToHalf = true;
    private bool isFillingToFull = false;
    public GameObject gameStats;
    public StatManager statManager = null;

    void Awake()
    {
        gameStats = GameObject.FindGameObjectWithTag("StatManager");
        statManager = gameStats.GetComponent<StatManager>();
        if(statManager._currentRound == 2)
        {
            _slider = _sliderTwo;
            _sliderOne.value = 1f;
            Debug.Log("Round 2 Done");
        }
    }
    void Start()
    {
        nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isFillingToHalf && _slider.value < 0.5f) 
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _rateOfIncrease;
            _slider.value = Mathf.Lerp(_slider.minValue, 0.5f, t);
        }
        if (isFillingToFull)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _rateOfIncrease;
            _slider.value = Mathf.Lerp(0.5f, _slider.maxValue, t);

            if (_slider.value >= 0.99f)
            {
                _slider.value = 1f;
                isFillingToFull = false;
            }
        }

    }
    
    void OnNextButtonClicked()
    {
        // Reset time and start second phase
        elapsedTime = 0;
        isFillingToHalf = false;
        isFillingToFull = true;
    }
}
