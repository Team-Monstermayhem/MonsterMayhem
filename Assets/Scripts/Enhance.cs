using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enhance : MonoBehaviour
{
    public float maxHealth;
    public float coin = 1000;
    public float attack = 10;
    public float speed;
    public GameObject uI;

    public Player playerInstance;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = GameManager.instance.maxhealth;
        speed = playerInstance.speed;
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    public void Back()
    {
        uI.SetActive(false);
    }
}
