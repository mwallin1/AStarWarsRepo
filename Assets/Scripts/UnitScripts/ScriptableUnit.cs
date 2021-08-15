using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Scriptable Unit")]

public class ScriptableUnit : ScriptableObject
{
    public Team Team;
    public UnitBase unitPrefab;
}

public enum Team { 
    Player = 0,
    AI = 1
}