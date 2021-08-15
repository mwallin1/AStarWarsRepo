using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public void toCredit() {
        StateManager.instance.changeState(State.creditMenu);
    }
}
