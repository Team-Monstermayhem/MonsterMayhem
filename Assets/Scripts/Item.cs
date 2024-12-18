using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Gear gear;
    public GameObject itemUI;
	public Player player;

    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    void Awake()
    {
		
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;
		Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.itemName;
    }

    void OnEnable()
    {
        textLevel.text = "Lv." + (level + 1);

        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            default:
                textDesc.text = string.Format(data.itemDesc);
                break;
        }
    }

    public void OnClick()
    {
        switch(data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    weapon.LevelUp(nextDamage, nextCount);
                }
                level++;
                SetUI(level);
                break;
            case ItemData.ItemType.Glove:
				Scanner scanner = GameManager.instance.player.GetComponent<Scanner>();
				scanner.attackInterval /= 1.5f;  // 공격 주기
				scanner.projectileSpeed += 0.5f;  // 발사체 속도
				player = GameManager.instance.player.GetComponent<Player>();
				player.attack *= 1.3f;
				break;
            case ItemData.ItemType.Shoe:
                if(level == 0)
                {
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                }
                else
                {
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }
                level++;
                SetUI(level);
                break;
            case ItemData.ItemType.Heal:
                GameManager.instance.health = GameManager.instance.maxhealth;
                break;
        }

        if (level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }

    public void SetUI(int lv)
    {
        Transform childTransform = itemUI.transform.GetChild(1);
        Text childText = childTransform.GetComponent<Text>();

        childText.text = "Lv. " + lv;
    }
}
