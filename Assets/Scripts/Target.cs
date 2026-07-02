using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("HP Stats")]
    public double maxHp = 100;
    public double currentHp;
    private bool isDestroyed = false;

    private void Start()
    {
        ResetTarget();
    }

    // 타겟 초기화 (재생성할 때도 사용)
    public void ResetTarget()
    {
        currentHp = maxHp;
        isDestroyed = false;
        gameObject.SetActive(true); // 화면에 다시 보이게 설정
    }

    // 화살에 맞았을 때 데미지를 입는 함수
    public void TakeDamage(double damage)
    {
        if (isDestroyed) return;

        currentHp -= damage;
        if (currentHp <= 0)
        {
            currentHp = 0;
            OnDestroyed();
        }
    }

    private void OnDestroyed()
    {
        isDestroyed = true;
        gameObject.SetActive(false); // 타겟을 화면에서 숨김
        Debug.Log("과녁이 파괴되었습니다! 새로운 과녁을 크래프트하세요.");

        // UI 매니저에게 타겟이 부서졌으니 Craft 버튼을 켜라고 알림
        // (2단계에서 UIManager를 수정할 예정입니다)
        FindObjectOfType<UIManager>().ShowCraftButton(true);
    }
}