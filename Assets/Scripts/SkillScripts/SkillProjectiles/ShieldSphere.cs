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
    // Start is called before the first frame update
    void Start()
    {
		speed = 70;
		projectileNum = 5;
		Batch();
    }

    // Update is called once per frame
    void Update()
    {
		transform.position = GameManager.instance.player.transform.position;
		transform.Rotate(Vector3.back, speed * Time.deltaTime);
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
			skillproj.data = data;
			CircleCollider2D circleCollider = bullet.AddComponent<CircleCollider2D>();
			bullet.GetComponent<Collider2D>().isTrigger = true;
			Animator animator = bullet.AddComponent<Animator>();
			RuntimeAnimatorController controller = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Assets/SkillSprite/S3L2.overrideController");
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
			bullet.transform.Translate(bullet.transform.up * 1.5f, Space.World);
		}
	}
}
