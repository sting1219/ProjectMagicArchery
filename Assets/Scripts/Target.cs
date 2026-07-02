using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("Hp Specs")]
    public double maxHp = 100;
    private double currentHp;

    [Header("Reward Specs")]
    // 이 과녁을 부쉈을 때 주는 기본 경험치 기본값
    public double baseExpReward = 20;

    private void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(double damage)
    {
        // 이미 죽어서 꺼진 상태라면 데미지를 받지 않음
        if (!gameObject.activeSelf) return;

        currentHp -= damage;
        Debug.Log($"과녁 피격! 남은 체력: {currentHp}");

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // 1. 매니저에게 유저가 '지금 선택해 둔 스탯'으로 경험치를 쌓으라고 요청
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddExpToSelectedStat(baseExpReward);
        }

        // [핵심] 2. UIManager에게 Craft 버튼을 켜라고 신호를 보냅니다.
        // FindObjectOfType을 쓰거나, 매니저 싱글톤이 있다면 그걸 쓰셔도 됩니다.
        UIManager ui = FindAnyObjectByType<UIManager>();
        if (ui != null)
        {
            ui.ShowCraftButton(true);
        }

        // [핵심] 3. 완전히 파괴하지 않고, 오브젝트를 맵에서 숨깁니다.
        gameObject.SetActive(false);
    }

    // [핵심] UIManager에서 버튼을 눌렀을 때 과녁을 재조립(부활)하는 함수
    public void ResetTarget()
    {
        currentHp = maxHp;
        gameObject.SetActive(true); // 다시 맵에 보이게 켜기
    }
}