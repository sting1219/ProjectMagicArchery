using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlotUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI questNameText;
    public TextMeshProUGUI levelText;
    public Slider progressSlider;
    public Button upskillButton; // Upskill 버튼

    [Header("Lock Panel (Optional)")]
    public GameObject lockPanel; // 잠금 화면 오브젝트 (있을 경우)
    public Button unlockButton;  // 해금용 버튼

    [Header("Settings")]
    public string questID = "CityWatch";

    private QuestData targetData;

    private void Start()
    {
        if (upskillButton != null) upskillButton.onClick.AddListener(OnClickUpskill);
        if (unlockButton != null) unlockButton.onClick.AddListener(OnClickUnlock);
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;

        if (targetData == null)
        {
            targetData = GameManager.Instance.activeQuests.Find(q => q.questID == questID);
            if (targetData != null) questNameText.text = targetData.displayName;
        }

        if (targetData == null) return;

        // 1. 해금 여부에 따른 패널 스위칭
        if (lockPanel != null)
        {
            lockPanel.SetActive(!targetData.isUnlocked);
        }

        if (!targetData.isUnlocked) return; // 잠겨있으면 진행바 연산 안 함

        // 2. 실시간 데이터 동기화
        progressSlider.value = targetData.currentProgress / targetData.duration;
        levelText.text = $"Lv. {targetData.level}";

        // 3. 업스킬 버튼 활성화/비활성화 제어 (스탯 레벨 조건을 만족했는가?)
        if (GameManager.Instance.playerStats.TryGetValue(targetData.requiredStatID, out StatData stat))
        {
            bool canUpskill = stat.level >= targetData.GetRequiredStatLevelForUpskill();
            upskillButton.interactable = canUpskill; // 조건 만족 안 하면 버튼 클릭 불가 상화
        }
    }

    private void OnClickUpskill()
    {
        if (GameManager.Instance != null && GameManager.Instance.TryUpskillQuest(questID))
        {
            Debug.Log($"{questID} 업스킬 성공!");
        }
    }

    private void OnClickUnlock()
    {
        if (GameManager.Instance != null && GameManager.Instance.TryUnlockQuest(questID))
        {
            Debug.Log($"{questID} 해금 성공!");
        }
    }
}