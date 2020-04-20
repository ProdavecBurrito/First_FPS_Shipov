using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeapon : BaseObject
{
    int previousWeapon;
    int weaponID = 0;

    protected override void Awake()
    {
        base.Awake();
        SelectWeapon();
    }

    void Update()
    {
        previousWeapon = weaponID;

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (weaponID == 0)
            {
                weaponID = ChildCounter - 1;
            }
            else
            {
                weaponID--;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (weaponID == ChildCounter - 1)
            {
                weaponID = 0;
            }
            else
            {
                weaponID++;
            }
        }

        if (previousWeapon != weaponID)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in Transform)
        {
            if (i == weaponID)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
