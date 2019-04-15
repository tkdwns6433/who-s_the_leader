using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    protected ObjectPool Pool;

    public virtual void Create(ObjectPool pool)
    {
        Pool = pool;
        gameObject.SetActive(true);
    }

    public virtual void Push()
    {
        Pool.PushObject(this);
    }
}
