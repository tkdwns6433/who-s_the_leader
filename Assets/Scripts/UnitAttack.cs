using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttack : MonoBehaviour
{
    private int m_attacker;
    private int m_defender;
    Unit attackUnit; 
    Unit defendUnit;

    //본부 공격용
    public UnitAttack(int attacker)
    {
        Debug.Log("생성");
        m_attacker = attacker;
        attackUnit = GameManager.GetInstance.getUnit(m_attacker);
    }

    //특수한 버프 또는 능력 설정
    bool isNight;
    //만약에 내 단말에서 시행하는 공격이면 true이다.
    public void DoAttack(int defenderID)
    {
        Debug.Log("공격");
        m_defender = defenderID;
        defendUnit = GameManager.GetInstance.getUnit(m_defender);
        if (GameManager.GetInstance.myTurn)
            sendUnitAttack(); //네트워크에 상대방에 보내줌   
        setFlags();
        int damage = CalculateByDamageFomula();
        //어택 유닛의 애니메이션 작동
        //애니메이션 타이밍에 맞게 코루틴으로 일정시간 맞춰야함
        defendUnit.getDamage(damage);
    }

    void sendUnitAttack()
    {
        var m_network = GameObject.FindWithTag("Network").GetComponent<Network>();
        UnitAttackData data = new UnitAttackData();
        data.unitId = m_attacker;
        data.targetUnidId = m_defender;
        UnitAttackPacket attackPacket = new UnitAttackPacket(data);
        m_network.SendReliable(attackPacket);
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
        int damage = attackUnit.unitData.max_atk;
        Debug.Log(damage);
        return damage;
    }
}
