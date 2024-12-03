using System;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public static bool isDay { get; set; }
    public static bool isFpv { get; set; }

    #region Game events
    // Emit Signal Trigger Dispatch [Event]
    public static void TriggerGameEvent(String eventName, object data)
    {
        if (subscribers.ContainsKey(eventName))
        {
            foreach (var action in subscribers[eventName])
            {
                action(eventName, data);
            }
        }
    }

    private static Dictionary<string, List<Action<String, object>>> subscribers = new();
    public static void Subscribe(Action<String, object> action, String eventName)
    {
        if (subscribers.ContainsKey(eventName))
        {
            subscribers[eventName].Add(action);
        }
        else
        {
            subscribers[eventName] = new() { action };
        }
    }
    public static void Unsubscribe(Action<String, object> action, String eventName)
    {
        if (subscribers.ContainsKey(eventName))
        {
            subscribers[eventName].Remove(action);
        }
        else Debug.LogWarning("Unsubscribe of not subscribed key: " + eventName);
    }
    #endregion
}
/* Д.З. Реалізувати процес розряджання "батарейок" - якщо їх тривалий
 * час не збирають, то їх заряд зменшується.
 * - додати індикатор залишкового заряду
 * - реалізувати можливість зазначення часу розряду кожній батарейці окремо.
 */
