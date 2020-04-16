using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : BaseWeapon
{
    [SerializeField] int ammo;
    int maxMag;
    [SerializeField] float hitDistance;
    [SerializeField] int dmg;
    [SerializeField] bool pref;

    GameObject bullet;
    LineRenderer line;

    KeyCode reload = KeyCode.R;

    Transform mCam;

    protected override void Awake()
    {
        base.Awake();
        maxMag = ammo;
    }

    private void Start()
    {
        mCam = MainCamera.transform;
        if (pref)
        {
            bullet = Resources.Load<GameObject>("Preffabs/7.62");
            line = GetComponent<LineRenderer>();
            line.startWidth = 0.02f;
            line.endWidth = 0.02f;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
        if (Input.GetKeyDown(reload) && ammo != maxMag)
        {
            fire = false;
            Anim.SetTrigger("Reload");
            //_audio.PlayOneShot(gunSound[1]);
        }
    }

    public override void Fire()
    {
        if (ammo > 0 && fire)
        {
            muzzleFlash.Play();
            ammo--;
            Anim.SetTrigger("Shoot");
            //_audio.PlayOneShot(gunSound[0]);
            // Для стрельбы префабами
            if (pref)
            {
                RaycastHit hit;
                Ray ray = MainCamera.ScreenPointToRay(crossHair.transform.position);
                GameObject bulletTemp = Instantiate(bullet, fireStart.position, Quaternion.identity);
                Rigidbody bulletRB = bulletTemp.GetComponent<Rigidbody>();
                line.SetPosition(0, fireStart.position);
                if (Physics.Raycast(ray, out hit, hitDistance))
                {
                    bulletRB.AddForce(GetDirection(bulletTemp.transform.position, hit.point) * 5, ForceMode.Impulse);
                    line.SetPosition(1, hit.point);
                    CreatePartHit(hit);
                }
                else
                {
                    bulletRB.AddForce(GetDirection(bulletTemp.transform.position, ray.GetPoint(10000f)) * 5, ForceMode.Impulse);
                    line.SetPosition(1, ray.GetPoint(10000f));
                }

            }
            // Для стрельбы без прифабов
            else
            {
                RaycastHit hit;
                Ray ray = new Ray(mCam.position, mCam.forward);
                if (Physics.Raycast(ray, out hit, hitDistance))
                {
                    if (hit.collider.tag == "Player")
                    {
                        return;
                    }
                    else
                    {
                        SetDmg(hit.collider.GetComponent<ISetDmg>());
                        CreatePartHit(hit);
                    }
                }
            }
        }
        else if (ammo <= 0 && fire)
        {
            fire = false;
        }

    }

    /// <summary>
    /// Метод для получения вектора пули
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="hitPoint"></param>
    /// <returns></returns>
    Vector3 GetDirection (Vector3 startPos, Vector3 hitPoint)
    {
        // Направление
        Vector3 decr = hitPoint - startPos;
        // Длинна
        float dist = decr.magnitude;
        // Вектор
        return decr / dist;

    }

    void Reloaded()
    {
        ammo = maxMag;
        fire = true;
    }

    void SetDmg(ISetDmg obj)
    {
        if (obj != null)
        {
            obj.SetDmg(dmg);
        }
    }

    // Создание частиц в месте попадания
    void CreatePartHit(RaycastHit hit)
    {
        GameObject tempHit = Instantiate(hitParticle, hit.point, Quaternion.identity);
        tempHit.transform.parent = hit.transform;
        Destroy(tempHit, 0.2f);
    }
}
