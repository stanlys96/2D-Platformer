using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
  public static PlayerHealthController instance;

  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  public int currentHealth;
  public int maxHealth;

  public float invincibilityLength;
  private float invincibilityCounter;

  public float flashLength;
  private float flashCounter;

  public SpriteRenderer[] playerSprites;

  // Start is called before the first frame update
  void Start()
  {
    currentHealth = maxHealth;

    
  }

  // Update is called once per frame
  void Update()
  {
    if (invincibilityCounter > 0)
    {
      invincibilityCounter -= Time.deltaTime;

      flashCounter -= Time.deltaTime;
      if (flashCounter <= 0)
      {
        foreach (SpriteRenderer spriteRenderer in playerSprites)
        {
          spriteRenderer.enabled = !spriteRenderer.enabled;
        }
        flashCounter = flashLength;
      }

      if (invincibilityCounter <= 0)
      {
        foreach (SpriteRenderer spriteRenderer in playerSprites)
        {
          spriteRenderer.enabled = true;
        }
        flashCounter = 0f;
      }
    }
  }

  public void DamagePlayer(int damageAmount)
  {
    if (invincibilityCounter <= 0)
    {
      currentHealth -= damageAmount;
      if (currentHealth <= 0)
      {
        currentHealth = 0;

        // gameObject.SetActive(false);
        RespawnController.instance.Respawn();

        AudioManager.instance.PlaySFX(8);
      }
      else
      {
        invincibilityCounter = invincibilityLength;

        AudioManager.instance.PlaySFXAdjusted(11);
      }

      UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }
  }

  public void RestoreHealth()
  {
    currentHealth = maxHealth;
    UIController.instance.UpdateHealth(currentHealth, maxHealth);
  }

  public void HealPlayer(int healAmount)
  {
    currentHealth += healAmount;

    if (currentHealth > maxHealth)
    {
      currentHealth = maxHealth;
    }

    UIController.instance.UpdateHealth(currentHealth, maxHealth);
  }
}
