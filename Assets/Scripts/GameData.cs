using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType { UnitNotExist, Bully, Police, Soldier, Doctor, Investor }

public struct UnitData
{
    public UnitType unitType;
    public int atk;
    public int hp;
    public int intel;

    public int movement;
    public int range;
    public int sight;

    public UnitData(UnitType ut, int _atk, int _hp, int _intel, int _mv, int ran, int sig)
    {
        unitType = ut; atk = _atk; hp = _hp; intel = _intel; movement = _mv; range = ran; sight = sig;
    }
}

public class GameData
{
    // 유닛타입 어택 hp 인텔
    // 이동거리 거리 시야
    // 순서로 추가하고 싶은 데이터를 추가하면 됩ㄴ다.
    public static UnitData[] UnitDatas = new UnitData[]
    {
        new UnitData(UnitType.UnitNotExist, 0, 0, 0,
            0, 0, 0),
        new UnitData(UnitType.Bully, 5, 5, 5,
            4, 4, 4)
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
}





