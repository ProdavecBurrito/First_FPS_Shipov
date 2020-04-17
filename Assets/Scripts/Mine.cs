using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour, IMine
{
    int dmg;
    float speed = 0.2f;
    float hight;
    float maxHight;

    private void Awake()
    {
        
    }


    public void DealDmg(int _dmg)
    {

    }

    public void Jump(float _hight)
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}


