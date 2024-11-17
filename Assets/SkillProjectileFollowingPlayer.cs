using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillProjectileFollowingPlayer : SkillProjectiles
{
	public Scanner scanner;
	Transform child;
	Transform nearestTarget;
	Transform player;
	public float dieTime;
	public Vector3 direction;

	void Awake()
	{
		player = GameManager.instance.player.transform;
		transform.position = player.position;
		scanner = player.GetComponent<Scanner>();
	}

	private void OnEnable()
	{
		dieTime = 3f;
		Invoke("DeActive", dieTime);
		transform.position = player.position;
		child = transform.GetChild(0);
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		transform.position = player.position;
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = 0;
		if (scanner.nearestTarget != null)
		{
			Vector3 direction = (scanner.nearestTarget.position - transform.position).normalized;
			direction.z = 0;
			// 오브젝트가 바라볼 방향을 계산
			//float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

			transform.up = direction;
		}
		transform.Rotate(0, 0, 90);
		// Z축을 기준으로 회전 적용
		//transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

		transform.position += transform.position - child.position;
	}
	void DeActive()
	{
		gameObject.SetActive(false);
	}
}
