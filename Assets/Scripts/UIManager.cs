using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI goldText;

    // ★ 추가된 변수들
    public GameObject craftButtonObject; // Craft 버튼 오브젝트 자체
    public Target targetScript;          // 맵에 있는 Target 스크립트 연결

    private void Start()
    {
        // 게임 시작할 때는 Craft 버튼을 숨김
        ShowCraftButton(false);
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;

    }


    // ★ 타겟 파괴/생성 시 버튼을 켜고 끄는 함수
    public void ShowCraftButton(bool show)
    {
        if (craftButtonObject != null)
        {
            craftButtonObject.SetActive(show);
        }
    }

    // ★ Craft 버튼을 눌렀을 때 실행될 함수
    public void OnClickCraftTarget()
    {
        if (targetScript != null)
        {
            targetScript.ResetTarget(); // [주석 해제] 타겟 HP 리셋 및 부활!
            ShowCraftButton(false);     // Craft 버튼 다시 숨기기
        }
    }
}