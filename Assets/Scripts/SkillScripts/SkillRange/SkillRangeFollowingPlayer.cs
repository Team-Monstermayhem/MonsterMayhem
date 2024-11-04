using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillRangeFollowingPlayer : MonoBehaviour
{
	public Transform child;
	public Transform player;
    // Start is called before the first frame update
    void Awake()
    {
        child = transform.GetChild(0);
		player = GameManager.instance.player.transform;
		transform.position = player.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		transform.position = player.position;
		
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = 0;
		Vector3 direction = (mousePosition - transform.position).normalized;
		direction.z = 0;
		// 오브젝트가 바라볼 방향을 계산
		//float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		transform.up = direction;
		// Z축을 기준으로 회전 적용
		//transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

		transform.position += transform.position - child.position;
	}
}
