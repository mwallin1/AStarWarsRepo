using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Grid : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private Tile Grass, Water, Sand;
    [SerializeField] private Transform camera;

    public static Grid instance;
    private Dictionary<Vector2, Tile> tiles;
    private List<Tile> openList;
    private List<Tile> closedList;
    private const int STRAIGHT_MOVE = 10;
    private const int DIAG_MOVE = 14;
    

    void Awake() {
        instance = this;
    }

    

    public void makeGrid() {
        tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                var rand = Random.Range(0, 10);
                var randomTile = Grass;
                if (rand == 2)
                {
                    randomTile = Water;
                }
                else if (rand == 3 || rand == 4)
                {
                    randomTile = Sand;
                }
                else
                {
                    randomTile = Grass;
                }
            
                var createdTile = Instantiate(randomTile, new Vector3(x, y), Quaternion.identity);
                createdTile.name = $"Tile {x} {y}";
                createdTile.x = x;
                createdTile.y = y;
                createdTile.Init(x, y);
               
                tiles[new Vector2(x, y)] = createdTile;
            }
        }

        camera.transform.position = new Vector3((float)width/2 -0.5f, (float)height / 2 - 0.5f, -10);

        StateManager.instance.changeState(State.spawnPlayer);
    }

    public Tile getPlayerSpawnTile() {
        return tiles.Where(t => t.Key.x < width / 2 && t.Value.traversable).OrderBy(t => Random.value).First().Value;
    }

    public Tile getAISpawnTile()
    {
        return tiles.Where(t => t.Key.x > width / 2 && t.Value.traversable).OrderBy(t => Random.value).First().Value;
    }

    public Tile findPath(EnemyBase AI, PlayerBase player)
    {
        
        Tile onTile;
        Vector2 startPos = new Vector2(AI.currentTile.getX(), AI.currentTile.getY());
        Tile start = tiles.Where(t => t.Key == startPos).First().Value;
        

        Vector2 targetPos = new Vector2(player.currentTile.getX(), player.currentTile.getY());
        Tile target = tiles.Where(t => t.Key == targetPos).First().Value;
        

        openList = new List<Tile> { start };
        closedList = new List<Tile>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 cTilePos = new Vector2(x, y);
                Tile cTile = tiles.Where(t => t.Key == cTilePos).First().Value;
                cTile.gCost = int.MaxValue;
                cTile.getTileFCost();
                cTile.prevTile = null;
            }
        }

        start.gCost = 0;
        start.hCost = calcMoveCost(start, target);
        start.getTileFCost();
        

        while (openList.Count > 0)
        {
            onTile = getLowestFCostTile(openList);
            if (onTile == target)
            {
                return createPath(AI, target);
            }
        

            openList.Remove(onTile);
            closedList.Add(onTile);

            foreach (Tile neighborTile in getNieghbors(onTile))
            {
                if (closedList.Contains(neighborTile)) continue;

                int potentialGCost = onTile.gCost + calcMoveCost(onTile, neighborTile);
                
                if (potentialGCost < neighborTile.gCost && !neighborTile.isWater)
                {
                    
                    neighborTile.prevTile = onTile;
                    neighborTile.gCost = potentialGCost;
                    neighborTile.hCost = calcMoveCost(neighborTile, target);
                    neighborTile.getTileFCost();

                    if (!openList.Contains(neighborTile))
                    {
                        openList.Add(neighborTile);
                    }
                }
            }
        }
        return null;
    }

    public List<Tile> getNieghbors(Tile presentTile) {
        
        List<Tile> neighborsList = new List<Tile>();
        //left
        if (presentTile.x - 1 >= 0) {
            Vector2 leftTilePos = new Vector2(presentTile.getX() - 1, presentTile.getY());
            Tile leftTile = tiles.Where(t => t.Key == leftTilePos).First().Value;
            neighborsList.Add(leftTile);
            //left down diagonal
            if (presentTile.y - 1 >= 0) {
                Vector2 leftDownTilePos = new Vector2(presentTile.getX() - 1, presentTile.getY() - 1);
                Tile leftDownTile = tiles.Where(t => t.Key == leftDownTilePos).First().Value;
                neighborsList.Add(leftDownTile);
            }
            //left up diagonal
            if (presentTile.y + 1 < height)
            {
                Vector2 leftUpTilePos = new Vector2(presentTile.getX() - 1, presentTile.getY() + 1);
                Tile leftUpTile = tiles.Where(t => t.Key == leftUpTilePos).First().Value;
                neighborsList.Add(leftUpTile);
            }
        }
        //right
        if (presentTile.x + 1 < width)
        {
            Vector2 rightTilePos = new Vector2(presentTile.getX() + 1, presentTile.getY());
            Tile rightTile = tiles.Where(t => t.Key == rightTilePos).First().Value;
            neighborsList.Add(rightTile);
            //right down diagonal
            if (presentTile.y - 1 >= 0)
            {
                Vector2 rightDownTilePos = new Vector2(presentTile.getX() + 1, presentTile.getY() - 1);
                Tile rightDownTile = tiles.Where(t => t.Key == rightDownTilePos).First().Value;
                neighborsList.Add(rightDownTile);
            }
            //right up diagonal
            if (presentTile.y + 1 < height)
            {
                Vector2 rightUpTilePos = new Vector2(presentTile.getX() + 1, presentTile.getY() + 1);
                Tile rightUpTile = tiles.Where(t => t.Key == rightUpTilePos).First().Value;
                neighborsList.Add(rightUpTile);
            }
        }
        // up
        if (presentTile.y + 1 < height) {
            Vector2 upTilePos = new Vector2(presentTile.getX(), presentTile.getY() + 1);
            Tile upTile = tiles.Where(t => t.Key == upTilePos).First().Value;
            neighborsList.Add(upTile);
        }
        // down
        if (presentTile.y - 1 >= 0)
        {
            Vector2 downTilePos = new Vector2(presentTile.getX(), presentTile.getY() - 1);
            Tile downTile = tiles.Where(t => t.Key == downTilePos).First().Value;
            neighborsList.Add(downTile);
        }

        return neighborsList;
    }
    //dont need it to be a path just tile it can reach
    public Tile createPath(EnemyBase AI, Tile t) {
        
        List<Tile> path = new List<Tile>();
        
        path.Add(t);
        Tile currentTile = t;
        while (currentTile.prevTile != null) {
            path.Add(currentTile.prevTile);
            currentTile = currentTile.prevTile;
        }
        path.Reverse();
        
        if (path.Count > AI.availableMoves) {
            //FOUND BUG WHERE IF PLAYER MOVES ON A TILE PREVIOSLY BEEN ON BY AI IT KILLS AI
            //path[AI.availableMoves].highlight.SetActive(true);
            //print(path[AI.availableMoves]);
            return path[AI.availableMoves];
        }

        //print(path[path.Count -1]);
        return path[path.Count - 1];
        
        
    }

    public int calcMoveCost(Tile a, Tile b) {
        int xDist = Mathf.Abs(a.x - b.x);
        int yDist = Mathf.Abs(a.y - b.y);
        int rem = Mathf.Abs(xDist - yDist);
        return DIAG_MOVE * Mathf.Min(xDist, yDist) + STRAIGHT_MOVE * rem;
    }

    public Tile getLowestFCostTile(List<Tile> tileList) {
       Tile minF = tileList[0];
        for (int i = 1; i < tileList.Count; i++) {
            if (tileList[i].fCost < minF.fCost) {
                minF = tileList[i];
            }
        }
        return minF;
    }

}
