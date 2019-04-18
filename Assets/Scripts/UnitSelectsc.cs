using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectsc : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (this.gameObject.tag == "MafiaUnit")
        {
            GameUIManager.Instance.build.Unitgenset(UnitType.Mafiaunit);
        }
    }
}
