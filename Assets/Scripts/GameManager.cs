﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private static GameObject container;
    public static GameManager GetInstance()
    {
        if (!instance)
        {
            container = new GameObject();
            container.name = "GameManager";
            instance = container.AddComponent(typeof(GameManager)) as GameManager;
        }
        return instance;
    }

    public Player player1;
    public Player player2;

    public bool isPlayer1;

    public Unit getUnit(int id)
    {
        var player1List = player1.unitList;
        var player2List = player2.unitList;
        for (int i = 0; i < player1List.Count; ++i)
        {
            if (player1List[i].unitID == id)
                return player1List[i];
        }
        for (int i = 0; i < player2List.Count; ++i)
        {
            if (player2List[i].unitID == id)
                return player2List[i];
        }
        Debug.Log("error unit id not exist");
        return null;
    }

    Unit getUnitByPos(int x, int y)
    {
        var player1List = player1.unitList;
        var player2List = player2.unitList;
        for (int i = 0; i < player1List.Count; ++i)
        {
            if (player1List[i].isInPos(x, y))
                return player1List[i];
        }
        for (int i = 0; i < player2List.Count; ++i)
        {
            if (player2List[i].isInPos(x, y))
                return player2List[i];
        }
        Debug.Log("error pos unit not exist");
        return null;
    }
}
