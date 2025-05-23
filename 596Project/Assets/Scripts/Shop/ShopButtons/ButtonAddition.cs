using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class ButtonAddition : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button hpButton;
    [SerializeField] private Button defButton;
    [SerializeField] private Button atkButton;
    [SerializeField] private Button mpButton;
    [SerializeField] private Button aRangeButton;
    [SerializeField] private Button mMoveButton;

    [Header("Text")]
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text defText;
    [SerializeField] private TMP_Text atkText;
    [SerializeField] private TMP_Text mpText;
    [SerializeField] private TMP_Text aRangeText;
    [SerializeField] private TMP_Text mMoveText;
    [SerializeField] private TMP_Text skillPointText;

    [Header("Stages")]
    [SerializeField] private Image stageOne;
    [SerializeField] private Image stageTwo;
    [SerializeField] private Image stageThree;

    [Header("Slider")]
    [SerializeField] private Slider _sliderTwo;
    [SerializeField] private Slider _slider;
    [SerializeField] private string scene;
    [SerializeField] private Image blackscreen;
    [SerializeField] private Animator anim;
    private bool transitioning = false;

    //variables
    public GameObject gameStats;
    public StatManager statManager=null;
    public int currentHp = 0; //10 set to default for testing
    public int currentDef = 10;
    public int currentAtk = 10;
    public int currentMp = 3;
    public int currentARange = 10;
    public int currentMRange = 10;
    public int skillPoints = 6;

    void Awake()
    {
        //get values from DontDestroyOnLoadObject
        gameStats = GameObject.FindGameObjectWithTag("StatManager");
        statManager = gameStats.GetComponent<StatManager>();
        if(statManager._currentRound != 1)
        {
            _slider = _sliderTwo;
        }
       
    }
    void Start()
    {
        if(statManager._currentRound != 1)
        {
            stageTwo.color = Color.green;
        }
        //hpText.text = "HP: 10";
        currentHp = statManager._maxHealth;
        currentDef = statManager._defense;
        currentAtk = statManager._attack;
        currentARange = statManager._attackRange;
        currentMRange = statManager._movementRange;
        /*if (currentHp == statManager._maxHealth)
        {
            Debug.Log("hp set to statManager's _maxHealth field");
        }*/
        skillPointText.text = "Skill Points: " + skillPoints.ToString();
        hpText.text = "HP: " + currentHp.ToString();
        defText.text = "DF: "+ currentDef.ToString();
        atkText.text = "Atk: "+ currentAtk.ToString();
        aRangeText.text = "ARnge: " + currentARange.ToString();
        mMoveText.text = "MRange: " + currentMRange.ToString();
    }
    void Update()
    {
        if ((_slider.value == 1f) && !transitioning)//change to Blackscreen fade
        {
            Debug.Log("SceneLoad");
            transitioning = true;
            StartCoroutine(Fade());
        }
    }

    IEnumerator Fade()
    {
        anim.SetBool("fade", true);


        int currentRound = 1;

        if (StatManager.Instance != null)
        {
            currentRound = statManager._currentRound;
        }
            
        yield return new WaitUntil(() => blackscreen.color.a == 1);

        if (currentRound == 2)
        {
            SceneManager.LoadScene("Round 2");
        }
            
        else if (currentRound == 3)
        {
            SceneManager.LoadScene("Round 3");
        }
        else
        {
            SceneManager.LoadScene("Title Screen");
        }
    }

    public void HPAdd()
    {
        if (skillPoints > 0)
        {
            skillPoints = skillPoints - 1;
            skillPointText.text = "Skill Points: " + skillPoints.ToString();
            currentHp = currentHp + 10;
            hpText.text = "HP: " + currentHp.ToString();
        }
    }
    public void DFAdd()
    {
        if (skillPoints > 0)
        {
            skillPoints = skillPoints - 1;
            skillPointText.text = "Skill Points: " + skillPoints.ToString();
            currentDef = currentDef + 10;
            defText.text = "DF: " + currentDef.ToString();
        }
    }
    public void AtkAdd()
    {
        if (skillPoints > 0)
        {
            skillPoints = skillPoints - 1;
            skillPointText.text = "Skill Points: " + skillPoints.ToString();
            currentAtk = currentAtk + 10;
            atkText.text = "Atk: " + currentAtk.ToString();
        }
    }
    public void mpAdd()
    {
        if (skillPoints > 0)
        {
            skillPoints = skillPoints - 1;
            skillPointText.text = "Skill Points: " + skillPoints.ToString();
            currentMp += 10;
            mpText.text = "MP: " + currentMp.ToString();
            
        }
    }
    public void ARangeAdd()
    {
        if (skillPoints > 0)
        {
            skillPoints = skillPoints - 1;
            skillPointText.text = "Skill Points: " + skillPoints.ToString();
            currentARange = currentARange + 10;
            aRangeText.text = "ARnge: " + currentARange.ToString();
        }
    }
    public void MvmtRangeAdd()
    {
        if (skillPoints > 0)
        {
            skillPoints = skillPoints - 1;
            skillPointText.text = "Skill Points: " + skillPoints.ToString();
            currentMRange = currentMRange + 10;
            mMoveText.text = "MRange: " + currentMRange.ToString();
        }
    }
    public void NextLevel()
    {
        statManager.IncreaseStats(currentHp, currentAtk, currentDef, currentMRange, currentARange, (currentMp/10));
    }
    
} 
