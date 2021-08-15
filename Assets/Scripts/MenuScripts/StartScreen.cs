using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public void playGame() {
        MenuManager.instance.startScreen.SetActive(false);
        StateManager.instance.changeState(State.makeGrid);
    }

    public void toInstructions() {
        StateManager.instance.changeState(State.instructionMenu);
    }
}
