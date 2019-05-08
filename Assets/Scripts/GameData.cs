using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType { UnitNotExist, CommandCenter, Bully, Police, Soldier, Doctor, Investor, Mafiaunit }
public enum BuildingType { High, Ground, Under}


public struct UnitData
{
    public UnitType unitType;
    public int min_atk;
    public int max_atk;
    public int catk;
    public int hp;
    public int intel;
    

    public int movement;
    public int range;
    public int sight;

    public int cost;

    public UnitData(UnitType ut, int mi_atk, int mx_atk, int c_atk, int _hp, int _intel, int _mv, int ran, int sig, int c)
    {
        unitType = ut; min_atk = mi_atk; max_atk = mx_atk; catk = c_atk; hp = _hp; intel = _intel; movement = _mv; range = ran; sight = sig; cost = c;
    }
}

public struct BuildingData
{
    public BuildingType buildingType;
    int goldAvenue;

    public BuildingData(BuildingType bt, int gd)
    {
        buildingType = bt; goldAvenue = gd;
    }
}

public class GameData
{
    // 유닛타입 어택 hp 인텔
    // 이동거리 거리 시야 가격
    // 순서로 추가하고 싶은 데이터를 추가하면 됩니다.
    public static UnitData[] UnitDatas = new UnitData[]
    {
        new UnitData(UnitType.UnitNotExist,0 , 0, 0, 0, 0,
            0, 0, 0, 0),
        new UnitData(UnitType.Bully, 3, 5, 4, 5, 5,
            4, 4, 4, 5),
        new UnitData(UnitType.CommandCenter, 5, 5, 0, 0, 0,
            0, 4, 0, 0),
        new UnitData(UnitType.Mafiaunit, 3, 1,3,3,3,3,3,3,3)
    };

    public static BuildingData[] BuildingDatas = new BuildingData[]
    {
        new BuildingData(BuildingType.High, 100),
        new BuildingData(BuildingType.Ground, 50),
        new BuildingData(BuildingType.Under, 20)
    };

    public static UnitData getUnitData(UnitType ut)
    {
        foreach(var iter in UnitDatas)
        {
            if(ut == iter.unitType)
            {
                return iter;
            }
        }
        Debug.Log("unitType Unit does not exist");
        return UnitDatas[0];
    }

    public static BuildingData GetBuildingData(BuildingType bt)
    {
        foreach (var iter in BuildingDatas)
        {
            if (bt == iter.buildingType)
            {
                return iter;
            }
        }
        Debug.Log("unitType Unit does not exist");
        return BuildingDatas[0];
    }
}





