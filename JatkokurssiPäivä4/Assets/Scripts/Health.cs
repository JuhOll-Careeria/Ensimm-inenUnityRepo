using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public AudioClip takeDamageSE;  // audio clip joka toistetaan aina kun objekti ottaa damagea
    public AudioClip onDeathSE;     // audio clip joka toisteaan kun objekti kuolee/tuhoutuu
    public UnityEvent onDeathEvents;

    private int currentHealth = 0;
    private bool isDead = false;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
    }

    // T‰t‰ kutsutaan kun objekti ottaa osumaa
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        // V‰hennet‰‰n currentHealth m‰‰r‰‰ amountilla
        currentHealth -= amount;

        // Jos AudioSource ja takeDamagaSE Ei ole tyhj‰/referoitu
        if (audioSource != null && takeDamageSE != null)
        {
            // Toistetaan "osuma" ‰‰nieffekti
            audioSource.PlayOneShot(takeDamageSE);
        }

        // UUSI KOMMENTTI GITTIƒ VARTEN


        // Jos currentHealth on alle 0, objekti tuhoutuu/kuolee
        if (currentHealth <= 0)
        {
            isDead = true;
            OnDeath();
        }
    }

    public void OnDeath()
    {
        // Jos AudioSource ja onDeathSE Ei ole tyhj‰/referoitu
        if (audioSource != null && onDeathSE != null)
        {
            // Toistetaan kun objekti tuhoutuu/kuolee
            audioSource.PlayOneShot(onDeathSE);
        }

        onDeathEvents.Invoke();
    }
}
