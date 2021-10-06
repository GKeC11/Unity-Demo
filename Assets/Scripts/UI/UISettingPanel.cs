using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class UISettingPanel : MonoBehaviour
{
    public Slider _mouseSensitiveSlider;
    public Text _mouseSensitiveText;

    private void Start()
    {
        _mouseSensitiveSlider.minValue = 1;
        _mouseSensitiveSlider.maxValue = 100;
        _mouseSensitiveSlider.value = GameplayManager._instance._player.GetComponent<PlayerController>().mouseSensitivity;
    }

    public void MouseSensitiveSliderUpdate()
    {
        GameplayManager._instance._player.GetComponent<PlayerController>().mouseSensitivity = _mouseSensitiveSlider.value;
        _mouseSensitiveText.text = GameplayManager._instance._player.GetComponent<PlayerController>().mouseSensitivity.ToString();
    }
}
