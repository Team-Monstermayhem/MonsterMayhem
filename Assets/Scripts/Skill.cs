using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Skill : MonoBehaviour
{
	public SkillData skillData; // 스킬 데이터
	public Image coolImage;
	public int level; // 현재 레벨
	public bool isOnCooldown; // 쿨타임 상태
	float cooldownTimer = 0f;

	public PoolManager poolManager;
	public Image skillIcon;
	Text textLevel;


	private void Start()
	{

		poolManager = GameManager.instance.poolManager;
		coolImage = transform.GetChild(0).GetComponent<Image>();
		//poolManager = GameManager.instance.poolManager;
		skillIcon = GetComponentsInChildren<Image>()[1];
		skillData = poolManager.skillDatas[GameManager.instance.selectSKillType];

		Text[] texts = GetComponentsInChildren<Text>();
		textLevel = texts[0];

		level = 1;

	}

	private void Update()
	{
		if (isOnCooldown && level != 0)
		{
			cooldownTimer += Time.deltaTime;
			//Debug.Log("Cooldown Timer : " + cooldownTimer + "Cooldown Target : " + skillData.cooldowns[level]);
			if (cooldownTimer >= skillData.cooldowns[level-1])
			{
				isOnCooldown = false;
				coolImage.fillAmount = 1;
			}
			else
			{
				coolImage.fillAmount = cooldownTimer / skillData.cooldowns[level-1];
			}
		}
	}

	private void LateUpdate()
	{
		textLevel.text = "Lv." + (level);
	}

	public void UseSkill(Vector3 mouseClickPos, int selectedSkillIndex)
	{
		if (isOnCooldown)
		{
			Debug.Log($"{level} level {skillData.skillName} is still on cooldown.");
			return;
		}
		GameObject skillprojectile = poolManager.GetObject(28 + (int)skillData.skillType * 4 + selectedSkillIndex); //0 : enemy 1~25 : skill range 26~50 : skill projectile
		Debug.Log(skillprojectile + "is NULL?");
		skillprojectile.GetComponent<SkillProjectiles>().Init(level, mouseClickPos, skillData);


		StartCoroutine(SkillCooldown(skillData.cooldowns[level])); // 레벨에 따라 쿨타임 설정
	}

	public GameObject ShowSkillRange(int selectedSkillIndex)
	{
		Debug.Log("Skill Rangef??" + "SkillData : " + skillData.skillType + "Level : " + level);
		GameObject skillRangeInstance = poolManager.GetObject(12 + (int)skillData.skillType * 4 + selectedSkillIndex);
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
