using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PLAYER { PLAYER1, PLAYER2, NONE};

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

    public List<Building> field_buildings;

    public bool isPlayer1;
    public bool myTurn;

    public Building getBuilding(int id)
    {
        for (int i = 0; i < field_buildings.Count; i++)
        {
            if (field_buildings[i].building_id == id)
                return field_buildings[i];
        }
        Debug.Log("Error : building id is not exist");
        return null;
    }

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

    public void deleteUnit(int id)
    {
        var player1List = player1.unitList;
        var player2List = player2.unitList;
        for (int i = 0; i < player1List.Count; ++i)
        {
            if (player1List[i].unitID == id)
            {
                player1List.RemoveAt(i);
                return;
            }
        }
        for (int i = 0; i < player2List.Count; ++i)
        {
            if (player2List[i].unitID == id)
            {
                player2List.RemoveAt(i);
                return;
            }
        }
        Debug.Log("error : no delete unit id exist");
        return;
    }
}
