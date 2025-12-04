using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsSettings : MonoBehaviour
{
    public static EventString SwitchLanguage = new EventString();

    public static void SwitchLanguageEvent(string language)
    {
        SwitchLanguage?.Invoke(language);
    }
}


public class EventString : UnityEvent<string>
{ }
