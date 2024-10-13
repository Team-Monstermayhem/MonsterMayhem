using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;


public class Skill : MonoBehaviour
{
	public SkillData skillData; // 스킬 데이터
	public Button skillButton;
	public Image coolImage;
	public int level; // 현재 레벨
	private bool isOnCooldown; // 쿨타임 상태
	float cooldownTimer = 0f;

	public PoolManager poolManager;

	public Image skillIcon;
	Text textLevel;


	private void Awake()
	{
		level = 0;
		skillIcon = GetComponentsInChildren<Image>()[1];
		skillIcon.sprite = skillData.skillIcon;

		Text[] texts = GetComponentsInChildren<Text>();
		textLevel = texts[0];

		
	}



	private void Start()
	{
		poolManager = GameManager.instance.poolManager;
		skillButton = GetComponent<Button>();
		coolImage = transform.GetChild(1).GetComponent<Image>();
	}

	private void Update()
	{
		if (isOnCooldown)
		{
			cooldownTimer += Time.deltaTime;
			//Debug.Log("Cooldown Timer : " + cooldownTimer + "Cooldown Target : " + skillData.cooldowns[level]);
			if (cooldownTimer >= skillData.cooldowns[level])
			{
				isOnCooldown = false;
				coolImage.fillAmount = 1;
				skillButton.interactable = true;
			}
			else
			{
				coolImage.fillAmount = cooldownTimer / skillData.cooldowns[level];
			}
		}
	}

	private void LateUpdate()
	{
		textLevel.text = "Lv." + (level);
	}

	public void UseSkill(Vector3 mouseClickPos)
	{
		if (isOnCooldown)
		{
			Debug.Log($"{level} level {skillData.skillName} is still on cooldown.");
			return;
		}
		GameObject skillprojectile = poolManager.GetObject(25 + (int)skillData.skillType * 5 + level); //0 : enemy 1~25 : skill range 26~50 : skill projectile
		skillprojectile.GetComponent<SkillProjectiles>().Init(level, mouseClickPos, skillData);


		StartCoroutine(SkillCooldown(skillData.cooldowns[level - 1])); // 레벨에 따라 쿨타임 설정
	}

	public GameObject ShowSkillRange()
	{
		GameObject skillRangeInstance = poolManager.GetObject((int)skillData.skillType * 5 + level);
		if (skillRangeInstance.GetComponent<Collider2D>() == null)
		{
			skillRangeInstance.AddComponent<Collider2D>();	
		}
		skillRangeInstance.GetComponent<Collider2D>().isTrigger = true;

		return skillRangeInstance;
	}

	private IEnumerator SkillCooldown(float cooldown)
	{
		//Debug.Log("Skill Cool downing!!");
		isOnCooldown = true;
		cooldownTimer = 0f;
		coolImage.fillAmount = 0;
		skillButton.interactable = false;
		yield return new WaitForSeconds(cooldown);
		isOnCooldown = false;
		yield return null;
	}

	public void LevelUp()
	{
		level++;
		if (level >= skillData.maxLevel)
			level = skillData.maxLevel;
	}
}
