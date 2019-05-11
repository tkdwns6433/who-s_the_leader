using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMove : MonoBehaviour
{
    static private CameraMove instance;
    
    Camera thecamera;

    public GameObject target;
    public int maxZoomSize;
    public int minZoomSize;
    
    public float zoomSpeed;
    public float speed;
    public float mouseSpeed;


    public float mouse_speedX = 3.0f;    //마우스 좌우
    public float mouse_speedY = 3.0f;    //마우스 상하
    float rotationY = 0f;



    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        thecamera = GetComponent<Camera>();


    }
 


    Vector3 mousePos;
    Vector3 refVelov;
    public void MoveCamera(MovePanel panel)
    {
        if (panel.name == "Left")
            transform.Translate(Vector3.left* mouseSpeed);
        else if (panel.name == "Right")
            transform.Translate(Vector3.right * mouseSpeed);
        else if(panel.name == "Up")
            transform.Translate(Vector3.up * mouseSpeed);

    }
    void LateUpdate()
    {
        
        ZoomInOut();
       
        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        if (pos.x < 0.05f) transform.Translate(Vector3.left * mouseSpeed);
        if (pos.x > 0.95f) transform.Translate(Vector3.right * mouseSpeed);
        if (pos.y < 0.05f) transform.Translate(Vector3.down * mouseSpeed);
        if (pos.y > 0.95f) transform.Translate(Vector3.up * mouseSpeed);/* */
        /*
                transform.position = Camera.main.ViewportToWorldPoint(pos);
               
                //마우스 위치에 따른 카메라 이동 
                //방법1

                mousePos = transform.position;
                float rotationX = Input.GetAxis("Mouse X")* mouse_speedX;
                float rotationY = Input.GetAxis("Mouse Y") * mouse_speedY;
                if (rotationX < 0)
                {
                    mousePos.x -= mouseSpeed;
                }
                else if (rotationX > 0)
                {
                    mousePos.x += mouseSpeed;
                }     
                if (rotationY < 0)
                {
                    mousePos.y -= mouseSpeed;
                }
                else if (rotationY > 0)
                {
                    mousePos.y += mouseSpeed;
                }

                //방법2
                Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                temp.z = 0f;
                temp.x = Mathf.Clamp(temp.x, -2130, 3031);
                temp.y = Mathf.Clamp(temp.y, -811, 990);
                Vector3 tempV = Vector3.SmoothDamp(transform.position, temp, ref refVelov, 2.5f);

                tempV.z = -10f;

                transform.position = tempV;
                */


    }

    void ZoomInOut()
    {
        //카메라 줌&아웃 부분
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float zoomSize = thecamera.orthographicSize;
        if (scroll < 0)
        {
            zoomSize -= zoomSpeed;
        }
        else if (scroll > 0)
        {
            zoomSize += zoomSpeed;
        }
        thecamera.orthographicSize = Mathf.Clamp(zoomSize, minZoomSize, maxZoomSize); ;

    }

}
