using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Базовый класс для всех обьектов
/// </summary>
public abstract class BaseObject : MonoBehaviour
{
    private Transform _transform;
    private GameObject _instance;

    private string _name;

    private bool _isVisible;

    private Vector3 _position;
    private Quaternion _rotation;
    private Vector3 _scale;

    private Material _material;
    private Rigidbody _rigBody;
    private Camera _mainCamera;
    private Animator _animator;

    /// <summary>
    /// Солличество доч. обьектов
    /// </summary>
    protected int ChildCounter { get => Transform.childCount; }
    protected GameObject Instance { get => _instance;}
    protected Material Material { get => _material;}
    protected Camera MainCamera { get => _mainCamera;}
    protected Animator Animator { get => _animator;}
    protected Rigidbody RigBody { get => _rigBody;}
    protected Transform Transform { get => _transform;}
    protected string Name { get => _name; set => _name = value; }
    protected Vector3 Position { get => _position; set => _position = value; }
    protected Quaternion Rotation { get => _rotation; set => _rotation = value; }
    protected Vector3 Scale { get => _scale; set => _scale = value; }
    protected bool IsVisible
    {
        get => _isVisible;
        set
        {
            _isVisible = value;
            if (_instance.GetComponent<Renderer>())
            {
                _instance.GetComponent<Renderer>().enabled = _isVisible;
            }
        }
    }

    protected virtual void Awake()
    {
        _instance = gameObject;
        _transform = gameObject.transform;
        name = gameObject.name;
        _mainCamera = Camera.main;

        if (GetComponent<Rigidbody>())
        {
            _rigBody = GetComponent<Rigidbody>();
        }

        if (GetComponent<Animator>())
        {
            _animator = GetComponent<Animator>();
        }

        if (GetComponent<Renderer>())
        {
            _material = GetComponent<Renderer>().material;
        }
    }
}
