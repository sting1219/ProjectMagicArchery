using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Economy")]
    public double gold = 0;

    [Header("Selected Stat Management")]
    public string selectedStatID = "Concentration";
    public Dictionary<string, StatData> playerStats = new Dictionary<string, StatData>();

    [Header("Backend Quest Management")]
    public List<QuestData> activeQuests = new List<QuestData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitStats();
            InitQuests();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        UpdateBackendQuests();
    }

    private void InitStats()
    {
        // 테스트를 위해 기본 레벨을 1로 세팅 (나중에 과녁 부수거나 퀘스트 보상으로 70까지 성장 가능)
        playerStats.Add("Concentration", new StatData { statID = "Concentration", displayName = "Concentration", level = 1, maxExp = 50 });
        playerStats.Add("Strength", new StatData { statID = "Strength", displayName = "Strength", level = 1, maxExp = 100 });
        playerStats.Add("Dexterity", new StatData { statID = "Dexterity", displayName = "Dexterity", level = 1, maxExp = 100 });
        playerStats.Add("Accuracy", new StatData { statID = "Accuracy", displayName = "Accuracy", level = 1, maxExp = 120 });
        playerStats.Add("Vitality", new StatData { statID = "Vitality", displayName = "Vitality", level = 1, maxExp = 150 });
        playerStats.Add("MagicAffinity", new StatData { statID = "MagicAffinity", displayName = "Magic Affinity", level = 1, maxExp = 200 });
    }

    private void InitQuests()
    {
        activeQuests.Clear();

        // 스크린샷 데이터 기반 매핑 (예: City Watch 퀘스트)
        activeQuests.Add(new QuestData
        {
            questID = "CityWatch",
            displayName = "City Watch",
            isUnlocked = true, // 첫 번째는 열려있다고 가정
            duration = 3.0f,
            baseGoldReward = 1000,
            baseExpReward = 106000,
            rewardStatID = "Concentration",
            requiredStatID = "Concentration", // 업스킬 시 Concentration 요구
            unlockRequiredLevel = 10,
            upskillLevelStep = 10
        });

        // 잠겨있는 다음 퀘스트 예시
        activeQuests.Add(new QuestData
        {
            questID = "ForestPatrol",
            displayName = "Forest Patrol",
            isUnlocked = false,
            duration = 4.0f,
            baseGoldReward = 5000,
            baseExpReward = 500000,
            rewardStatID = "Strength",
            requiredStatID = "Accuracy", // 해금 시 Accuracy 요구
            unlockRequiredLevel = 70,
            upskillLevelStep = 15
        });
    }

    private void UpdateBackendQuests()
    {
        foreach (QuestData quest in activeQuests)
        {
            if (quest == null || !quest.isUnlocked) continue; // 해금 안 된 퀘스트는 루프 안 도ㅁ

            quest.currentProgress += Time.deltaTime;

            if (quest.currentProgress >= quest.duration)
            {
                quest.currentProgress = 0f;

                // 보상 지급
                gold += quest.CurrentGoldReward;
                if (playerStats.TryGetValue(quest.rewardStatID, out StatData stat))
                {
                    stat.AddExp(quest.CurrentExpReward);
                }
            }
        }
    }

    // [체크] 해금 버튼 클릭 시 호출
    public bool TryUnlockQuest(string questID)
    {
        QuestData quest = activeQuests.Find(q => q.questID == questID);
        if (quest == null || quest.isUnlocked) return false;

        if (playerStats.TryGetValue(quest.requiredStatID, out StatData stat))
        {
            if (stat.level >= quest.unlockRequiredLevel)
            {
                quest.isUnlocked = true;
                return true;
            }
        }
        return false;
    }

    // [체크] 업스킬 버튼 클릭 시 호출
    public bool TryUpskillQuest(string questID)
    {
        QuestData quest = activeQuests.Find(q => q.questID == questID);
        if (quest == null || !quest.isUnlocked) return false;

        if (playerStats.TryGetValue(quest.requiredStatID, out StatData stat))
        {
            // 요구 조건 레벨 만족하는지 검사
            if (stat.level >= quest.GetRequiredStatLevelForUpskill())
            {
                quest.level++;
                return true;
            }
        }
        return false;
    }

    // 1. StatSlotUI가 스탯 탭을 클릭하거나 토글할 때 선택한 스탯 ID를 바꾸는 함수
    public void SelectStat(string statID)
    {
        if (playerStats.ContainsKey(statID))
        {
            selectedStatID = statID;
            Debug.Log($"현재 집중 사냥 능력치 변경: {selectedStatID}");
        }
    }

    // 2. Target(과녁)을 부셨을 때 현재 선택된(초록불 켜진) 스탯에 경험치를 먹이는 함수
    public void AddExpToSelectedStat(double baseAmount)
    {
        if (!playerStats.ContainsKey(selectedStatID)) return;

        StatData activeStat = playerStats[selectedStatID];
        double finalExp = baseAmount;

        // Concentration(집중 사냥) 스탯이 레벨업되어 있다면 보너스 경험치 획득 연산
        if (playerStats.ContainsKey("Concentration"))
        {
            // GetCalculatedValue() 함수가 StatData에 구현되어 있다면 사용, 
            // 없다면 그냥 간단하게 level 수치를 활용하도록 방어 코드를 작성합니다.
            float bonusPercent = playerStats["Concentration"].level * 10f; // 레벨당 10% 보너스 예시
            finalExp = baseAmount * (1f + (bonusPercent / 100f));
        }

        activeStat.AddExp(finalExp);
        Debug.Log($"{activeStat.displayName} 경험치 획득: +{finalExp:F1} (현재: {activeStat.currentExp}/{activeStat.maxExp})");
    }
}