using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 targetPos;
    private Vector3 controlPos; // 곡선의 정점이 될 중간 제어점

    private float speed = 2.0f;  // 총 비행 시간 관련 속도 (작을수록 느림)
    private float progress = 0f; // 0(시작) ~ 1(도착)까지의 진행도
    private bool isInitialized = false;

    private GameObject damageTextPrefab;
    private float damage = 10f; // 기본 데미지 설정

    public void Setup(Vector3 start, Vector3 target, GameObject textPrefab)
    {
        startPos = start;
        targetPos = target;
        damageTextPrefab = textPrefab; // 프리팹 전달받기

        // 시작점과 목표점의 중간 위치를 구한 뒤, 위쪽(Y)으로 무작위 높이를 더해 포물선 생성
        Vector3 midPoint = (startPos + targetPos) / 2f;
        float arcHeight = Random.Range(2.0f, 4.0f); // 화살마다 높낮이를 다르게 주어 도파민 생성
        controlPos = midPoint + Vector3.up * arcHeight;

        progress = 0f;
        isInitialized = true;
    }

    private void Update()
    {
        if (!isInitialized) return;

        // 시간에 따라 진행도 증가
        progress += Time.deltaTime * speed;

        if (progress >= 1.0f)
        {
            progress = 1.0f;
            EvaluatePosition();
            OnTargetHit();
        }
        else
        {
            EvaluatePosition();
        }
    }

    private void EvaluatePosition()
    {
        // 2차 베지에 곡선 공식으로 현재 프레임의 위치 계산
        Vector3 m1 = Vector3.Lerp(startPos, controlPos, progress);
        Vector3 m2 = Vector3.Lerp(controlPos, targetPos, progress);
        Vector3 currentPos = Vector3.Lerp(m1, m2, progress);

        // 이동하기 전, 다음 프레임에 바라볼 방향으로 회전 정렬 (화살 촉이 진행 방향을 바라봄)
        Vector3 dir = currentPos - transform.position;
        if (dir != Vector3.zero)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        }

        transform.position = currentPos;
    }

    private void OnTargetHit()
    {
        // 1. 돈 벌기 연동
        GameManager.Instance.AddGold(damage);

        // 2. 타겟 컴포넌트를 찾아와서 데미지 입히기 (★ 추가된 부분)
        // Launcher가 쏘아 올린 TargetPoint 오브젝트에서 Target 스크립트를 가져옵니다.
        Target target = FindObjectOfType<ProjectileLauncher>().targetPoint.GetComponent<Target>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }

        // 3. 데미지 텍스트 생성
        if (damageTextPrefab != null)
        {
            GameObject textGo = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
            textGo.GetComponent<DamageText>().Setup(damage);
        }

        Destroy(gameObject);
    }
}