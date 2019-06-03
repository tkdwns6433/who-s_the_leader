using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public Text screenSizText;
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
    public float width;
    void LateUpdate()
    {
        
        ZoomInOut();
       
        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //Vector3 camPos = Camera.main.ScreenToViewportPoint(transform.position);
        Vector2 camPos = transform.position;
 
        float width = 7680 / Screen.width;
        if (pos.x < 0.05f && camPos.x > 530) transform.Translate(Vector3.left * mouseSpeed);
        if (pos.x > 0.95f && camPos.x < 4530) transform.Translate(Vector3.right * mouseSpeed);
        if (pos.y < 0.05f && camPos.y > 300) transform.Translate(Vector3.down * mouseSpeed);
        if (pos.y > 0.95f && camPos.y < 1300) transform.Translate(Vector3.up * mouseSpeed);
        
        
       


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
