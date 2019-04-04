using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMove : MonoBehaviour
{
    static public CameraMove instance;
    Camera thecamera;

    public GameObject target;
    public int maxZoomSize;
    public int minZoomSize;

    public float zoomSpeed;
    public float speed;
    public float keyspeed;
    Vector3 setPos;
    Vector3 curPos;
    Vector3 keyPos;

    private Vector3 worldDefalutForward;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

    }

    private void Start()
    {
        thecamera = GetComponent<Camera>();
      
        worldDefalutForward = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
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



        if (Input.GetMouseButtonDown(1))
        {
            setPos.Set(Input.mousePosition.x, Input.mousePosition.y, this.transform.position.z);
            setPos = Camera.main.ScreenToWorldPoint(setPos);
        }

        if (Input.GetMouseButton(1))
        {
            curPos.Set(Input.mousePosition.x, Input.mousePosition.y, this.transform.position.z);
            curPos = Camera.main.ScreenToWorldPoint(curPos);
            transform.position = transform.position - (curPos - setPos);

        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * keyspeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {

            transform.Translate(Vector3.right * keyspeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * keyspeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * keyspeed * Time.deltaTime);
        }

    }
}
