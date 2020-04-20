using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [Header("Настройки мины")]
    [SerializeField ] int dmg;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] Animator animator;

     void Awake()
    {
        animator = GetComponent<Animator>();
        explosion = GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "Player" || obj.tag == "Enemy")
        {
            animator.SetTrigger("Jump");
            Invoke("Expl", 0.5f);
            Destroy(gameObject, 0.7f);
            obj.GetComponent<ISetDmg>().SetDmg(dmg);
        }
    }

    void Expl()
    {
        explosion.Play();
    }
}


