using UnityEngine;

public class WeaponSwapping : MonoBehaviour
{
    [SerializeField] private int selectedWeapon = 0;
    public bool hasSwitched = false;
    public AudioClip swap;

	void Start ()
    {
        SelectWeapon();	
	}
	
	void Update ()
    {
        int previousSelectedWeapon = selectedWeapon;

        print(Input.GetAxis("DPAD Vertical"));

        if (Input.GetAxis("DPAD Vertical") > 0f && hasSwitched == false)
        {
            SoundManager.instance.Play(swap, "swap");
            if (selectedWeapon >= transform.childCount - 1)
            {
                print("DPAD RIGHT");
                hasSwitched = true;
                selectedWeapon = 0;
            }
            else
            {
                hasSwitched = true;
                selectedWeapon++;
            }
        }
        
        if (Input.GetAxis("DPAD Vertical") < 0f && hasSwitched == false)
        {
            SoundManager.instance.Play(swap, "swap");
            if (selectedWeapon <= 0)
            {
                print("DPAD LEFT");
                hasSwitched = true;
                selectedWeapon = transform.childCount - 1;
            }
            else
            {
                print("DPAD NULL");
                hasSwitched = true;
                selectedWeapon--;
            }
        }
        if (Input.GetAxis("DPAD Vertical") == 0f)
        {
            hasSwitched = false;
        }

        #region Keyboard Settings
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            selectedWeapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            selectedWeapon = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4)
        {
            selectedWeapon = 3;
        }
        #endregion

        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}