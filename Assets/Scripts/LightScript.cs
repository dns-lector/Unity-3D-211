using System.Linq;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    private Light[] dayLights;
    private Light[] nightLights;
    private AudioSource dayAmbientSound;

    void Start()
    {
        dayLights = GameObject
            .FindGameObjectsWithTag("DayLight")
            .Select(g => g.GetComponent<Light>())
            .ToArray();

        nightLights = GameObject
            .FindGameObjectsWithTag("NightLight")
            .Select(g => g.GetComponent<Light>())
            .ToArray();

        dayAmbientSound = GetComponent<AudioSource>();
        dayAmbientSound.volume = GameState.ambientVolume;
        GameState.Subscribe(OnSoundsVolumeTrigger, "AmbientVolume");
        SwitchLight();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.N))
        {
            SwitchLight();
        }
    }

    private void OnSoundsVolumeTrigger(string eventName, object data)
    {
        if (eventName == "AmbientVolume")
        {
            dayAmbientSound.volume = (float)data;
        }
    }

    private void SwitchLight()
    {
        GameState.isDay = !GameState.isDay;
        foreach (Light light in dayLights)
        {
            light.enabled = GameState.isDay;
        }
        foreach (Light light in nightLights)
        {
            light.enabled = !GameState.isDay;
        }
    }

    private void OnDestroy()
    {
        GameState.Unsubscribe(OnSoundsVolumeTrigger, "AmbientVolume");
    }
}
/* Д.З. Додати до проєкта музикальне оформлення
 * - підібрати кліпи для денного/нічного озвучування
 * - забезпечити регулювання їх гучності через меню налаштувань
 * * реалізувати перемикання звуків у відповідності до "дня/ночі"
 * - реалізувати зменшення гучності до 0 усіх видів звуків при 
 *    виборі "галочки" вимкнення всіх звуків.
 */
