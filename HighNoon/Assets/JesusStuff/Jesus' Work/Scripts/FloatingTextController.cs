using UnityEngine;

public class FloatingTextController : MonoBehaviour
{
    private static FloatingText popupText;

    public static void Initialize()
    {
        if(!popupText)
        popupText = Resources.Load<FloatingText>("Prefabs/DamNumPar");
    }

    public static void CreateFloatingText(string text, Transform location, int pNum, int playerWhoShot)
    {
        FloatingText instance = Instantiate(popupText);
        instance.SetText(text);
        instance.transform.SetParent(GameObject.Find("P" + pNum + "Canvas").transform, false);

        GameObject.Find("P" + pNum + "Canvas").GetComponent<BillboardCamFacer>().camToFace = GameObject.Find("Player" + playerWhoShot).transform.
            Find("FirstPersonCharacter/Revolver 1 - FPSController/Camera").GetComponent<Camera>();
    }
}