using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float rayDistance = 2f;      // Säteen pituus
    public PlayerUIHandler UIHandler;

    [Header("Weapons")]
    public List<Weapon> availableWeapons = new List<Weapon>();
    public Weapon currentEquippedWeapon = null;

    Camera cam;     // referenssi kameraan

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;      // Camera.main => Pääkamera joka pelissä on

        // JOS availableWeapons lista sisältää aseita jota voidaan käyttää
        if (availableWeapons.Count > 0)
        {
            // asetetaan currentEquippedWeaponiksi listan ensimmäinen ase
            ChangeWeapon(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleInteraction();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeWeapon(2);
        }
    }

    void ChangeWeapon(int weaponIndex)
    {
        // Jos vaihdetaan aseeseen jota ei ole olemassa/referoitu, niin poistutaan
        if (availableWeapons.Count < weaponIndex + 1) return;

        if (currentEquippedWeapon != null)
            currentEquippedWeapon.gameObject.SetActive(false); // Disabloidaan aiempi ase

        currentEquippedWeapon = availableWeapons[weaponIndex]; // asetetaan uuden aseen data
        currentEquippedWeapon.gameObject.SetActive(true); //Enabloidaan uusi ase

        UIHandler.UpdateWeaponUI(currentEquippedWeapon);
    }

    // Päivittää nykyisen aseen ammus-määrän käyttöliittymään
    // Tätä kutsutaan Weapon koodista kun ammutaan
    public void UpdateWeaponUI()
    {
        UIHandler.UpdateWeaponUI(currentEquippedWeapon);
    }

    void HandleInteraction()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, rayDistance))
        {
            // Jos osutaan, niin alla oleva toteutetaan jos se on Interactable
            if (hit.transform.GetComponent<Interaction>())
            {
                hit.transform.GetComponent<Interaction>().OnInteract();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Jos objektissa johon osuttiin on komponentti "Interaction"..
        if (other.GetComponent<Interaction>())
        {
            // .. Kutsutaan Interaction luokan metodia OnInteract
            other.GetComponent<Interaction>().OnInteract();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * rayDistance);
    }
}
