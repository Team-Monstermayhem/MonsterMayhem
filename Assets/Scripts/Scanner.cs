using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Scanner : MonoBehaviour
{
	public float scanRange;
	public LayerMask targetLayer;
	public RaycastHit2D[] targets;
	public Transform nearestTarget;

    public GameObject projectilePrefab;  // �߻�ü ������
    public float attackInterval = 0.4f;  // ���� �ֱ�
    public float projectileSpeed = 10f;  // �߻�ü �ӵ�

    private void Start()
    {
        InvokeRepeating("AutoAttack", 3f, attackInterval);
    }

    private void FixedUpdate()
	{
		targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
		nearestTarget = GetNearest();
	}

	Transform GetNearest()
	{
		Transform result = null;
		float diff = 100;

		foreach (RaycastHit2D target in targets)
		{
			Vector3 myPos = transform.position;
			Vector3 targetPos = target.transform.position;
			float curDiff = Vector3.Distance(myPos, targetPos);

			if (curDiff < diff)
			{
				diff = curDiff;
				result = target.transform;
			}
		}
		return result;
	}

    void AutoAttack()
    {
        if (nearestTarget != null)
        {
			GameObject projectile = GameManager.instance.poolManager.GetObject(44);
			projectile.transform.position = GameManager.instance.player.transform.position;
           // GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

            Vector2 direction = (nearestTarget.position - transform.position).normalized;
            projectileRb.velocity = direction * projectileSpeed;
        }
    }
}
