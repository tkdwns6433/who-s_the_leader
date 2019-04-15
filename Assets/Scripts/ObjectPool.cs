using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private PoolableObject poolObj;
    [SerializeField]
    private int allocateCount;
    private Stack<PoolableObject> stack = new Stack<PoolableObject>();
    public Transform parentTf;

    void Start()
    {
        Allocate();
    }
    public int mapRow = 44;
    public int mapCol = 1;
    public void Allocate()
    {
        for(int j = 0; j< mapCol; j++)
        {
            for (int i = 0; i < mapRow; i++)
            {
                PoolableObject tObj = Instantiate(poolObj, parentTf);
                Vector3 tempV = tObj.transform.position;
                tempV.x += 120f * i;
                
                //tempV.y -= 450f * j;

                tObj.transform.position = tempV;
                tObj.Create(this);
                stack.Push(tObj);
            }
        }
       
    }

    public GameObject PopObject()
    {
        PoolableObject obj = stack.Pop();
        obj.gameObject.SetActive(true);
        return obj.gameObject;
    }

    public void PushObject(PoolableObject obj)
    {
        obj.gameObject.SetActive(false);
        stack.Push(obj);
    }
}
