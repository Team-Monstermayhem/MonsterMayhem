using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShieldSphere : SkillProjectiles
{
	int projectileNum;
	int speed;
	SpriteRenderer spriteRenderer;
	public Sprite circleSprite;
	GameObject bullet;

	public float delay;
    // Start is called before the first frame update

	private void OnEnable()
	{
		delay = 5f;
		speed = 70;
		projectileNum = 5;
		delay = 3f;
		projectileNum = curlevel * 5;
		Batch();
		// delay초 후 DeactivateObject 호출
		Invoke("DeactivateObject", delay);
	}

	// 오브젝트가 비활성화될 때 호출되어서 이전 Invoke를 취소
	private void OnDisable()
	{
		CancelInvoke("DeactivateObject");
	}

	// Update is called once per frame
	void Update()
    {
		transform.position = GameManager.instance.player.transform.position;
		transform.Rotate(Vector3.back, speed * Time.deltaTime);
	}

	private void DeactivateObject()
	{
		gameObject.SetActive(false); // 오브젝트 비활성화
	}

	void Batch()
	{
		for (int index = 0; index < projectileNum; index++)
		{
			bullet = new GameObject();
			bullet.transform.SetParent(transform);
			spriteRenderer = bullet.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = circleSprite;

			SkillProjectiles skillproj = bullet.AddComponent<SkillProjectiles>();
			skillproj.curDamage = curDamage;
			CircleCollider2D circleCollider = bullet.AddComponent<CircleCollider2D>();
			bullet.GetComponent<Collider2D>().isTrigger = true;
			Animator animator = bullet.AddComponent<Animator>();
			RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>("S3L2");
			if (controller != null)
				animator.runtimeAnimatorController = controller;
			else
				Debug.LogError("S3L2 애니메이션 파일 못찾음");
/*			float objectWidth = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
			float objectHeight = gameObject.GetComponent<SpriteRenderer>().bounds.size.y;

			// 오브젝트의 크기를 기준으로 CircleCollider의 반지름 설정
			// width와 height 중 더 작은 값을 기준으로 반지름 설정
			float newRadius = Mathf.Min(objectWidth, objectHeight) / 2f;

			// CircleCollider2D 반지름 업데이트*/
			circleCollider.radius = 1.3f;
			bullet.transform.localScale = new Vector3(1f, 1f, 0f);

			bullet.tag = "SkillProjectile";
			
			bullet.transform.localPosition = Vector3.zero;
			bullet.transform.localRotation = Quaternion.identity;
			//bullet.transform.position = GameManager.instance.player.transform.position;
			Vector3 rotVec = Vector3.forward * 360 * index / projectileNum;
			bullet.transform.Rotate(rotVec);
			bullet.transform.Translate(bullet.transform.up * (curlevel + 1), Space.World);
		}
	}
}
