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
            if (other.GetComponent<Unit>().Health != 100)
            {
                HealUp(other.GetComponent<IAidKit>());
                Destroy(gameObject, 0.5f);
            }
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
