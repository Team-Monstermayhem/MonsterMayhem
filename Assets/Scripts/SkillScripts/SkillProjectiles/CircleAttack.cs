
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAttack : SkillProjectiles
{
	private void Start()
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pos.z = 0;

		transform.position = pos;
		StartCoroutine(DeactivateAfterTime(3f));
	}

	IEnumerator DeactivateAfterTime(float delay)
	{
		// 5초 기다리기
		yield return new WaitForSeconds(delay);

		// 오브젝트 비활성화
		transform.gameObject.SetActive(false);
	}
}
