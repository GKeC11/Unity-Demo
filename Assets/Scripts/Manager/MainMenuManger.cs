using System.Collections;
using System.Collections.Generic;
using Manager;
using Manager.Utils;
using UnityEngine;
using Utils;

public class MainMenuManger : Singleton<MainMenuManger>, IManager
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitializeCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Initialize()
    {
        GameManager._instance.ReInitializeMainMenu();
    }

    IEnumerator InitializeCoroutine()
    {
        while (!GameManager._instance.isGameManagerInitializeFinish)
        {
            yield return new WaitForSeconds(1.0f);
        }

        Initialize();
    }
}