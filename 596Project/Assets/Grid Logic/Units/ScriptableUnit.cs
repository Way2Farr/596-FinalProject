using JetBrains.Annotations;
using UnityEngine;


[CreateAssetMenu(fileName = "New Unit", menuName = "Scriptable Unit")]
public class ScriptableUnit : ScriptableObject
{
    public Faction Faction;
    public BaseUnit UnitPrefab;

}

public enum Faction
{
    Hero = 0,
    Enemy = 1
}