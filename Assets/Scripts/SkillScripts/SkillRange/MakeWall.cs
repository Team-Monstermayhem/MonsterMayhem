using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeWall : MonoBehaviour
{
	public Transform player;
	void Awake()
	{
		player = GameManager.instance.player.transform;
		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pos.z = 0;
		transform.position = pos;

		Vector3 direction = (pos - player.position).normalized;
		direction.z = 0;
		// 오브젝트가 바라볼 방향을 계산
		//float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		transform.up = direction;

	}
}
