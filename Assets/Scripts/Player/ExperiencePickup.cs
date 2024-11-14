// Scripts/Player/ExperiencePickup.cs
using UnityEngine;

public class ExperiencePickup : MonoBehaviour
{
    public int experienceValue = 10; // Amount of experience this pickup grants

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Find the PlayerExperience component and add experience
            PlayerExperience playerExperience = other.GetComponent<PlayerExperience>();
            if (playerExperience != null)
            {
                playerExperience.AddExperience(experienceValue);
            }

            // Destroy the pickup after collection
            Destroy(gameObject);
        }
    }
}
