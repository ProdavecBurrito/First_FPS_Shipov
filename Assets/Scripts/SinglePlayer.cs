using UnityEngine;
using System;
using Photon.Pun;
using UnityStandardAssets.Characters.FirstPerson;


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

    ISaveData xmlData;
    ISaveData jsonData;
    ISaveData streamData;

    FirstPersonController controller;
    PhotonView photon;

#if UNITY_EDITOR
    [SerializeField] int selfDmg = 10;
    [SerializeField] KeyCode damage = KeyCode.Tab;
#endif

    void Start()
    {
        controller = GetComponent<FirstPersonController>();
        photon = GetComponent<PhotonView>();
        
        xmlData = new XMLData();
        jsonData = new JSONData();
        streamData = new StreamData();

        Health = 100;

        _outLine = Shader.Find("Toon/Lit Outline");
        _base = Shader.Find("Legacy Shaders/Bumped Specular");
        MainCam = MainCamera.transform;

        PlayerData singlePlayer = new PlayerData
        {
            _Name = name,
            _Health = Health,
            _Dead = IsDead
        };

        PlayerPrefs.SetString("Name", singlePlayer._Name);
        PlayerPrefs.SetInt("Health", singlePlayer._Health);
        PlayerPrefs.SetInt("Dead", Convert.ToInt32(singlePlayer._Dead));
        PlayerPrefs.Save();

        xmlData.Save(singlePlayer);
        jsonData.Save(singlePlayer);
        streamData.Save(singlePlayer);

        PlayerData xmlNewPlayer = xmlData.Load();
        PlayerData jsonNewPlayer = jsonData.Load();
        PlayerData streemNewPlayer = streamData.Load();

#if UNITY_EDITOR
        Debug.Log(xmlNewPlayer);
        Debug.Log(jsonNewPlayer);
        Debug.Log(streemNewPlayer);
        Debug.Log(PlayerPrefs.GetString("Name"));
        Debug.Log(PlayerPrefs.GetInt("Health"));
        Debug.Log(PlayerPrefs.GetInt("Dead"));

        if (!photon.IsMine)
        {
            controller.enabled = false;
        }
        else
        {
            controller.enabled = true;
        }
#endif
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
