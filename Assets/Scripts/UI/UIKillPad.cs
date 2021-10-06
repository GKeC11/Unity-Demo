using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIKillPad : MonoBehaviour
{
    public GameObject _killCardPrefab;
    public GameObject _content;
    
    public void AddKillCard(string killer, string killed)
    {
        var killInfoCard = Instantiate(_killCardPrefab, _content.transform);
        killInfoCard.GetComponent<UIKillInfoCard>().SetKillInfo(killer, killed);
    }
    
}
