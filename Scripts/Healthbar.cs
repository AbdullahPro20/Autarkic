using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class Healthbar : MonoBehaviour {
    public GameObject LEVELFAILED;  
    
    private Slider healthbarDisplay;

    [Header("Main Variables:")]
    
    [Tooltip("Health variable: (default range: 0-100)")] public float health = 100;

    
    private int healthPercentage = 100;

    
    [Tooltip("Minimum possible heath: (default is 0)")] public float minimumHealth = 0;


    [Tooltip("Maximum possible heath: (default is 100)")] public float maximumHealth = 100;


    [Tooltip("Low health is less than or equal to this:")] public int lowHealth = 33;

   
    [Tooltip("High health is greater than or equal to this:")] public int highHealth = 66;

    [Space]

    [Header("Regeneration:")]    
   
    public bool regenerateHealth;
    public float healthPerSecond;

    [Space]

    [Header("Healthbar Colors:")]
    public Color highHealthColor = new Color(0.35f, 1f, 0.35f);
    public Color mediumHealthColor = new Color(0.9450285f, 1f, 0.4481132f);
    public Color lowHealthColor = new Color(1f, 0.259434f, 0.259434f);

    private void Start()
    {
        
        if (healthbarDisplay == null)
        {
            healthbarDisplay = GetComponent<Slider>();
        }

       
        healthbarDisplay.minValue = minimumHealth;
        healthbarDisplay.maxValue = maximumHealth;


        UpdateHealth();
    }

   
    private void Update()
    {
        healthPercentage = int.Parse((Mathf.Round(maximumHealth * (health / 100f))).ToString());

        
        if (health < minimumHealth)
        {
            health = minimumHealth;
        }

        
        if (health > maximumHealth)
        {
            health = maximumHealth;
        }

        
        if (health < maximumHealth && regenerateHealth)
        {
            health += healthPerSecond * Time.deltaTime;

            
            UpdateHealth();
        }
    }

    
    public void UpdateHealth()
    {

        if (healthPercentage <= lowHealth && health >= minimumHealth && transform.Find("Bar").GetComponent<Image>().color != lowHealthColor)
        {
            ChangeHealthbarColor(lowHealthColor);
        }
        else if (healthPercentage <= highHealth && health > lowHealth)
        {
            float lerpedColorValue = (float.Parse(healthPercentage.ToString()) - 25) / 41;
            ChangeHealthbarColor(Color.Lerp(lowHealthColor, mediumHealthColor, lerpedColorValue));
        }
        else if (healthPercentage > highHealth && health <= maximumHealth)
        {
            float lerpedColorValue = (float.Parse(healthPercentage.ToString()) - 67) / 33;
            ChangeHealthbarColor(Color.Lerp(mediumHealthColor, highHealthColor, lerpedColorValue));
        }

        healthbarDisplay.value = health;
    }

    public void GainHealth(float amount)
    {
        
        health += amount;
        UpdateHealth();
    }

    public void TakeDamage(float amount)
    {
        
        health -= float.Parse(amount.ToString());
        UpdateHealth();
        if (health <= 0) {
            LEVELFAILED.SetActive(true);
        
        }
    }

    public void ChangeHealthbarColor(Color colorToChangeTo)
    {
        transform.Find("Bar").GetComponent<Image>().color = colorToChangeTo;
    }

    public void ToggleRegeneration()
    {
        regenerateHealth = !regenerateHealth;
    }

    public void SetHealth(float value)
    {
        health = value;
        UpdateHealth();
    }
}