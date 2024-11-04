
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
		// delay�� �� DeactivateObject ȣ��
		Debug.Log("�������ڸ���?");
		delay = 3f;
		Invoke("DeactivateObject", delay);
	}

	// ������Ʈ�� ��Ȱ��ȭ�� �� ȣ��Ǿ ���� Invoke�� ���
	private void OnDisable()
	{
		Debug.Log("OnDisable ȣ���. Stack Trace:\n" + System.Environment.StackTrace);
		CancelInvoke("DeactivateObject");
	}


	void DeactivateObject()
	{
		Debug.Log("Deactivate �Լ� ȣ���. Stack Trace:\n" + System.Environment.StackTrace);
		gameObject.SetActive(false);
	}
}
