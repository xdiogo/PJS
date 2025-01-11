using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public static CursorManager current;
    public Image cursorimage;
    public Sprite interactionsprite;
    // Update is called once per frame
    private void Awake()
    {
        cursorimage.enabled = false;
        current = this;
    }

    public void SetcursorInteraction()
    {
        cursorimage.sprite = interactionsprite;
        cursorimage.enabled = true;
    }

    public void Setcursornone()
    {
        cursorimage.enabled = false;
    }
}
