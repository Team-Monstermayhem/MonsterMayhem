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
		//Debug.Log("wall pos : " + pos);
		Vector3 direction = (pos - GameManager.instance.player.transform.position).normalized;
		direction.z = 0;
		// ������Ʈ�� �ٶ� ������ ���
		//float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		transform.up = direction;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		// ��� ������Ʈ�� Rigidbody2D�� ������ ���� ���� �ݻ� ���
		Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
		if (rb != null)
		{
			// ���� �浹 ���� ����
			Vector3 collisionNormal = other.transform.position - transform.position;
			collisionNormal.z = 0;
			collisionNormal = collisionNormal.normalized;

			// �浹 ���� ���͸� �������� �ݻ� ���� ���
			Vector3 reflectDirection = Vector3.Reflect(rb.velocity, collisionNormal);
			reflectDirection.z = 0;

			// �ݻ� �������� �ӵ� ����
			rb.velocity = reflectDirection.normalized * 1.2f * rb.velocity.magnitude ; // ���� �ӵ� ����
		}
	}
}
