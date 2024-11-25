using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    private Transform character;
    private InputAction lookAction;
    private Vector3 cameraAngles;
    private Vector3 r;
    private float sensitivityH = 5.0f;
    private float sensitivityV = -3.0f;
    private float minFpvDistance = 0.9f;

    void Start()
    {
        lookAction = InputSystem.actions.FindAction("Look");
        cameraAngles = this.transform.eulerAngles;
        character = GameObject.Find("Character").transform;
        r = this.transform.position - character.position;
    }

    void Update()
    {
        Vector2 wheel = Input.mouseScrollDelta;
        if(wheel.y != 0)
        {
            if(r.magnitude >= minFpvDistance)
            {
                float rr = r.magnitude * (1 - wheel.y / 10);
                if (rr <= minFpvDistance)
                {
                    r *= 0.01f;
                }
                else
                {
                    r *= (1 - wheel.y / 10);
                }
            }
            else
            {
                if(wheel.y < 0)
                {
                    r *= 100f;
                }
            }
        }

        Vector2 lookValue = lookAction.ReadValue<Vector2>();
        if(lookValue != Vector2.zero)
        {
            cameraAngles.x += lookValue.y * Time.deltaTime * sensitivityV;
            cameraAngles.y += lookValue.x * Time.deltaTime * sensitivityH;
            this.transform.eulerAngles = cameraAngles;
        }
        this.transform.position = character.position + 
            Quaternion.Euler(0, cameraAngles.y, 0) * r;
    }
}
/* �������� �������� ���� ���� ������ �� �������� (� �� ���������)
 * � ��������� �� ������ �� ������:
 *  FPV: �� -10 ������� �� 40 �������
 *  ������: �� 35 ������� �� 75 �������
 *  
 * �������� �������� ������������� ��������� ������ �� ���������
 *  - ������� ��������� �� �������, � ��� ����� ��� ����
 */