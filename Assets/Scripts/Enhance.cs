using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enhance : MonoBehaviour
{
    public float maxHealth;
    public float coin = 1000;
    public float attack;
    public float speed;
    public int healthEnhance = 0;
    public int attackEnhance = 0;
    public int speedEnhance = 0;
    public int[] healthEnhanceCoin = { 100, 200, 300, 400, 500 };
    public int[] attackEnhanceCoin = { 100, 200, 300, 400, 500 };
    public int[] speedEnhanceCoin = { 100, 200, 300, 400, 500 };

    public GameObject uI;

    public Player playerInstance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        maxHealth = GameManager.instance.maxhealth;
        speed = playerInstance.speed;
        attack = playerInstance.attack;

        Transform panel = transform.GetChild(0);
        
        Transform coinObject = panel.GetChild(1);
        Transform coinText = coinObject.GetChild(0);
        Text coinTextComponent = coinText.GetComponent<Text>();
        coinTextComponent.text = coin.ToString();

        Transform healthObject = panel.GetChild(2);
        Transform healthText = healthObject.GetChild(0);
        Text healthTextComponent = healthText.GetComponent<Text>();
        healthTextComponent.text = maxHealth.ToString();

        Transform attackObject = panel.GetChild(3);
        Transform attackText = attackObject.GetChild(0);
        Text attackTextComponent = attackText.GetComponent<Text>();
        attackTextComponent.text = attack.ToString();

        Transform speedObject = panel.GetChild(4);
        Transform speedText = speedObject.GetChild(0);
        Text speedTextComponent = speedText.GetComponent<Text>();
        speedTextComponent.text = speed.ToString();

        Transform healthEnhanceObject = panel.GetChild(6);
        Transform healthEnhanceText = healthEnhanceObject.GetChild(0);
        Text healthEnhanceTextComponent = healthEnhanceText.GetComponent<Text>();
        healthEnhanceTextComponent.text = $"{healthEnhance}/ 5";
        Transform healthEnhanceButton = healthEnhanceObject.GetChild(1);
        Transform healthEnhanceCoinText = healthEnhanceButton.GetChild(0);
        Text healthEnhanceCoinTextComponent = healthEnhanceCoinText.GetComponent<Text>();
        healthEnhanceCoinTextComponent.text = $"{healthEnhanceCoin[healthEnhance]} G";

        Transform attackEnhanceObject = panel.GetChild(7);
        Transform attackEnhanceText = attackEnhanceObject.GetChild(0);
        Text attackEnhanceTextComponent = attackEnhanceText.GetComponent<Text>();
        attackEnhanceTextComponent.text = $"{attackEnhance}/ 5";
        Transform attackEnhanceButton = attackEnhanceObject.GetChild(1);
        Transform attackEnhanceCoinText = attackEnhanceButton.GetChild(0);
        Text attackEnhanceCoinTextComponent = attackEnhanceCoinText.GetComponent<Text>();
        attackEnhanceCoinTextComponent.text = $"{attackEnhanceCoin[attackEnhance]} G";

        Transform speedEnhanceObject = panel.GetChild(8);
        Transform speedEnhanceText = speedEnhanceObject.GetChild(0);
        Text speedEnhanceTextComponent = speedEnhanceText.GetComponent<Text>();
        speedEnhanceTextComponent.text = $"{speedEnhance}/ 5";
        Transform speedEnhanceButton = speedEnhanceObject.GetChild(1);
        Transform speedEnhanceCoinText = speedEnhanceButton.GetChild(0);
        Text speedEnhanceCoinTextComponent = speedEnhanceCoinText.GetComponent<Text>();
        speedEnhanceCoinTextComponent.text = $"{speedEnhanceCoin[speedEnhance]} G";
    }

    public void EnhanceHealth()
    {
        if (coin >= healthEnhanceCoin[healthEnhance])
        {
            coin -= healthEnhanceCoin[healthEnhance];
            healthEnhance++;
            maxHealth += 10;

            GameManager.instance.maxhealth = maxHealth;
        }
    }

    public void EnhanceAttack()
    {
        if (coin >= attackEnhanceCoin[attackEnhance])
        {
            coin -= attackEnhanceCoin[attackEnhance];
            attackEnhance++;
            attack += 2;

            playerInstance.attack = attack;
        }
    }

    public void EnhanceSpeed()
    {
        if (coin >= speedEnhanceCoin[speedEnhance])
        {
            coin -= speedEnhanceCoin[speedEnhance];
            speedEnhance++;
            speed += 1;

            playerInstance.speed = speed;
        }
    }

    public void Back()
    {
        uI.SetActive(false);
    }
}