using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;
    private List<ScriptableUnit> units;
    public List<PlayerBase> playerList;
    public List<EnemyBase> AIList;
    public PlayerBase selectedPlayer, targetPlayer;
    
    private Tile bestTile;
    public int AICount, playerCount, movesMade;
    

    void Awake() {
        instance = this;
        
        units = Resources.LoadAll<ScriptableUnit>("Units").ToList();

    }

    void Update() {
        if (movesMade >= playerList.Count && StateManager.instance.State == State.playerTurn)
        {
            StateManager.instance.changeState(State.AITurn);
            return;
        }
        
    }

    
    public void spawnPlayer() {
        playerCount = 4;
        playerList = new List<PlayerBase>();
        for (int i = 0; i < playerCount; i++) {
            var randomPrefab = getRandomUnit<PlayerBase>(Team.Player);
            var spawnedPlayer = Instantiate(randomPrefab);
            var randomSpawnTile = Grid.instance.getPlayerSpawnTile();
            playerList.Add(spawnedPlayer);
            randomSpawnTile.setUnit(spawnedPlayer);
        }
        //print(playerList);
        StateManager.instance.changeState(State.spawnAI);
    }

    public void spawnAI()
    {
        AIList = new List<EnemyBase>();
        AICount = 4;
        
        for (int i = 0; i < AICount; i++)
        {
            var randomPrefab = getRandomUnit<EnemyBase>(Team.AI);
            var spawnedEnemy = Instantiate(randomPrefab);
            var randomSpawnTile = Grid.instance.getAISpawnTile();
            AIList.Add(spawnedEnemy);
            randomSpawnTile.setUnit(spawnedEnemy);
        }
        //print(AIList);
        StateManager.instance.changeState(State.playerTurn);
    }

    public T getRandomUnit<T>(Team team) where T : UnitBase {
        return (T) units.Where(u => u.Team == team).OrderBy(o => Random.value).First().unitPrefab;
    }

    public void setSelectedPlayer(PlayerBase player) {
        selectedPlayer = player;
        MenuManager.instance.showSelectedPlayer(player);
    }

    public void AITurn() {
        
        for (int i = 0; i < AIList.Count; i++) {

            PlayerBase bestTarget;
            int bestIndex = 0;
            int worstFCost = int.MaxValue;

            for (int j = 0; j < playerList.Count; j++) { 
                Tile temp = Grid.instance.findPath(AIList[i], playerList[j]);
                if (temp.fCost < worstFCost) {
                    bestTile = temp;
                    bestTarget = playerList[j];
                    bestIndex = i;
                }
            }
            if (!bestTile.isWater)
            {

                if (playerList.Contains(bestTile.Tenent))
                {
                    var playerTarget = (PlayerBase)bestTile.Tenent;
                    // add damage stuff
                    Destroy(playerTarget.gameObject);
                    playerList.Remove(playerTarget);
                    FindObjectOfType<AudioManager>().Play("Boom");
                    playerCount -= 1;
                    if (playerCount == 0)
                    {
                        FindObjectOfType<AudioManager>().Play("Lose");
                        StateManager.instance.changeState(State.loseMenu);
                    }
                    bestTile.setAIPos(AIList[bestIndex], bestTile);
                    movesMade = 0;
                    StateManager.instance.changeState(State.playerTurn);



                }
                if (bestTile.Tenent == null)
                {

                    bestTile.setAIPos(AIList[bestIndex], bestTile);
                    FindObjectOfType<AudioManager>().Play("AIMove");
                    movesMade = 0;
                    StateManager.instance.changeState(State.playerTurn);

                }
            }

        }
        

        StateManager.instance.changeState(State.playerTurn);
        


    }
    
}


