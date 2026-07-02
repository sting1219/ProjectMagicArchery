using System;

public static class BigNumberExtensions
{
    // 단위를 담아둘 배열 (K, M, B, T, Qa, Qi, Sx, Sp, Oc, No, Dc ...)
    private static readonly string[] Units = {
        "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc"
    };

    /// <summary>
    /// double 숫자를 방치형 게임 스타일(예: 53.9M, 16.0K)로 변환합니다.
    /// </summary>
    public static string ToBigNumberString(this double value)
    {
        if (value == 0) return "0";

        // 음수 처리
        bool isNegative = value < 0;
        value = Math.Abs(value);

        // 1000 미만은 그냥 정수로 표현
        if (value < 1000)
        {
            return $"{(isNegative ? "-" : "")}{value:F0}";
        }

        // 1000 지수(로그) 계산을 통해 어떤 단위를 쓸지 인덱스 구함
        int unitIndex = (int)(Math.Log10(value) / 3);

        // 준비된 단위 배열 범위를 벗어나지 않도록 방어
        if (unitIndex >= Units.Length)
        {
            unitIndex = Units.Length - 1;
        }

        // 해당 단위에 맞는 값으로 축소 (예: 53,900,000 -> 53.9)
        double num = value / Math.Pow(1000, unitIndex);

        // 소수점 첫째 자리까지 표기 (예: 53.9M)
        string sign = isNegative ? "-" : "";
        return $"{sign}{num:F1}{Units[unitIndex]}";
    }
}