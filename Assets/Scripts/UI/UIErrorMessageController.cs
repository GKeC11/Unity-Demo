using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIErrorMessageController : MonoBehaviour
{
    public Text _messageText;
    public Action _buttonAction; 

    public void ClickEventPositiveButton()
    {
        _buttonAction();
        Destroy(gameObject);
    }
}
