using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSceneManager : MySceneManager
{
    public GameObject clearSpriteObj;
    public GameObject creditSpriteObj;
    private SpriteRenderer clearSprite;
    private SpriteRenderer creditSprite;

    private float clearAlpha = 0.0f;
    private float creditAlpha = 0.0f;

    private bool transitionScene = false;

    enum DISPLAY_IMAGE
    {
        CLEAR,
        CREDIT,
        END
    }
    DISPLAY_IMAGE displayedImage = DISPLAY_IMAGE.CLEAR;

    void Start ()
    {
        StartCoroutine(FadeIn());
        clearSprite = clearSpriteObj.gameObject.GetComponent<SpriteRenderer>();
        creditSprite = creditSpriteObj.gameObject.GetComponent<SpriteRenderer>();

        clearSprite.color = new Color(1.0f, 1.0f, 1.0f, clearAlpha);
        creditSprite.color = new Color(1.0f, 1.0f, 1.0f, creditAlpha);
    }

    void Update()
    {
        switch (displayedImage)
        {
            case DISPLAY_IMAGE.CLEAR:
                if(clearAlpha <= 1.0f)
                {
                    clearAlpha+= 0.1f;
                    clearSprite.color = new Color(1.0f, 1.0f, 1.0f, clearAlpha);
                }
                break;
            case DISPLAY_IMAGE.CREDIT:
                if (creditAlpha <= 1.0f)
                {
                    creditAlpha+=0.1f;
                    creditSprite.color = new Color(1.0f, 1.0f, 1.0f, creditAlpha);
                }
                break;
            case DISPLAY_IMAGE.END:
                break;
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            displayedImage++;

            if(displayedImage == DISPLAY_IMAGE.END)
            {
                if (!transitionScene)
                {
                    transitionScene = true;
                    TranditionScene(SCENES.TITLE);
                }
            }
        }
    }
}
