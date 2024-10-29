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
	public virtual void Init(int level, Vector3 mouseClickPos, SkillData skillData)
	{
		data = skillData;
		curlevel = level;
		curDamage = data.baseDamage + data.damages[curlevel];
		curPer = data.baseCount + data.counts[curlevel];
		curSpeed = 5f;
		curCoolTime = data.cooldowns[curlevel];
		curMouseClickPos = mouseClickPos;
	}
}

