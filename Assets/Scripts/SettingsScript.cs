using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    private GameObject content;
    private Slider effectsSlider;
    private Slider ambientSlider;
    private Slider sensitivityXSlider;
    private Slider sensitivityYSlider;
    private Toggle linkToggle;

    void Start()
    {
        Transform contentTransform = transform.Find("Content");
        content = contentTransform.gameObject;
        if(content.activeInHierarchy)
        {
            Time.timeScale = 0.0f;
        }
        effectsSlider = contentTransform
            .Find("Sound/EffectsSlider")
            .GetComponent<Slider>();
        OnEffectsSliderChanged(effectsSlider.value);

        ambientSlider = contentTransform
            .Find("Sound/AmbientSlider")
            .GetComponent<Slider>();
        OnAmbientSliderChanged(ambientSlider.value);

        sensitivityXSlider = contentTransform
            .Find("Controls/SensXSlider")
            .GetComponent<Slider>();
        sensitivityYSlider = contentTransform
            .Find("Controls/SensYSlider")
            .GetComponent<Slider>();
        linkToggle = contentTransform
            .Find("Controls/LinkToggle")
            .GetComponent<Toggle>();
        OnSensitivityXSliderChanged(sensitivityXSlider.value);
        if( ! linkToggle.isOn) OnSensitivityYSliderChanged(sensitivityYSlider.value);

        LoadSettings();
    }
    private void LoadSettings()
    {
        if(PlayerPrefs.HasKey(nameof(sensitivityXSlider)))
        {
            OnSensitivityXSliderChanged(
                PlayerPrefs.GetFloat(nameof(sensitivityXSlider))
            );
        }
        if(PlayerPrefs.HasKey(nameof(sensitivityYSlider)))
        {
            OnSensitivityYSliderChanged(
                PlayerPrefs.GetFloat(nameof(sensitivityYSlider))
            );
        }
        if(PlayerPrefs.HasKey(nameof(linkToggle)))
        {
            linkToggle.isOn = 
                PlayerPrefs.GetInt(nameof(linkToggle)) > 0;
        }
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Time.timeScale = content.activeInHierarchy ? 1.0f  : 0.0f;
            content.SetActive( ! content.activeInHierarchy );
        }
    }
    public void OnSaveButtonClick()
    {
        PlayerPrefs.SetFloat(nameof(sensitivityXSlider), sensitivityXSlider.value);
        PlayerPrefs.SetFloat(nameof(sensitivityYSlider), sensitivityYSlider.value);
        PlayerPrefs.SetInt(nameof(linkToggle), linkToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
    public void OnSensitivityXSliderChanged(float value)
    {
        float sens = Mathf.Lerp(1, 10, value);
        GameState.lookSensitivityX = sens;
        if (linkToggle.isOn)
        {
            sensitivityYSlider.value = value;
            GameState.lookSensitivityY = -sens;
        }
    }
    public void OnSensitivityYSliderChanged(float value)
    {
        float sens = Mathf.Lerp(1, 10, value);
        GameState.lookSensitivityY = -sens;
        if (linkToggle.isOn)
        {
            sensitivityXSlider.value = value;
            GameState.lookSensitivityX = sens;
        }
    }
    public void OnEffectsSliderChanged(float value)
    {
        // GameState.effectsVolume = value;
        GameState.TriggerGameEvent("EffectsVolume", GameState.effectsVolume = value);
    }
    public void OnAmbientSliderChanged(float value)
    {
        // GameState.effectsVolume = value;
        GameState.TriggerGameEvent("AmbientVolume", GameState.ambientVolume = value);
    }
}
/* Д.З. Реалізувати збереження налаштувань звуку (три гучності та перемикач)
 * та їх відновлення при запуску проєкту.
 */
