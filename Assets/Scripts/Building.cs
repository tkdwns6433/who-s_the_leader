using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int building_id;
    public PLAYER player_occupy;

    public int x;
    public int y;
    public int BuildNum;                      //2 => under, 1 => ground, 0 => high
    GameObject Tiled;
    SpriteRenderer spt;

    public float x;
    public float y;
    float second, second2;
    bool playerCheck, Checkcheck;

    public BuildingData buildingData;

    private void Start()
    {
        initiateBuilding(0, 0, GameData.BuildingDatas[BuildNum]);
        Tiled = GameObject.Find("Buildtiled" + building_id.ToString());
        Debug.Log("Buildtiled" + building_id.ToString());
        setBuilding(Tiled.transform.position.x, Tiled.transform.position.y + 200);   //타일에 위치에 시작할때 배정된다.

        spt = GetComponent<SpriteRenderer>();

        second = 0.0f;
        second2 = 0.0f;
    }

    private void OnMouseDown()
    {

        SetSelectCheck();

        //if(GameManager.GetInstance().myTurn == true)  //플레이어 턴때 ui등장이랑 유닛뽑는거 만들 예정
        //{
        //    playerCheck = true;
        //    //ui 가져와야됨
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player1")
        {
            second += Time.deltaTime;
        }
        else if (collision.gameObject.tag == "Player2")
        {
            second2 += Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player1")
        {
            if (player_occupy == PLAYER.NONE || collision.gameObject.tag == "Player2")
            {
                second2 = 0;
                second += Time.deltaTime;
                if (second >= 2.0f)
                {
                    player_occupy = PLAYER.PLAYER1;
                }
            }

            if (player_occupy == PLAYER.PLAYER1)
            {
                //playerCheck = collision.GetComponent<Tempmove>().clickCheck; -> 플레이어 캐릭터 선택했는지 체크 아직 필요성x
            }
        }
        else if (collision.gameObject.tag == "Player2")
        {
            if (player_occupy == PLAYER.NONE || collision.gameObject.tag == "Player1")
            {
                second = 0;
                second2 += Time.deltaTime;
                if (second2 >= 2.0f)
                {
                    player_occupy = PLAYER.PLAYER2;
                }
            }

            if (player_occupy == PLAYER.PLAYER2)
            {
                playerCheck = collision.GetComponent<Tempmove>().clickCheck;
            }

        }
    }

    void setBuilding(int _x, int _y)
    {
        //클라이언트 측 실제로 빌딩 위치 시키는 코드
        x = _x;
        y = _y;

        this.transform.position = new Vector2(x, y);

    }

    void SetSelectCheck()
    {
        if (Checkcheck)
        {
            if (building_id <= 4)
            {
                spt.sprite = Resources.Load<Sprite>("image/Underbuild lineX") as Sprite;
            }
            else if (building_id > 4 && building_id <= 6)
            {
                spt.sprite = Resources.Load<Sprite>("image/Groundbuild lineX") as Sprite;
            }
            else if (building_id > 6 && building_id <= 10)
            {
                spt.sprite = Resources.Load<Sprite>("image/Highbuild lineX") as Sprite;
            }
            Checkcheck = false;
        }
        else if (!Checkcheck)
        {
            if (building_id <= 4)
            {
                spt.sprite = Resources.Load<Sprite>("image/Underbuild white") as Sprite;
            }
            else if (building_id > 4 && building_id <= 6)
            {
                spt.sprite = Resources.Load<Sprite>("image/Groundbuild white") as Sprite;
            }
            else if (building_id > 6 && building_id <= 10)
            {
                spt.sprite = Resources.Load<Sprite>("image/Highbuild white") as Sprite;
            }
            Checkcheck = true;
        }
    }


    void initiateBuilding(int xPos, int yPos, BuildingData bd)
    {
        setBuilding(xPos, yPos);
        buildingData = bd;
        this.name = buildingData.buildingType.ToString();
        player_occupy = PLAYER.NONE;
        building_id = GameManager.GetInstance().giveID();
    }

    void setBuilding(int _x, int _y)
    {
        //클라이언트 측 실제로 빌딩 위치 시키는 코드
        x = _x;
        y = _y;
    }
    //아지트 타입에 따른 ui에 유닛 정보를 놓아 줌
    void give_ui_info()
    {
        //ui에 빌딩 아이디 보내줌
        switch (buildingData.buildingType)
        {
            case BuildingType.High:
                break;
            case BuildingType.Ground:
                break;
            case BuildingType.Under:
                break;
            default:
                break;
        }
    }
}
