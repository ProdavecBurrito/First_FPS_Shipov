using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : BaseObject
{
    [SerializeField] protected Transform fireStart;
    protected ParticleSystem muzzleFlash;
    [SerializeField] protected GameObject hitParticle;
    protected Timer restartTimer = new Timer();
    protected GameObject crossHair;
    [SerializeField] protected AudioClip[] gunSound;
    protected AudioSource _audio;

    protected bool fire = true;

    public abstract void Fire();

    protected override void Awake()
    {
        base.Awake();

        fireStart = FindChildTag();
        muzzleFlash = GetComponentInChildren<ParticleSystem>();
        hitParticle = Resources.Load<GameObject>("Preffabs/Flare");
        crossHair = GameObject.FindGameObjectWithTag("Cross");
    }

    private void Update()
    {
        restartTimer.Update();
        if (restartTimer.CanFire())
        {
            fire = true;
        }
    }

    // Поиск обьекта с тегом FireStartPosition
    Transform FindChildTag()
    {
        foreach (Transform child in Transform)
        {
            if (child.tag == "FireStartPosition")
            {
                return child;
            }
        }
        return null;
    }
}
