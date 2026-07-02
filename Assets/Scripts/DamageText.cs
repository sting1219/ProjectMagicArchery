using TMPro; // TMP를 쓰기 위해 이 줄을 꼭 추가해야 합니다!
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private float moveSpeed = 2f;
    private float alphaSpeed = 1.5f;
    private TextMeshPro textMesh; // TextMesh -> TextMeshPro로 변경
    private Color textColor;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>(); // GetComponent도 변경
        textColor = textMesh.color;
    }

    public void Setup(double damage) // 매개변수도 대용량 연산에 맞춰 double로 변경합니다.
    {
        textMesh.text = damage.ToBigNumberString();
    }

    private void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        textColor.a -= alphaSpeed * Time.deltaTime;
        textMesh.color = textColor;

        if (textColor.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}