using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class Knife : MonoBehaviour
{
    public int playerNum;
    public float damage = 50f;
    public ArmControllerScript arms;
    public Animator anim;
    public bool stillStabbing = false;
    public float stabDowntime = 3f;
    public string parentObj;
    public AudioClip stab;

	public void Awake ()
    {
        string numberOnly = Regex.Replace(transform.root.name, "[^0-9]", "");
        playerNum = int.Parse(numberOnly);
        arms = transform.parent.transform.parent.GetComponent<ArmControllerScript>();
        anim = arms.gameObject.GetComponent<Animator>();
        parentObj = transform.root.gameObject.name;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!stillStabbing && arms.isMeleeAttacking)
        {
            if (collision.transform.CompareTag("Player") && collision.transform.root.name != parentObj)
            {
                StartCoroutine(Stab(collision));
            }
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        if (!stillStabbing && arms.isMeleeAttacking)
        {
            if (collision.transform.CompareTag("Player") && collision.transform.root.name != parentObj)
            {
                StartCoroutine(Stab(collision));
            }
        }
    }

    public IEnumerator Stab(Collision collsion)
    {
        stillStabbing = true;

        collsion.transform.root.GetComponent<Player>().TakeDamage(damage, playerNum);
        SoundManager.instance.Play(stab, "sfx");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        stillStabbing = false;
    }
}