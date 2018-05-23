using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public Animator anim;
    private Text damageText;

	void Awake ()
    {
		AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length);
        damageText = anim.GetComponent<Text>();
	}
	
	public void SetText (string text)
    {
        damageText.text = text;
	}
}