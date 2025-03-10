using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    [Header("Base Stats")]
    public string weaponName;
    public int damage;       // aseen damage
    public float fireRate;   // Kuinka usein voidaan ampua

    [Header("Ammo")]
    public int clipSizeMax = 30;    // Maksimi lippaan m‰‰r‰ ammuksia
    public float reloadTime = 1f;   // kuinka kauan kest‰‰ reloadata lipas
    public int currentClipSize = 0;    // Kuinka monta kutia j‰ljell‰ nykyisess‰ lippaassa
    private bool isReloading = false;   // true/false ollaanko reloadaamassa

    [Header("Sounds")]
    public AudioClip[] onFireClips; // Array lista ampumis ‰‰niklipeist‰
    public AudioClip onFireSoundOnEmptyMag;   // ƒ‰nieffekti kun ammutaan tyhj‰ll‰ lippaalla
    public AudioClip reloadSound;   // ƒ‰nieffekti kun reloadataan ase

    private AudioSource audioSource;

    private Transform firePoint;    // Kohta mist‰ s‰de ammutaan
    private bool canFire = true;    // Fireraten mukaan asetetaan true/false

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        firePoint = Camera.main.transform;   // haetaan pelaajan kameran sijainti
        audioSource = GetComponent<AudioSource>();
        currentClipSize = clipSizeMax;  // asetetaan t‰ysi lipas kun peli k‰ynnistet‰‰n
    }

    // Update is called once per frame
    void Update()
    {
        // JOS canFire == true ja pelaaja painaa Left Mouse Btn, niin ammutaan
        if (canFire && Input.GetMouseButton(0))
        {
            OnFire();
        }

        // JOS pelaaja voi ampua JA pelajaa ei ole reloadaamassa JA painaa R-n‰pp‰int‰..
        //          isReloading == false
        if (canFire && !isReloading && Input.GetKeyDown(KeyCode.R))
        {
            // Jos aseen nykyinen luoti m‰‰r‰ EI OLE sama kuin maksimi m‰‰r‰..
            if (currentClipSize != clipSizeMax)
            {
                // Aloitetaan aseen reloadus
                StartCoroutine(OnReload());
            }
        }
    }

    IEnumerator OnReload()
    {
        isReloading = true;    // aloitetaan reload
        audioSource.PlayOneShot(reloadSound);   // toistetaan reload ‰‰nieffekti
        yield return new WaitForSeconds(reloadTime); // odotetaan "ReloadTime" verran
        isReloading = false;      // kun aika on odotettu, lopetetaan reload
        currentClipSize = clipSizeMax;  // asetetaan uusi lipas, clipSizeMax m‰‰r‰n mukaan
        GetComponentInParent<PlayerInteraction>().UpdateWeaponUI(); // P‰ivitet‰‰n aseen k‰yttˆliittym‰
    }

    void OnFire()
    {
        // JOS lippaassa ei ole luoteja, poistutaan eik‰ tehd‰ mit‰‰n
        if (currentClipSize <= 0)
        {
            audioSource.PlayOneShot(onFireSoundOnEmptyMag);
            StartCoroutine(WaitUntilCanFire());
            return;
        }

        RaycastHit hit;

        // ammutaan s‰de kamerasta eteenp‰in
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit))
        {
            // JOS osuma objektissa on komponentti "Health"..
            if (hit.transform.GetComponent<Health>())
            {
                hit.transform.GetComponent<Health>().TakeDamage(damage);
            }
        }

        currentClipSize--; // V‰hennet‰‰n nykyisen lippaan luoti m‰‰r‰‰ yhdell‰
        int randomClip = Random.Range(0, onFireClips.Length); // Arpoo mik‰ onFire ‰‰ni toistetaan
        audioSource.PlayOneShot(onFireClips[randomClip]); // Toisteaan arvottu ampumis ‰‰ni
        GetComponentInParent<PlayerInteraction>().UpdateWeaponUI(); // P‰ivitet‰‰n aseen k‰yttˆliittym‰

        // Aloitetaan Coroutine, joka est‰‰ ampumisen ja odottaa ennenkuin voidaan ampua uudestaan
        StartCoroutine(WaitUntilCanFire());
    }

    // IEnumerator mahdollistaa aseen fireRate cooldown systeemin
    // Tehd‰‰n jotain -> Odotetaan tietty aika -> tehd‰‰n sen j‰lkeen jotain
    IEnumerator WaitUntilCanFire()
    {
        canFire = false;   // Pelaaja ei voi ampua
        yield return new WaitForSeconds(fireRate);  // Odotetaan FireRate:n verran
        canFire = true;    // Pelaaja voi taas ampua
    }

}
