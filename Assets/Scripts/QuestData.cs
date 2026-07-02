[System.Serializable]
public class QuestData
{
    public string questID;
    public string displayName;
    public float duration = 2.0f; // 완료 주기 (초)
    public int goldReward = 1;
    public string targetStatReward; // 보상으로 줄 EXP의 스탯 ID
    public double expReward = 10;

    public string reqStatID;       // 해금에 필요한 스탯 ID
    public int reqStatLevel = 0;   // 해금에 필요한 레벨
}