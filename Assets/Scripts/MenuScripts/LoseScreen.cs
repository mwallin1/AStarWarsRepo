using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseScreen : MonoBehaviour
{
    public void toCredit()
    {
        StateManager.instance.changeState(State.creditMenu);
    }
}
