using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    private Transform character;
    private InputAction lookAction;
    private Vector3 cameraAngles, cameraAngles0;
    private Vector3 r;
    //private float sensitivityH = 5.0f;
    //private float sensitivityV = -3.0f;
    private float minFpvDistance = 0.9f;
    private float maxFpvDistance = 9.0f;
    private bool isPos3;

    void Start()
    {
        lookAction = InputSystem.actions.FindAction("Look");
        cameraAngles0 = cameraAngles = this.transform.eulerAngles;
        character = GameObject.Find("Character").transform;
        r = this.transform.position - character.position;
        isPos3 = false;
    }

    void Update()
    {
        Vector2 wheel = Input.mouseScrollDelta;
        if(wheel.y != 0)
        {
            if (r.magnitude >= maxFpvDistance)
            {
                isPos3 = true;
                if (wheel.y > 0)
                {
                    r *= 1 - wheel.y / 10;
                }
            }
            else
            {
                isPos3 = false;
                if (r.magnitude >= minFpvDistance)
                {
                    float rr = r.magnitude * (1 - wheel.y / 10);
                    if (rr <= minFpvDistance)
                    {
                        r *= 0.01f;
                        GameState.isFpv = true;
                    }
                    else
                    {
                        r *= (1 - wheel.y / 10);
                    }
                }
                else
                {
                    if (wheel.y < 0)
                    {
                        r *= 100f;
                        GameState.isFpv = false;
                    }
                }
            }
        }
        if (!isPos3)
        {
            Vector2 lookValue = lookAction.ReadValue<Vector2>();
            if (lookValue != Vector2.zero)
            {
                cameraAngles.x += lookValue.y * Time.deltaTime * GameState.lookSensitivityY;
                cameraAngles.y += lookValue.x * Time.deltaTime * GameState.lookSensitivityX;
                this.transform.eulerAngles = cameraAngles;
            }
            this.transform.position = character.position +
                Quaternion.Euler(
                    cameraAngles.x - cameraAngles0.x, 
                    cameraAngles.y - cameraAngles0.y, 
                    0
                ) * r;
        }
    }
}
/* Обмежити величину зміни кута камери по вертикалі (у бік горизонту)
 * у залежності від режиму її роботи:
 *  FPV: від -10 градусів до 40 градусів
 *  інакше: від 35 градусів до 75 градусів
 *  
 * Обмежити величину максимального віддалення камери від персонажу
 *  - підібрати практично за відстаню, з якої видно все поле
 *  \
 *  
 *  Д.З. Підібрати текстуру неба (Skybox)
 *  Розмістити по сцені декілька джерел точкового світла.
 */