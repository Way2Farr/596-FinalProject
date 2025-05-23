using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    //___________________________________________________________________________________\\
    [Header("UI Stats")]
    public TMP_Text unitHEALTH;
    public TMP_Text unitATK;
    public TMP_Text unitDEF;
    public TMP_Text unitRAN;
    public TMP_Text unitSPD;
    public TMP_Text unitName;
    public UnityEngine.UI.Image unitIMG;

    //___________________________________________________________________________________\\
    public TMP_Text eventMsg;

    public TMP_Text turnCount;
    public Canvas unitCanvas;
    //___________________________________________________________________________________\\


    public static MenuManager Instance;
    [SerializeField] private GameObject[] _menu;

    [SerializeField]  GameObject eventTxt;
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
        eventTxt.SetActive(false);

        if(state == GameManager.GameState.ChooseOption)
        {
            //Debug.Log("Activate menu.");
            
            foreach(GameObject panel in _menu)
            {
                panel.SetActive(true);
                
            }
            UnitStats(); // Update current stats
        }
        else
        {
            //Debug.Log("Deactivate menu.");
            foreach (GameObject panel in _menu)
            {
                panel.SetActive(false);
            }
        }
        //throw new NotImplementedException();
    }

  //___________________________________________________________________________________\\


    
    public void UnitStats(){

    BasePlayer selectedHero = UnitManager.Instance.Player;

        if (selectedHero == null)
    {
        Debug.LogWarning("No hero selected to display stats.");
        return;
    }

    unitHEALTH.text = $"Health: {selectedHero._currentHealth}";
    unitATK.text = $"ATK: {selectedHero._attack}";
    unitDEF.text = $"DEF: {selectedHero._defense}";
    unitRAN.text = $"RAN: {selectedHero._movementRange}";
    unitSPD.text = $"SPD: {selectedHero._movementRange}";
    unitName.text = "The Queen";
    unitCanvas.enabled = true;
    }


    public void UpdateCount() {
    
    turnCount.text = "Current Turn:" + GameManager.Instance.TurnManager.GetCurrentCount();
    }

//___________________________________________________________________________________\\

    public void EventMessages (string debug) {

        eventTxt.SetActive(true);
        eventMsg.text = debug;
        StartCoroutine(HideMessage(1f));

    }

    private IEnumerator HideMessage(float time) {
        yield return new WaitForSeconds(time);
        eventTxt.SetActive(false);

    }
  

}
