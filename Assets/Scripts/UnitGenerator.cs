using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGenerator : MonoBehaviour
{
    UnitType genUnit;
    int posX;
    int posY;
    bool isPlayer1;
    bool isEcho;
    public UnitGenerator(UnitType ut, int x, int y, bool is_player1)
    {
        genUnit = ut;
    }

    public void GenerateUnit()
    {
    }
}
