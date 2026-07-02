using UnityEngine;
using UnityEngine.InputSystem; // 유니티 최신 인풋 시스템 필수

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player Economic")]
    public double currentGold = 0;

    [Header("Upgrade Levels")]
    public int atkSpeedLevel = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddGold(double amount)
    {
        currentGold += amount;

        // ★ 핵심: 돈이 벌릴 때마다 빅넘버 변환 공식 결과를 콘솔창에 찍어줍니다!
        Debug.Log($"[골드 변동] 현재 보유량: {currentGold.ToBigNumberString()} (실제 double값: {currentGold})");
    }

    private void Update()
    {
        // [치트키] 게임 중에 키보드 G를 누르면 대용량 골드 주입!
        if (Keyboard.current != null && Keyboard.current.gKey.wasPressedThisFrame)
        {
            Debug.LogWarning("==== [치트] 키보드 G 입력! 거대 재화 주입 시도 ====");
            AddGold(9029392839828392); // 수경 단위의 거대한 숫자
        }
    }

    public float GetCurrentCooldown()
    {
        float cooldown = 1.0f - (atkSpeedLevel - 1) * 0.05f;
        return Mathf.Max(cooldown, 0.05f);
    }

    public double GetUpgradeCost()
    {
        return System.Math.Round(10 * System.Math.Pow(1.2, atkSpeedLevel - 1));
    }

    public void UpgradeAttackSpeed()
    {
        double cost = GetUpgradeCost();
        if (currentGold >= cost)
        {
            currentGold -= cost;
            atkSpeedLevel++;
        }
    }
}