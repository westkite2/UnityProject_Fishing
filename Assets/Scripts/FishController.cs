using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    //����� �Ӽ�
    public float fishExp;
    public float swimSpeed;
    public int swimDirection;

    //���� ������Ʈ �� ��ũ��Ʈ
    private GameObject hook;
    private GameObject line;
    private GameObject gameManager;
    private FishingRodController fishingRodController;
    private ExpManager expManager;

    //����
    private bool isHooked;

    //���� ����
    private Vector2 hookPositionOffset;

    // Start is called before the first frame update
    void Start()
    {
        isHooked = false;
        hook = GameObject.Find("Hook");
        line = GameObject.Find("Line");
        gameManager = GameObject.Find("GameManager");
        fishingRodController = line.GetComponent<FishingRodController>();
        expManager = gameManager.GetComponent<ExpManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHooked) //���� ���
        {
            //������ ���� �̵�
            transform.position = hook.transform.TransformPoint(hookPositionOffset);

            //���� ���� �� �Ҹ�
            if (fishingRodController.gotFishFlag)
            {
                expManager.addExp(fishExp);
                this.gameObject.SetActive(false);
                isHooked = false;
            }
        }
    }

    private void FixedUpdate()
    {
        //����� �̵�
        Vector2 position = transform.position;
        position.x = position.x + swimSpeed * swimDirection * Time.deltaTime;
        transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //����� ����
        if (!fishingRodController.isFishCaughtFlag)
        {
            isHooked = true;
            fishingRodController.SetFishCaughtTrue();

            hookPositionOffset = transform.position - collision.gameObject.transform.position;
        }
    }
}