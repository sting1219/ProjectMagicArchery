using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [Header("References")]
    public GameObject projectilePrefab;
    public GameObject damageTextPrefab; // 인스펙터에서 등록할 데미지 텍스트 슬롯 추가
    public Transform launchPoint;
    public Transform targetPoint; // 현재 조준 중인 과녁

    [Header("Combat Stats")]
    public float attackCooldown = 0.5f; // 스크린샷처럼 쏟아지도록 기본 연사속업 업그레이드!
    private float cooldownTimer = 0f;

    private void Update()
    {
        // GameManager의 실시간 공속 수치를 반영
        attackCooldown = GameManager.Instance.GetCurrentCooldown();

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
        if (projectilePrefab == null || launchPoint == null || targetPoint == null) return;

        GameObject projGo = Instantiate(projectilePrefab, launchPoint.position, Quaternion.identity);
        Projectile proj = projGo.GetComponent<Projectile>();

        if (proj != null)
        {
            // 시작점과 목표점을 던져줍니다.
            proj.Setup(launchPoint.position, targetPoint.position, damageTextPrefab);
        }
    }
}