using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : BaseUnit
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override List<Tile> getMovementTiles()
    {
        return base.getMovementTiles();
    }
}
