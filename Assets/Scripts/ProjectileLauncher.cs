using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [Header("References")]
    public GameObject projectilePrefab;
    public GameObject damageTextPrefab;
    public Transform launchPoint;
    public Transform targetPoint; // 현재 조준 중인 과녁

    [Header("Combat Stats")]
    public float attackCooldown = 0.5f;
    private float cooldownTimer = 0f;

    private void Update()
    {
        // GameManager의 실시간 공속 수치를 반영
        attackCooldown = 1;

        // [핵심 수정] 타겟이 아예 없거나, '맵에서 비활성화(숨김) 상태'라면 과녁 참조를 null로 밀어버립니다.
        if (targetPoint == null || !targetPoint.gameObject.activeInHierarchy)
        {
            targetPoint = null; // 꺼진 과녁 주소를 확실하게 비웁니다.
            FindNextTarget();   // 새로 켜진 과녁이 있는지 탐색합니다.
        }

        // 이제 진짜로 맵에 살아있고 활성화된 타겟이 존재할 때만 발사합니다.
        if (targetPoint != null)
        {
            cooldownTimer += Time.deltaTime;

            if (cooldownTimer >= attackCooldown)
            {
                Launch();
                cooldownTimer = 0f;
            }
        }
    }

    private void Launch()
    {
        // 여기서도 안전망으로 한 번 더 활성화 상태를 체크해줍니다.
        if (projectilePrefab == null || launchPoint == null || targetPoint == null || !targetPoint.gameObject.activeInHierarchy) return;

        GameObject projGo = Instantiate(projectilePrefab, launchPoint.position, Quaternion.identity);
        Projectile proj = projGo.GetComponent<Projectile>();

        if (proj != null)
        {
            proj.Setup(launchPoint.position, targetPoint.position, damageTextPrefab);
        }
    }

    // 씬에 남아있는 다음 타겟을 자동으로 서칭하는 메서드
    private void FindNextTarget()
    {
        // FindAnyObjectByType은 활성화(Active)된 오브젝트만 자동으로 찾아옵니다.
        Target nextTarget = FindAnyObjectByType<Target>();

        if (nextTarget != null)
        {
            targetPoint = nextTarget.transform;
        }
    }
}