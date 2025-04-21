using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    protected Color _baseColor, _offsetColor;
    [SerializeField]
    protected SpriteRenderer _renderer;
    [SerializeField]
    protected GameObject _highlight;

    [SerializeField] private bool _isWalkable = true;

    public BaseUnit OccupiedUnit;
    public bool Walkable => _isWalkable && OccupiedUnit != null;

    public virtual void Init(int x, int y)
    {
        //_renderer.color = isOffset ? _baseColor : _offsetColor;
    }

    private void OnMouseEnter()
    {
        _highlight.SetActive(true);
        //Debug.Log("Hovering square.");
    }

    private void OnMouseExit()
    {
        _highlight.SetActive(false);
        //Debug.Log("Not hovering square.");
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.State != GameManager.GameState.PlayerMove) return;

        if (OccupiedUnit != null)
        {
            if (OccupiedUnit.Faction == Faction.Hero)
            {
                UnitManager.Instance.SetSelectedHero((BasePlayer)OccupiedUnit);
            }
            else
            {
                if(UnitManager.Instance.SelectedHero != null)
                {
                    var enemy = (BaseEnemy)OccupiedUnit;

                    Destroy(enemy.gameObject);
                    UnitManager.Instance.SetSelectedHero(null);
                }
            }
        }
        else
        {
            if(UnitManager.Instance.SelectedHero != null)
            {
                SetUnit(UnitManager.Instance.SelectedHero);
                UnitManager.Instance.SetSelectedHero(null);
            }
        }
    }

    public void SetUnit(BaseUnit unit)
    {
        if (unit.OccupiedTile != null)
        {
            unit.OccupiedTile.OccupiedUnit = null;
        }
        unit.transform.position = transform.position;
        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }
}


