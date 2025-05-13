using JetBrains.Annotations;
using UnityEngine;


[CreateAssetMenu(fileName = "New Unit", menuName = "Scriptable Unit")]
public class ScriptableUnit : ScriptableObject
{
    public Faction Faction;
    public BaseUnit UnitPrefab;
    public RoundNumber RoundNumber;

}

public enum Faction
{
    Hero = 0,
    Enemy = 1
}

public enum RoundNumber
{
    Round1 = 1,
    Round2 = 2,
    Round3 = 3
}