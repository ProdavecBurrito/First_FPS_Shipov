using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour, IMine
{
    int dmg;
    float speed = 2;
    float hight;
    float maxHight;


    public void DealDmg(int _dmg)
    {

    }

    public void Jump(float _hight)
    {
        hight = transform.position + Vector3.up;
    }
}


