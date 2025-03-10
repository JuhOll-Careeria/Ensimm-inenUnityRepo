using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHandler : MonoBehaviour
{
    public Image weaponImageIcon;           // referoitu "Image" komponentti jos halutaan vaihtaa aseen ikonia
    public TextMeshProUGUI weaponNameTxt;   // aseen nimen tekstikentt‰
    public TextMeshProUGUI weaponAmmoTxt;   // aseen ammusten m‰‰r‰ + max magazine size

    // P‰ivitet‰‰n Aseen UI n‰kym‰ aina kun vaihdetaan asetta / ammutaan
    public void UpdateWeaponUI(Weapon currentWeapon)
    {
        // weaponImageIcon.sprite = currentWeapon.icon;
        weaponNameTxt.text = currentWeapon.weaponName;
        weaponAmmoTxt.text = currentWeapon.currentClipSize + " / " + currentWeapon.clipSizeMax;
    }
}
