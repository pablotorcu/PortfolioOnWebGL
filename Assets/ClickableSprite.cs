using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableSprite : MonoBehaviour
{
    public string url;

    private void OnMouseDown()
    {
        Application.OpenURL(url);
    }
}
