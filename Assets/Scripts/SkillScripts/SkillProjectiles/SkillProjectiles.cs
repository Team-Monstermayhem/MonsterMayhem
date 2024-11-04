using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillProjectiles : MonoBehaviour
{
	public SkillData data;
	public int curlevel;
	public float curDamage;
	public int curPer;
	public float curSpeed;
	public float curCoolTime;
	public Vector3 curMouseClickPos;
	public virtual void Init(int level, Vector3 mouseClickPos, SkillData skillData, int selectedSkillIndex)
	{
		data = skillData;
		curlevel = level;
		if (curlevel >= 5)
			curlevel = 4;
		curDamage = data.baseDamage + data.damages[selectedSkillIndex] * curlevel;
		curPer = data.baseCount + data.counts[selectedSkillIndex] * curlevel;
		curSpeed = 5f;
		curCoolTime = data.cooldowns[selectedSkillIndex] / curlevel;
		curMouseClickPos = mouseClickPos;
	}
}

