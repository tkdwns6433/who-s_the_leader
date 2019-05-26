using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PLAYER { PLAYER1, PLAYER2, NONE};

public class GameManager : MonoBehaviour
{
    
    
    public GameObject Player1Units;
    public GameObject Player2Units;
    public GameObject Player1Leader;
    public GameObject Player2Leader;
    public GameObject Agit1;
    public GameObject Agit2;
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
        while (gameTime > 0f)
        {//게임시간 줄이기

            GameTime -= Time.deltaTime;
            yield return null;
        }
    }

    int l, m, t; // 각각 갯수 제한을 위한 변수
    public bool currentUnit; //현재 유닛 선택여부확인 변수
    IEnumerator Buildsetting()
    {
        bool setstart = true;
        int random;
        GameObject obj;
        while (setstart)
        {
            random = Random.Range(0, 3);
            
            if (random == 0)
            {
                if (l != 4)
                {
                    obj = Resources.Load("Prefabs/buildings/underbuiled2") as GameObject;
                    l++;
                    buildingObj.Add(obj);
                }
            }

            if (random == 1)
            {
                if (m != 2)
                {
                    obj = Resources.Load("Prefabs/buildings/groundbuiled1") as GameObject;
                    m++;
                    buildingObj.Add(obj);
                }
            }

            if (random == 2)
            {
                if (t != 4)
                {
                    obj = Resources.Load("Prefabs/buildings/highbuiled0") as GameObject;
                    t++;
                    buildingObj.Add(obj);
                }
            }

            if(l==4&& m==2 && t == 4)
            {
                setstart = false;
            }
        }
        for (int b = 0; b < 10; b++)
        {
            obj = Instantiate(buildingObj[b]);

            field_buildings.Add(obj.GetComponent<Building>());

        }
        yield return new WaitForSeconds(0.5f);

        Agit1 = Instantiate(Resources.Load("Prefabs/buildings/Agit green")) as GameObject;
        Agit2 = Instantiate(Resources.Load("Prefabs/buildings/Agit red")) as GameObject;



        GameObject genTiledPos = GameObject.Find("Player1gen");

        Player1Leader.GetComponent<Unit>().initiateUnit(UnitType.Mafiaunit, genTiledPos.transform.position.x, genTiledPos.transform.position.y + 91, PLAYER.PLAYER1);

        genTiledPos = GameObject.Find("Player2gen");

        Player2Leader.GetComponent<Unit>().initiateUnit(UnitType.Mafiaunit, genTiledPos.transform.position.x, genTiledPos.transform.position.y + 91, PLAYER.PLAYER2);

    }

    public Text timeText;//초시계

    public float gameTime = 30f; //게임 시간
    public float GameTime
    {
        get { return gameTime; }
        set
        {
            gameTime = Mathf.Clamp(value, 0f, float.MaxValue);
            timeText.text = gameTime.ToString("00");
        }
    }

    public void subtractGold(PLAYER player, int gold)
    {
        switch (player)
        {
            case PLAYER.PLAYER1:
               // player1.Gold -= gold;
                break;
            case PLAYER.PLAYER2:
                //player2.Gold -= gold;
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
