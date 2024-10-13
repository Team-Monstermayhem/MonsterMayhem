using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : SkillProjectiles
{
	private void Start()
	{
		Vector3 pos = curMouseClickPos;
		pos.z = 0;

		transform.position = pos;
		Vector3 direction = (pos - GameManager.instance.player.transform.position).normalized;
		direction.z = 0;
		// 오브젝트가 바라볼 방향을 계산
		//float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		transform.up = direction;
	}
}
