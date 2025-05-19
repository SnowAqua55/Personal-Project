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
        curValue = Mathf.Min(curValue + Value, maxValue);  // Mathf.Min : 두 인자 값 중 작은 값을 반환
    }

    public void Subtract(float Value)
    {
        curValue = Mathf.Max(curValue - Value, 0);  // Mathf.Max : 두 인자 값 중 큰 값을 반환
    }
}