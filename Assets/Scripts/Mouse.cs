using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{

    void Update()
    {
        Vector3 tempV = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tempV.z = 0f;
        this.transform.position = tempV;

    }
    /*void OnMouseOver()
    {
        Debug.Log("마우스");
    }*/
        void OnTriggerStay2D(Collider2D other)
    {   //if(other.transform.tag == "UI")      
            //CameraMove.Instance.StopCamera(other);           
    }
    void OnTriggerExit2D(Collider2D other)
    {
        //if (other.transform.tag == "UI")
            //CameraMove.Instance.MoveCamera();
    }
}
