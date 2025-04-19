using UnityEngine;

public class GroundTile : Tile
{

    public override void Init(int x, int y)
    {
        var isOffset = (x + y) % 2 == 1;

        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
