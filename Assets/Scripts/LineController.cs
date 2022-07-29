using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineController : MonoBehaviour
{
    public GameObject targetPoint;
    public GameObject hook;
    public Image barFill;

    //�÷��� ���
    private bool isTargeting;

    //������ �Ӽ�
    private LineRenderer lineRenderer;

    private Vector2 startPosition; //������ ����
    private Vector2 pressPosition;
    private Vector2 endPosition;

    private float throwPower; //������ ��
    private float throwLength; //������ �Ÿ�
    private float throwSpeed = 5.0f; //������ �ӵ�
    private float throwStartTime; //������ ���� �ð�

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        startPosition = new Vector2(-0.4f, 4f);
        lineRenderer.SetPosition(0, new Vector3(startPosition.x, startPosition.y, 0f));
        lineRenderer.SetPosition(1, new Vector3(startPosition.x, startPosition.y, 0f));
        hook.transform.position = startPosition;
        
        isTargeting = true;

        targetPoint.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTargeting)
        {
            SetTargetPoint();
        }
        else
        {
            ExpandLine();

            
            //RewindLine();
            //isTargeting = true;
        }




    }

    private void SetTargetPoint()
    {
        //����: Ÿ�� ������ ��ǥ�� ������ �� ���ϱ�

        if (Input.GetMouseButtonDown(0)) //������ ���� ����
        {
            //������ �� �ʱ�ȭ
            throwPower = 0;

            //Ÿ�� ����Ʈ ǥ��
            Vector2 mousePosition = Input.mousePosition;
            targetPoint.transform.position = mousePosition;
            targetPoint.SetActive(true);

            //Ÿ�� ����Ʈ ��ǥ ����
            pressPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        }

        if (Input.GetMouseButton(0)) //������ ���� ����
        {
            //������ �� ����        
            if (throwPower <= 1)
            {
                throwPower += Time.deltaTime;
                barFill.fillAmount = throwPower;
            }
        }

        if (Input.GetMouseButtonUp(0)) //������ ���߸� ����
        {
            //Ÿ�� ����Ʈ ǥ�� ����
            targetPoint.SetActive(false);

            //������ ���� �ð� ����
            throwStartTime = Time.time;

            //Ÿ���� ��� ����
            isTargeting = false;
        }
    }

    void ExpandLine()
    {
        //����: ��ǥ �������� ���� Ȯ��

        //������ ���� ���
        Vector2 throwDirection = pressPosition - startPosition;
        endPosition = startPosition + (throwDirection * throwPower);

        //������ �ִϸ��̼�
        throwLength = Vector2.Distance(startPosition, endPosition);
        float coverdLength = (Time.time - throwStartTime) * throwSpeed;
        float coveredRatio = coverdLength / throwLength;
        Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, coveredRatio);
        lineRenderer.SetPosition(1, new Vector3(newPosition.x, newPosition.y, 0f));

        //���� �̵�
        hook.transform.position = newPosition;

        //���� ���� ����
        float throwAngle = Vector2.Angle(new Vector2(0, -1), throwDirection);
        if (throwDirection.x < 0 )
        {
            throwAngle = throwAngle * -1;
        }
        hook.transform.rotation = Quaternion.Euler(0, 0, throwAngle);
    }

}
