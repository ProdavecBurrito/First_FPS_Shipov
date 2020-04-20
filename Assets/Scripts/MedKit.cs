using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : MonoBehaviour
{

    [Range(10, 50)]
    [SerializeField] int heal = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            HealUp(other.GetComponent<IAidKit>());
        }
    }

    void HealUp(IAidKit obj)
    {
        if (obj != null)
        {
            obj.AddHeal(heal);
            Destroy(gameObject);
        }
    }

}
