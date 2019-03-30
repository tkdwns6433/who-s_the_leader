using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMove : MonoBehaviour
{
    static public CameraMove instance;
    Camera thecamera;

    public GameObject target;

    public float speed;
    public float keyspeed;
    Vector3 setPos;
    Vector3 curPos;
    Vector3 keyPos;

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
        //thecamera.orthographicSize = 540;
        keyPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
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

        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.Translate(Vector3.left * keyspeed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.Translate(Vector3.right * keyspeed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.Translate(Vector3.up * keyspeed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.Translate(Vector3.down * keyspeed * Time.deltaTime);
        }

    }
}
