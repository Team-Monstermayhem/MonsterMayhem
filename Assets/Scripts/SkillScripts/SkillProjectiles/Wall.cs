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
		// 오브젝트가 바라볼 방향을 계산
		//float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		transform.up = direction;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		// 상대 오브젝트가 Rigidbody2D를 가지고 있을 때만 반사 계산
		Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
		if (rb != null)
		{
			// 현재 충돌 법선 벡터
			Vector3 collisionNormal = other.transform.position - transform.position;
			collisionNormal.z = 0;
			collisionNormal = collisionNormal.normalized;

			// 충돌 법선 벡터를 기준으로 반사 벡터 계산
			Vector3 reflectDirection = Vector3.Reflect(rb.velocity, collisionNormal);
			reflectDirection.z = 0;

			// 반사 방향으로 속도 설정
			rb.velocity = reflectDirection.normalized * 1.2f * rb.velocity.magnitude ; // 기존 속도 유지
		}
	}
}
