﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PLAYER { PLAYER1, PLAYER2, NONE};

public class GameManager : MonoBehaviour
{
    
    
    public GameObject Player1Units;
    public GameObject Player2Units;
    //public ObjectSight sightPool;

    private static GameManager instance;
    public static GameManager GetInstance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

    }
    public Player player1;
    public Player player2;

    public List<GameObject> buildingObj;
    public List<Building> field_buildings;

    public PLAYER myPlayer;
    public PLAYER enemyPlayer;
    public bool myTurn;

    public int new_id = 1;

    public int giveID()
    {
        int give_id = new_id;
        new_id++;
        return give_id;
    }

    public void setSeed(int seed)
    {
        UnityEngine.Random.InitState(seed);
        GameObject.Find("TitleControl").GetComponent<TitleControl>().testText.text = UnityEngine.Random.seed.ToString();
        Debug.Log("Seed Number : " + UnityEngine.Random.seed);
    }

    void Start()
    {
        buildingObj = new List<GameObject>();
        field_buildings = new List<Building>();
        StartCoroutine(IeStartGame());
        StartCoroutine(Buildsetting());
    }

    IEnumerator IeStartGame()
    {


        while (true)
        {//게임시간 줄이기 


            if (gameTime >= 6000f) //만약 게임에서 승리한다면 break 
            {
                break;
            }
            GameTime += Time.deltaTime;
            yield return null;
        }



    }

    IEnumerator Buildsetting()
    {
        GameObject obj;
        for (int i = 0; i < 4; i++)
        {
            obj = Resources.Load("Prefabs/underbuiled2") as GameObject;

            buildingObj.Add(obj);
        }

        for (int j = 0; j < 2; j++)
        {
            obj = Resources.Load("Prefabs/groundbuiled1") as GameObject;

            buildingObj.Add(obj);
        }

        for (int k = 0; k < 4; k++)
        {
            obj = Resources.Load("Prefabs/highbuiled0") as GameObject;

            buildingObj.Add(obj);
        }

        for (int b = 0; b < 10; b++)
        {
            obj = Instantiate(buildingObj[b]);

            field_buildings.Add(obj.GetComponent<Building>());

        }
        yield return null;
    }

    public Text timeText;//초시계

    public float gameTime = 0f;
    public float GameTime
    {
        get { return gameTime; }
        set
        {

            gameTime = Mathf.Clamp(value, 0f, float.MaxValue);
            string hours = ((int)gameTime / 3600).ToString(); ;

            string minute = ((int)gameTime % 3600 / 60).ToString();

            string second = (gameTime % 60).ToString("f2");

            timeText.text = hours + ":" + minute + ":" + second;
        }
    }

    public void subtractGold(PLAYER player, int gold)
    {
        switch (player)
        {
            case PLAYER.PLAYER1:
                player1.Gold -= gold;
                break;
            case PLAYER.PLAYER2:
                player2.Gold -= gold;
                break;
            case PLAYER.NONE:
                break;
            default:
                break;
        }
    }

    public PLAYER checkPlayerByID(int id)
    {
        return getUnit(id).control_player;
    }

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

    public void MyTurnEnd()
    {
        myTurn = false;
        startTurn(enemyPlayer);
        var m_network = GameObject.FindWithTag("Network").GetComponent<Network>();
        TurnEndData data = new TurnEndData();
        data.state = 1;
        TurnEndPacket Packet = new TurnEndPacket(data);
        m_network.SendReliable(Packet);
    }

    public void startTurn(PLAYER player)
    {
        playerSight(player);
    }
    
    List<Unit> enemyPlayerList;
    List<Unit> playerList;
    public void playerSight(PLAYER player)
    {      
        switch (player)
        {
            case PLAYER.PLAYER1:
                playerList = player1.unitList;
                enemyPlayerList = player2.unitList;
                break;
            case PLAYER.PLAYER2:
                playerList = player2.unitList;
                enemyPlayerList = player1.unitList;
                break;
            case PLAYER.NONE:
                break;
            default:
                break;           
        }
        for (int i = 0; i < playerList.Count; ++i)
        {
            enemyPlayerList[i].Show();
        }
        for (int i = 0; i < enemyPlayerList.Count; ++i)
        {
            enemyPlayerList[i].Hide();
        }
    }
    
}
