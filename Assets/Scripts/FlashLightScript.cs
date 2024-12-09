using UnityEngine;

public class FlashLightScript : MonoBehaviour
{
    private Transform parentTransform;
    private Light flashLight;
    private float charge;
    private float worktime = 50.0f;

    public float chargeLevel => Mathf.Clamp01(charge);

    void Start()
    {
        parentTransform = transform.parent;
        if(parentTransform == null)
        {
            Debug.LogError("FlashLightScript: parentTransform not found");
        }
        flashLight = GetComponent<Light>();
        charge = 1.0f;
        GameState.Subscribe(OnBatteryEvent, "Battery");
    }

    void Update()
    {
        if (parentTransform == null) return;

        if(charge > 0 && !GameState.isDay)
        {
            flashLight.intensity = chargeLevel;
            charge -= Time.deltaTime / worktime;
        }

        if (GameState.isFpv)
        {
            transform.forward = Camera.main.transform.forward;
        }
        else
        {
            Vector3 f = Camera.main.transform.forward;
            f.y = 0.0f;
            if (f == Vector3.zero) f = Camera.main.transform.up;
            transform.forward = f.normalized;
        }
    }

    private void OnBatteryEvent(string eventName, object data)
    {
        if (data is GameEvents.MessageEvent e)
        {
            charge += (float)e.data;
        }
    }
    private void OnDestroy()
    {
        GameState.Unsubscribe(OnBatteryEvent, "Battery");
    }
}
/* Д.З. До задач об'єкта "батарейка"
 * додати виведення повідомлень щодо факту збирання 
 * та кількості одержаного заряду.
 * Імплементувати через систему ігрових повідомлень.
 */
