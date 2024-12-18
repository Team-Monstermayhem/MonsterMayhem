using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEditor;

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
	int skillNum;
	float[] coolValue = new float[] { 1.0f, 1.1f, 1.3f, 1.6f, 2.0f };
	float baseCoolTime;
	float curCoolTime = 1.0f;
	private void Start()
	{

		poolManager = GameManager.instance.poolManager;
		coolImage = transform.GetChild(0).GetComponent<Image>();
		//poolManager = GameManager.instance.poolManager;
		skillIcon = GetComponentsInChildren<Image>()[1];
        skillData = poolManager.skillDatas[GameManager.instance.selectSKillType];
		baseCoolTime = skillData.cooldowns[skillNum];

		Text[] texts = GetComponentsInChildren<Text>();
		textLevel = texts[0];
		char c = gameObject.name[gameObject.name.Length - 1]; // "item " 이후의 문자열 추출
		//Debug.Log(" c : " + c);
		skillNum = (int)(c - '0');
		//Debug.Log("gameObjectName : " + gameObject.name + "skillNum : " + skillNum);
		level = 0;

	}

	private void Update()
	{
		if (level == 0)
		{
			coolImage.fillAmount = 0;
			return;
		}
		else
		{
			coolImage.fillAmount = 1;
			if (isOnCooldown)
			{
				cooldownTimer += Time.deltaTime;
				curCoolTime = baseCoolTime / coolValue[level - 1];
				//Debug.Log("Cooldown Timer : " + cooldownTimer + "Cooldown Target : " + skillData.cooldowns[level]);
				if (cooldownTimer >= curCoolTime)
				{
					isOnCooldown = false;
					coolImage.fillAmount = 1;
				}
				else
				{
					//Debug.Log("Num : " + skillNum);
					coolImage.fillAmount = cooldownTimer / curCoolTime;
					//Debug.Log("fill amount : " + coolImage.fillAmount);
				}
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
		//Debug.Log(skillprojectile + "is NULL?");
		skillprojectile.GetComponent<SkillProjectiles>().Init(level, mouseClickPos, skillData, selectedSkillIndex);

		Debug.Log("usesskill : " + skillNum);
		StartCoroutine(SkillCooldown(skillData.cooldowns[skillNum])); // 레벨에 따라 쿨타임 설정
	}

	public GameObject ShowSkillRange(int selectedSkillIndex)
	{
		//Debug.Log("Skill Rangef??" + "SkillData : " + skillData.skillType + "Level : " + level);
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
