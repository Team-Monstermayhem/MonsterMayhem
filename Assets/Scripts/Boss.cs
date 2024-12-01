using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private float spawnTime;  // ���� ���� �ð�
    public float speed;
    public float health;
    public float maxHealth;
    public Rigidbody2D target;
    public float damageInterval = 0.5f;
    private float damageTimer = 0f;

    public float meleeDamage; // ���� ���� ����
    public float meleeCooldown = 5f; // ���� ���� ��Ÿ��
    public float meleeConeAngle = 90f; // ���� ���� ����
    public float meleeRange = 4f; // ���� ���� ����
    private float lastMeleeTime;

    public float aoeDamage; // ���Ÿ� ���� ���� ���� ����
    public float aoeRadius = 3f; // ���Ÿ� ���� ����
    public float aoeCooldown = 7f; // ���Ÿ� ���� ���� ��Ÿ��
    private float lastAoeTime;

    public GameObject aoeMarkerPrefab; // ���� ���� ��Ŀ ������
    public GameObject aoeEffectPrefab;  // AOE ��ƼŬ ȿ�� ������
    public GameObject coneMarkerPrefab; // ���� ���� ��Ŀ ������
    public GameObject coneEffectPrefab; // ���� ���� ȿ�� ������
    public float warningTime = 1.1f; // ���� �� ��� �ð�

    public GameObject projectilePrefab; // ����ü ������
    public float projectileSpeed = 10f; // ����ü �ӵ�
    public float projectileCooldown = 2f; // ����ü ��Ÿ��
    private float lastProjectileTime;

    // ������� �����ϴ� ���� �Ѿ�
    public float projectile2Speed = 5f; // �Ѿ� �ӵ�
    public int projectile2Count = 12; // �߻��� �Ѿ� ��
    public float projectile2Cooldown = 4f;
    private float lastProjectile2Time;

    public float dashSpeedMultiplier = 3f; // ��� �� �ӵ� ����
    public float dashDuration = 0.5f; // ��� ���� �ð�
    public float dashCooldown = 6f; // ��� ��Ÿ��
    private float lastDashTime;
    private bool isDashing = false;

    public float collisionDamage = 10f; // �÷��̾�� �浹 �� ���ط�

    public bool isLive = true;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    Collider2D coll;
    WaitForFixedUpdate wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        wait = new WaitForFixedUpdate();

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            target = player.GetComponent<Rigidbody2D>();
        }
    }

    void Start()
    {
        spawnTime = Time.time;  // ���� ���� ���� ���
		isLive = true;
        meleeCooldown = 5f;
        aoeCooldown = 7f;
        projectileCooldown = 2f;
        projectile2Cooldown = 4f;
        dashCooldown = 6f;
    }

    void FixedUpdate()
    {
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") || GameManager.instance.isLive != true)
            return;

        if (!isDashing)
        {
            // �÷��̾ ���� �̵�
            Vector2 dirVec = target.position - rigid.position;
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
            rigid.velocity = Vector2.zero;
        }
        if(health <= maxHealth / 2)
        {
            meleeCooldown = 3f;
            aoeCooldown = 4f;
            projectileCooldown = 1f;
            projectile2Cooldown = 2f;
            dashCooldown = 3f;
        }

        // ���� ���� ����
        ExecuteAttackPattern();
    }

    void LateUpdate()
    {
        if (!isLive || GameManager.instance.isLive != true)
            return;
        spriter.flipX = target.position.x < rigid.position.x;
    }

    // �پ��� ���� ������ ����
    void ExecuteAttackPattern()
    {
        float elapsedTime = Time.time - spawnTime;  // ���� ���� �� ��� �ð�

        // ���� ���� ���� ����
        if (elapsedTime - lastMeleeTime >= meleeCooldown)
        {
            StartCoroutine(PerformMeleeConeAttack());
            lastMeleeTime = elapsedTime;
        }

        // ���Ÿ� ���� ���� ����
        if (elapsedTime - lastAoeTime >= aoeCooldown)
        {
            StartCoroutine(PerformAoeAttack());
            lastAoeTime = elapsedTime;
        }

        // ���Ÿ� ����ü ����
        if (elapsedTime - lastProjectileTime >= projectileCooldown)
        {
            PerformRangedProjectileAttack();
            lastProjectileTime = elapsedTime;
        }
        // ���� ����ü ����
        if (elapsedTime - lastProjectile2Time >= projectile2Cooldown)
        {
            PerformCircularProjectileAttack();
            lastProjectile2Time = elapsedTime;
        }

        // ���� �̵�
        if (elapsedTime - lastDashTime >= dashCooldown && !isDashing)
        {
            StartCoroutine(PerformDashAttack());
            lastDashTime = elapsedTime;
        }
    }
    // �÷��̾���� ���� ��� (���� ���� ������ ����)
    float GetAngleToPlayer()
    {
        Vector2 direction = (target.position - rigid.position).normalized;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    IEnumerator PerformMeleeConeAttack()
    {
        // ���� ��Ŀ ǥ�� (������ �ڽ����� ����)
        GameObject coneMarker = Instantiate(coneMarkerPrefab, transform.position, Quaternion.identity);
        coneMarker.transform.SetParent(transform); // ������ �θ�� ������ �Բ� �����̰� ��
        coneMarker.transform.localPosition = Vector3.zero; // ��Ŀ�� ������ ��ġ�� ��Ȯ�� �µ���

        float timer = 0f; // ��� �ð� Ÿ�̸�
        float attackAngle = 0f;

        // ��� �ð� �� ���� ��Ŀ�� ������ ���������� �÷��̾� �������� ������Ʈ
        while (timer < warningTime)
        {
            if (timer < warningTime / 2)
            {
                // �÷��̾��� ���� ��ġ�� �������� ��Ŀ ȸ��
                coneMarker.transform.rotation = Quaternion.Euler(0, 0, GetAngleToPlayer());
                attackAngle = GetAngleToPlayer(); // ������ ���� ����
            }
            timer += Time.deltaTime; // ��� �ð� ������Ʈ
            yield return null; // ���� �����ӱ��� ���
        }

        // ��� �ð��� ������ ��Ŀ ����
        Destroy(coneMarker);

        // ���� ����
        float currentAngle = GetAngleToPlayer(); // ���� ���� �÷��̾�� ���� ����
        float angleDifference = Mathf.Abs(Mathf.DeltaAngle(attackAngle, currentAngle));

        // ��ƼŬ ����Ʈ ��ȯ ��ġ ��� (������ ��ġ���� attackAngle �������� meleeRange�� �ݸ�ŭ ������ ��ġ)
        Vector3 spawnPosition = transform.position + new Vector3(
            Mathf.Cos(attackAngle * Mathf.Deg2Rad) * (meleeRange / 2),
            Mathf.Sin(attackAngle * Mathf.Deg2Rad) * (meleeRange / 2),
            0
        );

        GameObject particleEffect = Instantiate(coneEffectPrefab, spawnPosition, Quaternion.identity);
        particleEffect.transform.SetParent(transform);
        particleEffect.transform.rotation = Quaternion.Euler(0, 0, attackAngle);
        Destroy(particleEffect, 1f);

        if (angleDifference <= meleeConeAngle / 2 && Vector2.Distance(target.position, rigid.position) <= meleeRange)
        {
            Player player = target.GetComponent<Player>();
            if (player != null)
            {
                Debug.Log("MeeleeConeAttackHit");
                GameManager.instance.health -= meleeDamage;
            }
        }
    }

    // ���Ÿ� ���� ���� ���� �Լ�
    IEnumerator PerformAoeAttack()
    {
        Vector2 attackPos = target.transform.position;
        // ���� ��Ŀ ǥ��
        GameObject aoeMarker = Instantiate(aoeMarkerPrefab, attackPos, Quaternion.identity);
        //aoeMarker.transform.localScale = new Vector3(aoeRadius, aoeRadius, 1); // ������ ���� ũ�� ����
        GameObject aoeEffect = Instantiate(aoeEffectPrefab, attackPos, Quaternion.identity);
        Destroy(aoeEffect, warningTime + 0.1f);

        yield return new WaitForSeconds(warningTime); // ��� �ð� ���

        Destroy(aoeMarker); // ��Ŀ ����


        // ���� ����
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPos, aoeRadius);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                Player player = hitCollider.GetComponent<Player>();
                if (player != null)
                {
                    Debug.Log("AoeAttackHit");
                    GameManager.instance.health -= aoeDamage;
                }
            }
        }

    }

    // ���Ÿ� ����ü ���� �Լ�
    void PerformRangedProjectileAttack()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        Vector2 direction = (target.position - rigid.position).normalized;
        projectileRb.velocity = direction * projectileSpeed;

        //anim.SetTrigger("ProjectileAttack");
    }

    // ������� ����ü ���� �Լ�
    void PerformCircularProjectileAttack()
    {
        float angleStep = 360f / projectile2Count; // �� �Ѿ� ������ ����
        float currentAngle = 0f; // ù �Ѿ��� ���� ������

        for (int i = 0; i < projectile2Count; i++)
        {
            // ���� ������ ���� ���� ���
            float projectileDirX = Mathf.Cos(currentAngle * Mathf.Deg2Rad);
            float projectileDirY = Mathf.Sin(currentAngle * Mathf.Deg2Rad);
            Vector2 direction = new Vector2(projectileDirX, projectileDirY).normalized;

            // �Ѿ� ����
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            projectileRb.velocity = direction * projectile2Speed; // �Ѿ��� ����� �ӵ� ����

            currentAngle += angleStep; // ���� ����
        }
    }

    // ���� �̵�(���) �Լ�
    IEnumerator PerformDashAttack()
    {
        isDashing = true; // ��� ����
        Vector2 dashDirection = (target.position - rigid.position).normalized;
        rigid.velocity = dashDirection * speed * dashSpeedMultiplier;

        //anim.SetTrigger("DashAttack");

        yield return new WaitForSeconds(dashDuration); // ��� ���� �ð���ŭ ��ٸ�

        rigid.velocity = Vector2.zero; // ��� �� �ӵ� �ʱ�ȭ
        isDashing = false; // ��� ����
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.CompareTag("Bullet") && isLive)
			health -= collision.GetComponent<Bullet>().damage;
		else if (collision.CompareTag("SkillProjectile") && isLive)
			health -= collision.GetComponent<SkillProjectiles>().curDamage;
		else
			return;
		StartCoroutine(KnockBack());

        if (health > 0)
        {
            anim.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
            Dead();
            if (GameManager.instance.isLive)
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
            GameManager.instance.GameVictory();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!(collision.CompareTag("continuousDamage") && isLive))
            return;

        damageTimer += Time.deltaTime;
        if (damageTimer >= damageInterval)
        {
            if (health > 0)
            {
                StartCoroutine(KnockBack());
                health -= collision.GetComponent<SkillProjectiles>().curDamage;
                damageTimer = 0f;
                anim.SetTrigger("Hit");
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
            }
            else
            {
                isLive = false;
                coll.enabled = false;
                rigid.simulated = false;
                spriter.sortingOrder = 1;
                anim.SetBool("Dead", true);
                GameManager.instance.kill++;
                GameManager.instance.GetExp();
                Dead();
                if (GameManager.instance.isLive)
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
                GameManager.instance.GameVictory();
            }

        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 오브젝트가 벽일 경우
        if (collision.gameObject.CompareTag("continuousDamage") && isLive)
            damageTimer = 0f;
        if (collision.gameObject.CompareTag("Wall") && isLive)
            health -= collision.gameObject.GetComponent<SkillProjectiles>().curDamage;
        else
            return;
        StartCoroutine(KnockBack());

        if (health > 0)
        {
            anim.SetTrigger("Hit");
        }
        else
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
            Dead();
            GameManager.instance.GameVictory();
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait;
        Vector3 playerPos = GameManager.instance.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }

}