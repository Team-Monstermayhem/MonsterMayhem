using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
/*using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class SkillBtn : MonoBehaviour
{
	public SkillData data;
	public int level;
	public Image skillIcon;
	Text textLevel;

	private void Awake()
	{
		level = 0;
		skillIcon = GetComponentsInChildren<Image>()[1];
		skillIcon.sprite = data.skillIcon;

		Text[] texts = GetComponentsInChildren<Text>();
		textLevel = texts[0];
	}

	private void LateUpdate()
	{
		textLevel.text = "Lv." + (level);
	}

	public void OnClick()
	{
		Skill skill = this.AddComponent<Skill>();
		skill.Init(data, level);
		level++;

		if (level >= data.maxLevel)
		{
			level = data.maxLevel;
		}
	}

}*/
