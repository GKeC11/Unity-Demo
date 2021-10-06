using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIKillInfoCard : MonoBehaviour
{
    public Text _killerText;
    public Text _killedText;

    private void Start()
    {
        Destroy(gameObject, 5.0f);
    }

    public void SetKillInfo(string killer, string killed)
    {
        _killerText.text = killer;
        _killedText.text = killed;
    }
}
