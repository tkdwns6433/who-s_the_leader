using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGenerator : MonoBehaviour
{

    public void GenerateUnit(int building_id, UnitType unitType, float x, float y)
    {
        GameObject newUnit = Instantiate(Resources.Load("Prefabs/Unit")) as GameObject;
        if (newUnit != null)
        {
            PLAYER request_player = GameManager.GetInstance.getBuilding(building_id).player_occupy;
            newUnit.GetComponent<Unit>().initiateUnit(unitType, x, y, request_player);
            GameManager.GetInstance.subtractGold(request_player, GameData.getUnitData(unitType).cost);
        }
        else
        {
            Debug.Log("unit is not instantiated");
            return;
        }

        if(GameManager.GetInstance.myTurn)
        {
            var m_network = GameObject.FindWithTag("Network").GetComponent<Network>();
            UnitProduceData data = new UnitProduceData();
            data.buildingId = building_id;
            data.producedUnit = (int)unitType;
            //data.x = x;
            //data.y = y;
            UnitProducePacket producePacket = new UnitProducePacket(data);
            m_network.SendReliable(producePacket);
        }
    }
}
