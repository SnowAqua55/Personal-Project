using UnityEngine;
using UnityEngine.UI;




public class Condition : MonoBehaviour
{
    public float curValue;
    public float maxValue;
    public float startValue;
    public float passiveValue;
    public Image uiBar;

    void Start()
    {
        curValue = startValue;
    }

    private void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    float GetPercentage()
    {
        return curValue / maxValue;
    }

    public void Add(float Value)
    {
        curValue = Mathf.Min(curValue + Value, maxValue);  // Mathf.Min : �� ���� �� �� ���� ���� ��ȯ
    }

    public void Subtract(float Value)
    {
        curValue = Mathf.Max(curValue - Value, 0);  // Mathf.Max : �� ���� �� �� ū ���� ��ȯ
    }
}