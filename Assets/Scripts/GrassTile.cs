using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GrassTile : Tile
{
    [SerializeField] private Color evenColor, oddColor;


    public override void Init(int x, int y) {
        var isOdd = (x + y) % 2 == 1;
        renderer.color = isOdd ? oddColor : evenColor;
    }
}
