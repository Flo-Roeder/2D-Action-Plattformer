using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public TMP_Text healthText;
    public Slider healthSlider;
    Damageable damageable;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player==null)
        {
            Debug.Log("no player with tag found");
        }
        damageable = player.GetComponent<Damageable>();
    }

    // Start is called before the first frame update
    void Start()
    {
        healthSlider.value = CalculateSlider(damageable.Health,damageable.MaxHealth);
        healthText.text = "HP: " + damageable.Health + " / " + damageable.MaxHealth;
    }

    private void OnEnable()
    {
        damageable.healthChanged.AddListener(OnPlayerHealthChanged);
    }

    private void OnDisable()
    {
        damageable.healthChanged.RemoveListener(OnPlayerHealthChanged);

    }

    private float CalculateSlider(float currentHealth, float maxHealth)
    {
        return currentHealth/maxHealth;
    }

    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        healthSlider.value = CalculateSlider(newHealth, maxHealth);
        healthText.text = "HP: " + newHealth + " / " + maxHealth;
    }
}
