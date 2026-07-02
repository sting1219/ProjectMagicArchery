using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Selected Stat Management")]
    // [핵심] 유저가 UI에서 선택한 능력치의 ID (기본값은 Concentration)
    public string selectedStatID = "Concentration";

    // 6종 능력치를 Key-Value로 관리하여 확장성 확보
    public Dictionary<string, StatData> playerStats = new Dictionary<string, StatData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitStats();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 기획서 스펙에 맞춘 6종 기본 스탯 초기화
    private void InitStats()
    {
        playerStats.Add("Concentration", new StatData { statID = "Concentration", displayName = "Concentration", baseValue = 0f, valuePerLevel = 10f, maxExp = 50 });
        playerStats.Add("Strength", new StatData { statID = "Strength", displayName = "Strength", baseValue = 0f, valuePerLevel = 15f, maxExp = 100 });
        playerStats.Add("Dexterity", new StatData { statID = "Dexterity", displayName = "Dexterity", baseValue = 0f, valuePerLevel = 5f, maxExp = 100 });
        playerStats.Add("Accuracy", new StatData { statID = "Accuracy", displayName = "Accuracy", baseValue = 0f, valuePerLevel = 5f, maxExp = 120 });
        playerStats.Add("Vitality", new StatData { statID = "Vitality", displayName = "Vitality", baseValue = 0f, valuePerLevel = 20f, maxExp = 150 });
        playerStats.Add("MagicAffinity", new StatData { statID = "MagicAffinity", displayName = "Magic Affinity", baseValue = 0f, valuePerLevel = 10f, maxExp = 200 });
    }

    // [핵심] UI 버튼을 눌러 선택 스탯을 바꿀 때 호출할 메서드
    public void SelectStat(string statID)
    {
        if (playerStats.ContainsKey(statID))
        {
            selectedStatID = statID;
            Debug.Log($"현재 선택된 능력치 변경: {selectedStatID}");
        }
    }

    // [핵심] 현재 선택된 능력치에 경험치를 꽂아주는 메서드
    public void AddExpToSelectedStat(double baseAmount)
    {
        if (!playerStats.ContainsKey(selectedStatID)) return;

        StatData activeStat = playerStats[selectedStatID];

        // 기획 반영: 만약 Concentration 레벨이 올라가 있다면 경험치 획득량 N% 증가 적용
        double finalExp = baseAmount;
        if (playerStats.ContainsKey("Concentration"))
        {
            float bonusPercent = playerStats["Concentration"].GetCalculatedValue(); // 예: 10%면 10
            finalExp = baseAmount * (1f + (bonusPercent / 100f));
        }

        // 경험치 주입 및 레벨업 체크
        bool isLevelUp = activeStat.AddExp(finalExp);

        Debug.Log($"{activeStat.displayName} 경험치 획득: +{finalExp:F1} (현재: {activeStat.currentExp}/{activeStat.maxExp})");

        if (isLevelUp)
        {
            Debug.LogWarning($"★ {activeStat.displayName} 레벨업! 현재 Lv.{activeStat.level} ★");
            // TODO: 나중에 여기에 UI 스탯창을 새로고침하는 코드를 연결합니다.
        }
    }
}