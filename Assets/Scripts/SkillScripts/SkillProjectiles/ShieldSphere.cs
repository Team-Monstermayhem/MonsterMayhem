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
		// delay�� �� DeactivateObject ȣ��
		Invoke("DeactivateObject", delay);
	}

	// ������Ʈ�� ��Ȱ��ȭ�� �� ȣ��Ǿ ���� Invoke�� ���
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
		gameObject.SetActive(false); // ������Ʈ ��Ȱ��ȭ
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
				Debug.LogError("S3L2 �ִϸ��̼� ���� ��ã��");
/*			float objectWidth = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
			float objectHeight = gameObject.GetComponent<SpriteRenderer>().bounds.size.y;

			// ������Ʈ�� ũ�⸦ �������� CircleCollider�� ������ ����
			// width�� height �� �� ���� ���� �������� ������ ����
			float newRadius = Mathf.Min(objectWidth, objectHeight) / 2f;

			// CircleCollider2D ������ ������Ʈ*/
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
