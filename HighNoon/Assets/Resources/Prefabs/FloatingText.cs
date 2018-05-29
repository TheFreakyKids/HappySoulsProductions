using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public Animator anim;
    private Text damageText;

	void Awake ()
    {
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        damageText = anim.GetComponent<Text>();
        Destroy(gameObject, clipInfo[0].clip.length);
	}

    public void SetText (string text)
    {
        damageText.text = text;
	}
}