using UnityEngine;

[System.Serializable]
public class QuestData
{
    public string questID;
    public string displayName;

    public bool isUnlocked = false;   // 퀘스트 해금 여부
    public int level = 1;             // 현재 퀘스트 레벨
    public float duration = 2.0f;     // 루프 주기
    public float currentProgress = 0f;

    [Header("1레벨 기준 기본 보상")]
    public double baseGoldReward = 4;
    public double baseExpReward = 5;
    public string rewardStatID = "Concentration"; // 보상으로 줄 스탯 ID

    [Header("해금 및 업스킬 요구 조건")]
    public string requiredStatID = "Accuracy";   // 요구하는 스탯 ID
    public int unlockRequiredLevel = 10;         // 최초 해금에 필요한 스탯 레벨
    public int upskillLevelStep = 5;             // 레벨당 추가로 요구할 스탯 레벨 증가폭

    // 현재 레벨 반영된 보상 연산
    public double CurrentGoldReward => baseGoldReward * level;
    public double CurrentExpReward => baseExpReward * level;

    // 현재 레벨업(Upskill)에 필요한 스탯 레벨 계산 (예: 1레벨->2레벨 가는데 필요한 레벨)
    public int GetRequiredStatLevelForUpskill()
    {
        return unlockRequiredLevel + (level * upskillLevelStep);
    }
}