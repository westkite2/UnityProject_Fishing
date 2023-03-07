using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpManager : MonoBehaviour
{
    public Text expText;
    public Image expBarFill;

    private float exp;

    private void Start()
    {
        exp = 0;
        expBarFill.fillAmount = exp;
    }

    // Update is called once per frame
    void Update()
    {
        //���� ����
        expText.text = (exp * 10).ToString();
    }
    
    public void addExp(float amount)
    {
        //���� ����
        exp += amount;
        expBarFill.fillAmount = exp;
    }
}
