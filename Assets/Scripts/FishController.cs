using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    //����� �Ӽ�
    public float swimSpeed = 1;
    public int fishScore = 10;
    private int swimDirection = 1;

    //���� ������Ʈ �� ��ũ��Ʈ
    private GameObject hook;
    private GameObject line;
    private GameObject gameManager;
    private FishingRodController fishingRodController;
    private ScoreManager scoreManager;
    
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
        scoreManager = gameManager.GetComponent<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHooked)
        {
            //���� ��� ���˴븦 ���� �̵�
            transform.position = hook.transform.TransformPoint(hookPositionOffset);

            if (fishingRodController.gotFishFlag)
            {
                scoreManager.addScore(fishScore);
                this.gameObject.SetActive(false);
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
