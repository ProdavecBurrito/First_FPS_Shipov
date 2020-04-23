using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public struct PlayerData
{
    public string _Name;
    public int _Health;
    public bool _Dead;

    public override string ToString()
    {
        return $"Name: {_Name} Health: {_Health} Dead: {_Dead}";
    }
}

public class SinglePlayer : Unit
{
    // Наложение шейдера на выделенный обьект
    [SerializeField] Shader _outLine;
    // Базовый шейдер
    [SerializeField] Shader _base;
    [SerializeField] bool _selected;
    [SerializeField] GameObject _tempGo;
    RaycastHit Hit;
    Transform MainCam;

    ISaveData data;

#if UNITY_EDITOR
    [SerializeField] int selfDmg = 10;
    [SerializeField] KeyCode damage = KeyCode.Tab;
#endif

    void Start()
    {
        //data = ;
        Health = 100;

        _outLine = Shader.Find("Toon/Lit Outline");
        _base = Shader.Find("Legacy Shaders/Bumped Specular");
        MainCam = MainCamera.transform;

        PlayerData playerData = new PlayerData
        {
            _Name = name,
            _Health = Health,
            _Dead = IsDead
        };

        //Save
        //Load
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(damage))
        {
            Health -= selfDmg;
        }
#endif

        // При попадании raycast в обьект с теггом "Pickup" - обьект помечается как selected
        if (Physics.Raycast(MainCam.position, MainCam.forward, out Hit, 4f))
        {
            Collider target = Hit.collider;
            if (target.tag == "Pickup")
            {
                if (!_tempGo)
                {
                    _tempGo = target.gameObject;
                }
                _selected = true;
                if (_tempGo.GetInstanceID() != target.gameObject.GetInstanceID())
                {
                    _selected = false;
                    _tempGo = null;
                }
            }
            else
            {
                _selected = false;
            }
        }
        else
        {
            _selected = false;
        }

        // Наложение шейдера
        if (_selected)
        {
            foreach (Transform item in _tempGo.transform)
            {
                if (item.GetComponent<Renderer>())
                {
                    item.GetComponent<Renderer>().material.shader = _outLine;
                }
            }
        }
        else
        {
            if (_tempGo)
            {
                foreach (Transform item in _tempGo.transform)
                {
                    if (item.GetComponent<Renderer>())
                    {
                        item.GetComponent<Renderer>().material.shader = _base;
                    }
                }
            }
        }
    }
}
