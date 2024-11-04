
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAttack : SkillProjectiles
{
	public float delay = 3f;

	private void OnEnable()
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pos.y += 3.0f;
		pos.z = 0;
		transform.position = pos;
		// delay초 후 DeactivateObject 호출
		Debug.Log("시작하자마자?");
		delay = 3f;
		Invoke("DeactivateObject", delay);
	}

	// 오브젝트가 비활성화될 때 호출되어서 이전 Invoke를 취소
	private void OnDisable()
	{
		Debug.Log("OnDisable 호출됨. Stack Trace:\n" + System.Environment.StackTrace);
		CancelInvoke("DeactivateObject");
	}


	void DeactivateObject()
	{
		Debug.Log("Deactivate 함수 호출됨. Stack Trace:\n" + System.Environment.StackTrace);
		gameObject.SetActive(false);
	}
}
