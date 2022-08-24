using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public float swimSpeed = 1;
    public GameObject hook;
    public GameObject line;

    private int swimDirection = 1;
    private bool isHooked;
    private Vector2 hookPositionOffset;

    FishingRodController fishingRodController;

    // Start is called before the first frame update
    void Start()
    {
        isHooked = false;
        fishingRodController = line.GetComponent<FishingRodController>();
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
                Debug.Log("ȹ��!");
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
        isHooked = true;

        fishingRodController.SetFishCaughtTrue();

        hookPositionOffset = transform.position - collision.gameObject.transform.position;
    }
   
}
