using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineController : MonoBehaviour
{
    public GameObject targetPoint;
    public GameObject hook;
    public Image barFill;

    //��� (exclusive)
    private bool isTargeting; //Ÿ����
    private bool isExpanding; //������
    private bool isRewinding; //�ǰ���
    
    //��� ���� ����
    private float throwStartTime; //������ ���� �ð�
    private float rewindStartTime; //�ǰ��� ���� �ð�
    private bool isNewClick; //���ο� Ŭ������ ���� (Ÿ���� �� Ŭ���� �ԷµǴ� ���� ����)

    //�÷��̾ �����ϴ� ��
    private Vector2 pressPosition; //������ ��ġ
    private float throwPower; //������ ��

    //�ٸ� ������� ���Ǵ� ��
    private Vector2 endPosition; //���� ���� ��ġ
    private Vector2 throwDirection; //���� ���� ����
    private float throwAngle; //���� ���� ����
    private float throwLength; //���� ���� ���� (start-end ���� �Ÿ�)

    //���˴� Ư��
    private LineRenderer lineRenderer; //���˴� ��
    private Vector2 startPosition; //�� �ʱ� ��ġ
    private float throwSpeed = 5.0f; //������ �ӵ�
    private float rewindSpeed = 6.0f; //�ǰ��� �ӵ�


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
        isExpanding = false;
        isRewinding = false;

        targetPoint.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTargeting)
        {
            bool isDone = SetTargetPoint();
            if (isDone)
            {
                //�ʿ��� ���� �� ���
                CalculateProperties();

                //������ ��� ���� �ð� ����
                throwStartTime = Time.time;

                //Ÿ���� ��忡�� ������ ���� ��ȯ
                isTargeting = false;
                isExpanding = true;
            }
        }
        else if (isExpanding)
        {
            bool isAtEndPoint = ExpandLineToEndPosition();
            if (isAtEndPoint)
            {
                //�ǰ��� ��� ���� �ð� ����
                rewindStartTime = Time.time;

                //������ ��忡�� �ǰ��� ���� ��ȯ
                isExpanding = false;
                isRewinding = true;
            }
        }
        else if (isRewinding)
        {
            bool isAtStartPoint = RewindLineToStartPosition();
            if (isAtStartPoint)
            {
                isRewinding = false;
                isTargeting = true;
            }
        }
    }
    
    private bool SetTargetPoint()
    {
        //����: Ÿ�� ����(pressPosition)�� ������ ��(throwPower) ���ϱ�. �Ϸ� �� true ��ȯ

        if (Input.GetMouseButtonDown(0)) //������ ���� ����
        {
            isNewClick = true;

            //Ÿ�� ����Ʈ ��ǥ ����
            Vector2 mousePosition = Input.mousePosition;
            pressPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            if (pressPosition.y <= 2.5) //�ٴ� ���� Ŭ�� �� ��ȿ
            {
                //������ �� �ʱ�ȭ
                throwPower = 0;

                //Ÿ�� ����Ʈ ǥ��
                targetPoint.transform.position = mousePosition;
                targetPoint.SetActive(true);

            }
        }

        if (Input.GetMouseButton(0)) //������ ���� ����
        {
            if(pressPosition.y <= 2.5)
            {
                //������ �� ����        
                if (throwPower <= 1)
                {
                    throwPower += Time.deltaTime;
                    barFill.fillAmount = throwPower;
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) //������ ���߸� ����
        {
            if (pressPosition.y <= 2.5 & isNewClick)
            {
                //Ÿ�� ����Ʈ ǥ�� ����
                targetPoint.SetActive(false);

                isNewClick = false;
                return true;
            }
        }
        return false;
    }

    private void CalculateProperties()
    {
        //����: ������ �� �ǰ��⿡ �ʿ��� ���� �� ���

        throwDirection = pressPosition - startPosition;
        
        throwAngle = Vector2.Angle(new Vector2(0, -1), throwDirection);
        if (throwDirection.x < 0)
        {
            throwAngle = throwAngle * -1;
        }

        endPosition = startPosition + (throwDirection * throwPower);

        throwLength = Vector2.Distance(startPosition, endPosition);

    }

    private bool ExpandLineToEndPosition()
    {
        //����: ���� ��ġ���� ���� Ȯ��. �����Ⱑ ������ true ��ȯ

        //�� �̵�
        float coveredLength = (Time.time - throwStartTime) * throwSpeed;
        float coveredRatio = coveredLength / throwLength;
        Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, coveredRatio);
        lineRenderer.SetPosition(1, new Vector3(newPosition.x, newPosition.y, 0f));
        
        //���� ���� ����
        hook.transform.rotation = Quaternion.Euler(0, 0, throwAngle);

        //���� �̵�
        hook.transform.position = newPosition;

        //������ �Ϸ�
        if (coveredRatio >= 1)
        {
            return true;
        }
        return false;
    }

    private bool RewindLineToStartPosition()
    {
        //����: �ʱ� ��ġ���� ���� ���. �ǰ��Ⱑ ������ true ��ȯ

        //�� �̵�
        float restoredLength = (Time.time - rewindStartTime) * rewindSpeed;
        float restoredRatio = restoredLength / throwLength;
        Vector3 newPosition = Vector3.Lerp(endPosition, startPosition, restoredRatio);
        lineRenderer.SetPosition(1, new Vector3(newPosition.x, newPosition.y, 0f));

        //���� ���� ����
        hook.transform.rotation = Quaternion.Euler(0, 0, throwAngle);

        //���� �̵�
        hook.transform.position = newPosition;

        //�ǰ��� �Ϸ�
        if (restoredRatio >= 1)
        {
            return true;
        }
        return false;
    }
}
