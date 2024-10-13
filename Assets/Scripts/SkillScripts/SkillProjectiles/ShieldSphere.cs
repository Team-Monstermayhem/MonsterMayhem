using System.Collections;
using System.Collections.Generic;
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
			bullet.AddComponent<CircleCollider2D>();
			bullet.GetComponent<Collider2D>().isTrigger = true;

			
			bullet.transform.localPosition = Vector3.zero;
			bullet.transform.localRotation = Quaternion.identity;
			//bullet.transform.position = GameManager.instance.player.transform.position;
			Vector3 rotVec = Vector3.forward * 360 * index / projectileNum;
			bullet.transform.Rotate(rotVec);
			bullet.transform.Translate(bullet.transform.up * 1.5f, Space.World);
		}
	}
}
