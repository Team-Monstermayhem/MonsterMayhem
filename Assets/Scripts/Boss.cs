using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private float spawnTime;  // 보스 등장 시간
    public float speed;
    public float health;
    public float maxHealth;
    public Rigidbody2D target;

    public float meleeDamage; // 근접 공격 피해
    public float meleeCooldown = 5f; // 근접 공격 쿨타임
    public float meleeConeAngle = 90f; // 원뿔 범위 각도
    public float meleeRange = 4f; // 근접 공격 범위
    private float lastMeleeTime;

    public float aoeDamage; // 원거리 원형 범위 공격 피해
    public float aoeRadius = 3f; // 원거리 원형 범위
    public float aoeCooldown = 8f; // 원거리 원형 공격 쿨타임
    private float lastAoeTime;

    public GameObject aoeMarkerPrefab; // 원형 범위 마커 프리팹
    public GameObject aoeEffectPrefab;  // AOE 파티클 효과 프리팹
    public GameObject coneMarkerPrefab; // 원뿔 범위 마커 프리팹
    public GameObject coneEffectPrefab; // 원뿔 범위 효과 프리팹
    public float warningTime = 1.1f; // 공격 전 경고 시간

    public GameObject projectilePrefab; // 투사체 프리팹
    public float projectileSpeed = 10f; // 투사체 속도
    public float projectileCooldown = 6f; // 투사체 쿨타임
    private float lastProjectileTime;

    // 사방으로 공격하는 패턴 총알
    public float projectile2Speed = 5f; // 총알 속도
    public int projectile2Count = 12; // 발사할 총알 수
    public float projectile2Cooldown = 6f;
    private float lastProjectile2Time;

    public float dashSpeedMultiplier = 3f; // 대시 시 속도 배율
    public float dashDuration = 0.5f; // 대시 지속 시간
    public float dashCooldown = 10f; // 대시 쿨타임
    private float lastDashTime;
    private bool isDashing = false;

    public float collisionDamage = 10f; // 플레이어와 충돌 시 피해량

    bool isLive = true;

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
        spawnTime = Time.time;  // 보스 등장 시점 기록
    }

    void FixedUpdate()
    {
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") || GameManager.instance.isLive != true)
            return;

        if (!isDashing)
        {
            // 플레이어를 따라 이동
            Vector2 dirVec = target.position - rigid.position;
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
            rigid.velocity = Vector2.zero;
        }

        // 공격 패턴 실행
        ExecuteAttackPattern();
    }

    void LateUpdate()
    {
        if (!isLive || GameManager.instance.isLive != true)
            return;
        spriter.flipX = target.position.x < rigid.position.x;
    }

    // 다양한 공격 패턴을 실행
    void ExecuteAttackPattern()
    {
        float elapsedTime = Time.time - spawnTime;  // 보스 등장 후 경과 시간

        // 근접 원뿔 범위 공격
        if (elapsedTime - lastMeleeTime >= meleeCooldown)
        {
            StartCoroutine(PerformMeleeConeAttack());
            lastMeleeTime = elapsedTime;
        }

        // 원거리 원형 범위 공격
        if (elapsedTime - lastAoeTime >= aoeCooldown)
        {
            StartCoroutine(PerformAoeAttack());
            lastAoeTime = elapsedTime;
        }

        // 원거리 투사체 공격
        if (elapsedTime - lastProjectileTime >= projectileCooldown)
        {
            PerformRangedProjectileAttack();
            lastProjectileTime = elapsedTime;
        }
        // 원형 투사체 공격
        if (elapsedTime - lastProjectile2Time >= projectile2Cooldown)
        {
            PerformCircularProjectileAttack();
            lastProjectile2Time = elapsedTime;
        }

        // 고속 이동
        if (elapsedTime - lastDashTime >= dashCooldown && !isDashing)
        {
            StartCoroutine(PerformDashAttack());
            lastDashTime = elapsedTime;
        }
    }
    // 플레이어와의 각도 계산 (원뿔 범위 공격을 위해)
    float GetAngleToPlayer()
    {
        Vector2 direction = (target.position - rigid.position).normalized;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    IEnumerator PerformMeleeConeAttack()
    {
        // 원뿔 마커 표시 (보스의 자식으로 설정)
        GameObject coneMarker = Instantiate(coneMarkerPrefab, transform.position, Quaternion.identity);
        coneMarker.transform.SetParent(transform); // 보스를 부모로 설정해 함께 움직이게 함
        coneMarker.transform.localPosition = Vector3.zero; // 마커가 보스의 위치에 정확히 맞도록

        float timer = 0f; // 경고 시간 타이머
        float attackAngle = 0f;

        // 경고 시간 반 동안 마커의 방향을 지속적으로 플레이어 방향으로 업데이트
        while (timer < warningTime)
        {
            if (timer < warningTime / 2)
            {
                // 플레이어의 현재 위치를 기준으로 마커 회전
                coneMarker.transform.rotation = Quaternion.Euler(0, 0, GetAngleToPlayer());
                attackAngle = GetAngleToPlayer(); // 공격할 최종 각도
            }
            timer += Time.deltaTime; // 경고 시간 업데이트
            yield return null; // 다음 프레임까지 대기
        }

        // 경고 시간이 끝나면 마커 제거
        Destroy(coneMarker);

        // 공격 수행
        float currentAngle = GetAngleToPlayer(); // 공격 시점 플레이어와 보스 각도
        float angleDifference = Mathf.Abs(Mathf.DeltaAngle(attackAngle, currentAngle));

        // 파티클 이펙트 소환 위치 계산 (보스의 위치에서 attackAngle 방향으로 meleeRange의 반만큼 떨어진 위치)
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

    // 원거리 원형 범위 공격 함수
    IEnumerator PerformAoeAttack()
    {
        Vector2 attackPos = target.transform.position;
        // 원형 마커 표시
        GameObject aoeMarker = Instantiate(aoeMarkerPrefab, attackPos, Quaternion.identity);
        //aoeMarker.transform.localScale = new Vector3(aoeRadius, aoeRadius, 1); // 범위에 맞춰 크기 설정
        GameObject aoeEffect = Instantiate(aoeEffectPrefab, attackPos, Quaternion.identity);
        Destroy(aoeEffect, warningTime + 0.1f);

        yield return new WaitForSeconds(warningTime); // 경고 시간 대기

        Destroy(aoeMarker); // 마커 제거


        // 공격 수행
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

    // 원거리 투사체 공격 함수
    void PerformRangedProjectileAttack()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        Vector2 direction = (target.position - rigid.position).normalized;
        projectileRb.velocity = direction * projectileSpeed;

        //anim.SetTrigger("ProjectileAttack");
    }

    // 사방으로 투사체 공격 함수
    void PerformCircularProjectileAttack()
    {
        float angleStep = 360f / projectile2Count; // 각 총알 사이의 각도
        float currentAngle = 0f; // 첫 총알의 각도 시작점

        for (int i = 0; i < projectile2Count; i++)
        {
            // 현재 각도에 따른 방향 계산
            float projectileDirX = Mathf.Cos(currentAngle * Mathf.Deg2Rad);
            float projectileDirY = Mathf.Sin(currentAngle * Mathf.Deg2Rad);
            Vector2 direction = new Vector2(projectileDirX, projectileDirY).normalized;

            // 총알 생성
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            projectileRb.velocity = direction * projectile2Speed; // 총알의 방향과 속도 설정

            currentAngle += angleStep; // 각도 갱신
        }
    }

    // 고속 이동(대시) 함수
    IEnumerator PerformDashAttack()
    {
        isDashing = true; // 대시 시작
        Vector2 dashDirection = (target.position - rigid.position).normalized;
        rigid.velocity = dashDirection * speed * dashSpeedMultiplier;

        //anim.SetTrigger("DashAttack");

        yield return new WaitForSeconds(dashDuration); // 대시 지속 시간만큼 기다림

        rigid.velocity = Vector2.zero; // 대시 후 속도 초기화
        isDashing = false; // 대시 종료
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;
        health -= collision.GetComponent<Bullet>().damage;
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
            //Dead();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player") || !isLive)
            return;
        Player player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            //player.TakeDamage(collisionDamage); // 충돌 시 플레이어에게 피해 입히기
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