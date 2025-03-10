using UnityEngine;
using UnityEngine.Events;

// Pakottaa ett‰ objektissa on AudioSource kun t‰m‰ komponentti lis‰t‰‰n siihen
[RequireComponent(typeof(AudioSource))]
public class Interaction : MonoBehaviour
{
    public UnityEvent interactEvents;       // Eventit joita halutaan toteuttaa kun interaction tapahtu
    public bool destroyOnInteract = false;   // Tuhotaanko objekti kun interaction on toteutunut
    public AudioClip soundEffect;           // ƒ‰nieffekti joka toistetaan kun interactoidaan

    AudioSource aSource;                    // AudioSource josta ‰‰ni toistetaan

    private void Start()
    {
        aSource = GetComponent<AudioSource>();  // Haetaan AudioSource komponentti
    }

    public void OnInteract()
    {
        // K‰ynnistet‰‰n halutut eventit kun metodia kutsutaan
        interactEvents.Invoke();

        if (soundEffect != null)
            aSource.PlayOneShot(soundEffect);       // Toistetaan ‰‰nieffekti audiosource komponentista

        // Jos DestroyOnInteract bool on true, tuhotaan t‰m‰ objekti
        if (destroyOnInteract)
        {
            this.enabled = false;
        }
    }
}
