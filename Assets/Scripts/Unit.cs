using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : BaseObject, ISetDmg, IAidKit
{

    [Range(50, 150)][SerializeField] int _health;
    bool isDead;

    public int Health { get => _health; set => _health = value; }
    public bool IsDead { get => isDead; set => isDead = value; }

    public void SetDmg(int dmg)
    {
        if (_health > 0)
        {
            _health -= dmg;
        }
        else if (_health <= 0)
        {
            _health = 0;

            if (tag != "Player")
            {
                IsDead = true;
            }
        }
    }

    public void AddHeal(int heal)
    {
        if (_health < 100)
        {

            _health += heal;
            
            if (_health > 100)
            {
                _health = 100;
            }
        }
    }
}
