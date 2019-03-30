using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGenerator
{
    UnitType genUnit;
    int posX;
    int posY;
    int m_building_id;
    public UnitGenerator(int building_id, UnitType ut, int x, int y)
    {
        genUnit = ut;
        posX = x;
        posY = y;
        m_building_id = building_id;
    }

    public void GenerateUnit()
    {
        GameObject obj = new GameObject("Unit");
        obj.tag = "Unit";
        if (obj != null)
        {
            obj.AddComponent<Unit>();
            var unitComp = obj.GetComponent<Unit>();
            unitComp.intiateUnit(genUnit);
            unitComp.setPos(posX, posY);
            var player_occupy = GameManager.GetInstance().getBuilding(m_building_id).getPlayer();
            if (player_occupy == PLAYER.PLAYER1)
            {
                GameManager.GetInstance().player1.unitList.Add(unitComp);
                obj.tag = "Player1Unit";
                unitComp.control_player = PLAYER.PLAYER1;
                obj.GetComponent<Transform>().SetParent(GameManager.GetInstance().Player1Units.GetComponent<Transform>());
            }
            else if (player_occupy == PLAYER.PLAYER2)
            {
                GameManager.GetInstance().player2.unitList.Add(unitComp);
                obj.tag = "Player2Unit";
                unitComp.control_player = PLAYER.PLAYER2;
                obj.GetComponent<Transform>().SetParent(GameManager.GetInstance().Player2Units.GetComponent<Transform>());
            }
            else
            {
                Debug.Log("Error : building is not occupied");
                return;
            }
        }

        if(GameManager.GetInstance().myTurn)
        {
            var m_network = GameObject.FindWithTag("Network").GetComponent<Network>();
            UnitProduceData data = new UnitProduceData();
            data.buildingId = m_building_id;
            data.producedUnit = (int)genUnit;
            data.x = posX;
            data.y = posY;
            UnitProducePacket producePacket = new UnitProducePacket(data);
            m_network.SendReliable(producePacket);
        }
    }
}
