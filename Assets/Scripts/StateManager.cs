using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager instance;
    public State State;
    public UnitManager uM;
    public float timer = 1;
    public bool wait;

    void Awake() {
        instance = this;
        wait = false;
    }

    void Update() {
        if (wait && UnitManager.instance.AICount != 0) {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                UnitManager.instance.AITurn();
                wait = false;
            }
        }
        if (!wait) {
            timer = 1;
        }
    }

    void Start()
    {
        changeState(State.startMenu);
    }

    public void changeState(State nextState) {
        State = nextState;

        switch (nextState) {
            case State.startMenu:
                MenuManager.instance.startScreenControl();
                break;
            case State.instructionMenu:
                MenuManager.instance.instructionScreenControl();
                break;
            case State.makeGrid:
                Grid.instance.makeGrid();
                break;
            case State.spawnPlayer:
                UnitManager.instance.spawnPlayer();
                break;
            case State.spawnAI:
                UnitManager.instance.spawnAI();
                break;
            case State.playerTurn:
                UnitManager.instance.movesMade = 0;
                for (int i = 0; i < UnitManager.instance.playerList.Count; i++) {
                    UnitManager.instance.playerList[i].hasMoved = false;
                }
                break;
            case State.AITurn:
                wait = true;
                break;
            case State.winMenu:
                MenuManager.instance.winScreenControl();
                break;
            case State.loseMenu:
                MenuManager.instance.loseScreenControl();
                break;
            case State.creditMenu:
                MenuManager.instance.creditScreenControl();
                break;
            
        }
    }

    public State getCurrentState() {
        return State;
    }
    
}

public enum State
{
    startMenu = 0,
    instructionMenu = 1,
    makeGrid = 2,
    spawnPlayer = 3,
    spawnAI = 4,
    playerTurn = 5,
    AITurn = 6,
    winMenu = 7,
    loseMenu = 8,
    creditMenu = 9
}