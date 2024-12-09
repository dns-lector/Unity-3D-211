using UnityEngine;

public class BatteryScript : MonoBehaviour
{
    [SerializeField]
    private float charge = 0.5f;
    [SerializeField]
    private bool isRandomCharge = false;
    private AudioSource collectSound;
    private float destroyTimeout;

    void Start()
    {
        collectSound = GetComponent<AudioSource>();
        destroyTimeout = 0f;
        collectSound.volume = GameState.effectsVolume;
        GameState.Subscribe(OnSoundsVolumeTrigger, "EffectsVolume");
    }

    void Update()
    {
        if (destroyTimeout > 0f)
        {
            destroyTimeout -= Time.deltaTime;
            if (destroyTimeout <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isRandomCharge) charge = Random.Range(0.3f, 1.0f);

        if (other.gameObject.CompareTag("Player"))
        {
            // collectSound.volume = GameState.effectsVolume;
            collectSound.Play();
            GameState.TriggerGameEvent("Battery", new GameEvents.MessageEvent
            {
                message = $"Знайдено батарейку з зарядом {charge:F1}" ,
                data = charge
            });
            // Destroy(gameObject);
            destroyTimeout = .6f;
        }
    }

    private void OnSoundsVolumeTrigger(string eventName, object data)
    {
        if(eventName == "EffectsVolume")
        {
            collectSound.volume = (float) data;
        }
    }

    private void OnDestroy()
    {
        GameState.Unsubscribe(OnSoundsVolumeTrigger, "EffectsVolume");
    }
}
