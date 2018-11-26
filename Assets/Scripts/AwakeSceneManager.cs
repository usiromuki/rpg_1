using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 起動時のシーンの管理
public class AwakeSceneManager : MySceneManager
{
    private void Start()
    {
        TranditionScene(SCENES.TITLE);
    }
}
