using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : BaseObject
{
    int dmg = 30;
    float liveTime = 5f;

    protected override void Awake()
    {
        base.Awake();
        Destroy(Instance, liveTime);
    }

    protected virtual void SetDmg(ISetDmg obj)
    {
        if (obj != null)
        {
            obj.SetDmg(dmg);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        SetDmg(collision.gameObject.GetComponent<ISetDmg>());
        Destroy(gameObject);
    }
}
