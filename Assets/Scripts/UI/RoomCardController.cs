using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class RoomCardController : MonoBehaviour
{
    public Text _roomNameText;

    public void ClickEventJoinRoom()
    {
        NetworkManager._instance.JoinRoom(_roomNameText.text);
    }
}
