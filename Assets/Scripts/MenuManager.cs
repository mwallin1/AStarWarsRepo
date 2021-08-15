using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [SerializeField] private GameObject selectedPlayerObject, tileObject, tileUnitObject; 
    public GameObject startScreen, instructionScreen, creditScreen, winScreen, loseScreen;

    
    void Awake() {
        instance = this;
    }

    void Update() {
        if (StateManager.instance.State != State.playerTurn)
        {
            selectedPlayerObject.SetActive(false);
            tileObject.SetActive(false);
            tileUnitObject.SetActive(false);
        }
    }

    public void displayTileInfo(Tile tile) {
       

        if (tile == null)
        {
            tileObject.SetActive(false);
            tileUnitObject.SetActive(false);
            return;
        }

        tileObject.GetComponentInChildren<Text>().text = tile.tileName;
        tileObject.SetActive(true);

        if (tile.Tenent)
        {
            tileUnitObject.GetComponentInChildren<Text>().text = tile.Tenent.unitName;
            tileUnitObject.SetActive(true);
        }
    }

    public void showSelectedPlayer(PlayerBase player) {
        if (player == null) {
            selectedPlayerObject.SetActive(false);
            return;
        }

        selectedPlayerObject.GetComponentInChildren<Text>().text = player.unitName;
        selectedPlayerObject.SetActive(true);
    }

    public void startScreenControl() {
        if (StateManager.instance.State != State.startMenu) {
            startScreen.SetActive(false);
        }
        instructionScreen.SetActive(false);
        creditScreen.SetActive(false);
        startScreen.SetActive(true);
        loseScreen.SetActive(false);
        winScreen.SetActive(false);
    }

    public void instructionScreenControl()
    {
        if (StateManager.instance.State != State.instructionMenu)
        {
            instructionScreen.SetActive(false);
        }
        instructionScreen.SetActive(true);
        creditScreen.SetActive(false);
        startScreen.SetActive(false);
        loseScreen.SetActive(false);
        winScreen.SetActive(false);
    }

    public void creditScreenControl()
    {
        if (StateManager.instance.State != State.creditMenu)
        {
            creditScreen.SetActive(false);
        }
        startScreen.SetActive(false);
        instructionScreen.SetActive(false);
        creditScreen.SetActive(true);
        loseScreen.SetActive(false);
        winScreen.SetActive(false);
    }

    public void winScreenControl()
    {
        if (StateManager.instance.State != State.winMenu)
        {
            winScreen.SetActive(false);
        }
        startScreen.SetActive(false);
        instructionScreen.SetActive(false);
        creditScreen.SetActive(false);
        loseScreen.SetActive(false);
        winScreen.SetActive(true);
    }

    public void loseScreenControl()
    {
        if (StateManager.instance.State != State.loseMenu)
        {
            loseScreen.SetActive(false);
        }
        startScreen.SetActive(false);
        instructionScreen.SetActive(false);
        creditScreen.SetActive(false);
        loseScreen.SetActive(true);
        winScreen.SetActive(false);
    }
}
