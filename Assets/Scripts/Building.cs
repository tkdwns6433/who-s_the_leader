using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int building_id;
    public PLAYER player_occupy;

    public int x;
    public int y;

    public BuildingData buildingData;

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
