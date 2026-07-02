[System.Serializable]
public class StatData
{
    public string statID;        // 고유 ID (예: "Strength")
    public string displayName;   // UI 표시용 이름 (예: "Strength")
    public int level = 1;
    public double currentExp = 0;
    public double maxExp = 100;  // 레벨 1일 때 기본 요구 경험치

    // 기획에 맞춘 능력치별 가중치 계수들
    public float baseValue;      // 기본 증가 수치
    public float valuePerLevel;  // 레벨당 증가 수치

    // 현재 레벨에 따른 실제 능력치 효과 (예: 공격력 증가 N%, 공속 증가 N%)
    public float GetCalculatedValue()
    {
        return baseValue + ((level - 1) * valuePerLevel);
    }

    // 경험치를 추가하는 메서드 (레벨업 루프 포함)
    public bool AddExp(double amount)
    {
        bool didLevelUp = false;
        currentExp += amount;

        // 경험치가 가득 차면 연속 레벨업 처리 (기획의 멱함수 반영)
        while (currentExp >= maxExp)
        {
            currentExp -= maxExp;
            level++;

            // 기획 수식: 레벨이 오를수록 요구 경험치 1.25배 증가 (정수 처리)
            maxExp = System.Math.Round(maxExp * 1.25);
            didLevelUp = true;
        }

        return didLevelUp; // 레벨업 발생 여부를 반환하여 UI에 알림
    }
}