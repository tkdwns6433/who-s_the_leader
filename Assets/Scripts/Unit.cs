using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int unitID;
    public int x;
    public int y;

    public bool isInPos(int _x, int _y)
    {
        return _x == x && _y == y;
    }
}
