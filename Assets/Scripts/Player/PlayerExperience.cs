// Scripts/Player/PlayerExperience.cs
using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    public int currentLevel = 1;
    public int currentExperience = 0;
    public int experienceToNextLevel = 100;

    public PlayerStats playerStats;

    private PlayerHealth playerHealth;
    private PlayerController playerController;
    private WeaponController weaponController;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerController = GetComponent<PlayerController>();
        weaponController = GetComponent<WeaponController>();
    }

    public void AddExperience(int amount)
    {
        currentExperience += amount;
        Debug.Log("Experience: " + currentExperience);

        if (currentExperience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        currentExperience -= experienceToNextLevel;
        experienceToNextLevel = Mathf.RoundToInt(experienceToNextLevel * 1.5f);

        ApplyRandomUpgrade();
    }

    private void ApplyRandomUpgrade()
    {
        int upgradeChoice = Random.Range(0, 3); // 0 = Health, 1 = Speed, 2 = Damage

        switch (upgradeChoice)
        {
            case 0:
                if (playerHealth != null)
                {
                    playerHealth.ApplyHealthUpgrade(20f); // Increase only instance-based health
                    Debug.Log("Level Up! +20 Health.");
                }
                break;
            case 1:
                if (playerController != null)
                {
                    playerController.ApplySpeedUpgrade(1.1f); // Increase only instance-based speed
                    Debug.Log("Level Up! +10% Speed.");
                }
                break;
            case 2:
                if (weaponController != null)
                {
                    weaponController.ApplyDamageUpgrade(1.1f); // Increase only instance-based damage
                    Debug.Log("Level Up! +10% Weapon Damage.");
                }
                break;
        }
    }
}
