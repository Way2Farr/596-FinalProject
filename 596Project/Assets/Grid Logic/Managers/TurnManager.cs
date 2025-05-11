using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class TurnManager 
{
  public int currentCount;


  public TMP_Text turnCount;
  public TurnManager() {
    currentCount = 1;
  }

  public void Tick(){
    currentCount += 1;
    Debug.Log("Current Tick:" + currentCount);    
  }

    public int GetCurrentCount() {
        return currentCount;
    }

}