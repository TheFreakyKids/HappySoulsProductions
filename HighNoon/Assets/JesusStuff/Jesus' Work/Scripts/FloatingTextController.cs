using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextController : MonoBehaviour
{
    private static FloatingText popupText;
    private static GameObject[] canvas;

    public static void Initialize()
    {
        canvas = GameObject.FindGameObjectsWithTag("PCanvas");
        if(!popupText)
        popupText = Resources.Load<FloatingText>("Prefabs/DamNumParent");
    }

    public static void CreateFloatingText(string text, Transform location, int pNum)
    {
        FloatingText instance = Instantiate(popupText);
        //Vector2 screenPos = Camera.main.WorldToScreenPoint(location.position);
        //Vector2 screenPos = GameObject.Find("Player" + pNum).GetComponentInChildren<Camera>().WorldToScreenPoint(location.position);
        instance.transform.SetParent(canvas[pNum-1].transform, false);
        //instance.transform.position = screenPos;
        instance.SetText(text);
    }
}