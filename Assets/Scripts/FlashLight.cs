using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashLight : BaseObject
{
    private KeyCode power = KeyCode.F;

    [SerializeField] Slider _lightWork;

    private Light _light;

    private float _workTime = 10f;
    private float _currentTime;
    private float _timeLeft = 10f;

    protected override void Awake()
    {
        base.Awake();
        _light = GetComponentInChildren<Light>();
    }

    void Update()
    {
        _lightWork.value = _timeLeft;

        if (Input.GetKeyDown(power) && ! _light.enabled)
        {
            ActivLight(true);
        }
        else if (Input.GetKeyDown(power) && _light.enabled)
        {
            _timeLeft = 10;
            ActivLight(false);
        }

        if (_light.enabled)
        {
            _timeLeft -= Time.deltaTime;
            if (_timeLeft < 0)
            {
                _timeLeft = 10;
                ActivLight(false);
            }
        }
    }

    private void ActivLight(bool val)
    {
        _light.enabled = val;
    }
}
