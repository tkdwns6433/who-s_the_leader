using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttack
{
    private int m_attacker;
    private int m_defender;
    Unit attackUnit; 
    Unit defendUnit;
    public UnitAttack(int attacker, int defender)
    {
        m_attacker = attacker;
        m_defender = defender;
        attackUnit = GameManager.GetInstance().getUnit(m_attacker);
        defendUnit = GameManager.GetInstance().getUnit(m_defender);
    }

    //특수한 버프 또는 능력 설정
    bool isNight;

    public void DoAttack()
    {
        setFlags();
        int damage = CalculateByDamageFomula();
        //어택 유닛의 애니메이션 작동
        //애니메이션 타이밍에 맞게 코루틴으로 일정시간 맞춰야함
        defendUnit.getDamage(damage);
        
    }
    //is Night 등 밤 여부, 개발하면서 추가되는 다양한 플래그를 설정해주는 함수
    void setFlags()
    {
        switch (attackUnit.unitData.unitType)
        {
            case UnitType.Bully:
                break;
            case UnitType.Police:
                break;
            case UnitType.Soldier:
                break;
            case UnitType.Doctor:
                break;
            case UnitType.Investor:
                break;
            default:
                break;
        }
        switch (defendUnit.unitData.unitType)
        {
            case UnitType.Bully:
                break;
            case UnitType.Police:
                break;
            case UnitType.Soldier:
                break;
            case UnitType.Doctor:
                break;
            case UnitType.Investor:
                break;
            default:
                break;
        }
    }

    int CalculateByDamageFomula()
    {
        //flag 설정한 걸로 데미지 공식 설정
        int damage = attackUnit.unitData.atk;
        return damage;
    }
}
