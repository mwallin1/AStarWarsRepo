using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    //[SerializeField] private Color evenColor, oddColor;
    [SerializeField] protected SpriteRenderer renderer;
    [SerializeField] public GameObject highlight;
    [SerializeField] public bool isWater;
    [SerializeField] private bool isSand;

    
    public string tileName;
    public UnitBase Tenent;
    public bool traversable => !isWater && Tenent == null;
    
    //A* Stuff
    public int gCost;
    public int hCost;
    public int fCost;

    public int x;
    public int y;

    public Tile prevTile;
    
    

    void Update() {
        
        
    }
    public virtual void Init(int x, int y) {
        
    }

    void OnMouseEnter() {
        highlight.SetActive(true);
        MenuManager.instance.displayTileInfo(this);
        //Debug.Log( this.name );
    }

    void OnMouseExit()
    {
        highlight.SetActive(false);
        MenuManager.instance.displayTileInfo(null);
    }

    void OnMouseDown() {
        if (StateManager.instance.State != State.playerTurn) {
            return;
        }
        if (Tenent != null)
        {
            if (Tenent.team == Team.Player && !Tenent.hasMoved) {
                UnitManager.instance.setSelectedPlayer((PlayerBase)Tenent);
                UnitManager.instance.selectedPlayer.hasMoved = true;
                
            } 
            else
            {
                if (UnitManager.instance.selectedPlayer != null && inRangeOfTile(UnitManager.instance.selectedPlayer))
                {
                    var enemy = (EnemyBase)Tenent;
                    // add damage stuff
                    Destroy(enemy.gameObject);
                    UnitManager.instance.AIList.Remove(enemy);
                    enemy.currentTile.Tenent = null;
                    UnitManager.instance.AICount -= 1;
                    UnitManager.instance.movesMade += 1;
                    if (UnitManager.instance.AICount == 0)
                    {
                        FindObjectOfType<AudioManager>().Play("Win");
                        StateManager.instance.changeState(State.winMenu);
                    }
                    FindObjectOfType<AudioManager>().Play("Boom");
                    setUnit(UnitManager.instance.selectedPlayer);
                    UnitManager.instance.setSelectedPlayer(null);
                    
                    
                }
            }
        }
        else {
            if (UnitManager.instance.selectedPlayer != null && traversable && inRangeOfTile(UnitManager.instance.selectedPlayer)) {
                
                
                setUnit(UnitManager.instance.selectedPlayer); 
              
                UnitManager.instance.setSelectedPlayer(null);

                UnitManager.instance.movesMade += 1;
                print(UnitManager.instance.movesMade);
                FindObjectOfType<AudioManager>().Play("PlayerMove");
                
               
            }
        }
        
    }
    public void setUnit(UnitBase unit) {
        if (unit.currentTile != null) unit.currentTile.Tenent = null;

        { 
            unit.transform.position = transform.position;
            Tenent = unit;
            unit.currentTile = this;
        }
        
        
        
    }

    public bool inRangeOfTile(UnitBase unit) { 
        int distX = Mathf.Abs(unit.currentTile.x - this.x);
        int distY = Mathf.Abs(unit.currentTile.y - this.y);

        if (distX <= 2 && distY <= 2)
        {
            return true;
        }
        else {
            return false;
        }
    }

    public void setAIPos(EnemyBase a, Tile t) {
        a.currentTile.Tenent = null;
        a.transform.position = t.transform.position;
        Tenent = a;
        a.currentTile = t;
    }
   
    public int getX() {
        return this.x;
    }

    public int getY()
    {
        return this.y;
    }

    public int getTileHCost()
    {
        return this.hCost;
    }

    public int getTileGCost()
    {
        return this.gCost;
    }

    public int getTileFCost()
    {
        return this.hCost + this.gCost;
    }

    
}
