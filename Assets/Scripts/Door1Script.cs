using System.Linq;
using UnityEngine;

public class Door1Script : MonoBehaviour
{
    [SerializeField]
    private string requiredKey = "1";
    private float openingTime = 3.0f;
    private float timeout = 0f;
    private AudioSource closedSound;
    private AudioSource openedSound;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(GameState.collectedItems.ContainsKey("Key" + requiredKey))
            {
                GameState.TriggerGameEvent("Door1",
                    new GameEvents.MessageEvent 
                    {
                        message = "Двері відчиняються",
                        data = requiredKey
                    });
                timeout = openingTime;
                openedSound.Play();
            }
            else
            {
                GameState.TriggerGameEvent("Door1", new GameEvents.MessageEvent
                {
                    message = "Для відкривання двері вам необхідно знайти ключ " + requiredKey,
                    data = requiredKey
                });
                closedSound.Play();
            }            
        }
    }
    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        closedSound = audioSources[0];
        openedSound = audioSources[1];
    }

    void Update()
    {
        if (timeout > 0f)
        {
            transform.Translate(Time.deltaTime / openingTime, 0, 0);
            timeout -= Time.deltaTime;
        }
    }
}
/* Д.З. додати до проєкту звуки
 * - перемикання день/ніч
 * - розряджання ліхтаря (** на рівень 30% один звук, на 10% - інший)
 * - звук одержання ключа (проходження KeyPoint) (** вчасно / не вчасно)
 */
