using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using BatteOfHerone.Enuns;


namespace BatteOfHerone.Utils
{
    public static class EventManager
    {
        private static Dictionary<EventName, Action<object>> m_eventParams = new();
        private static Dictionary<EventName, Action> m_eventNotParams = new();

        private static Dictionary<EventName, object> m_eventsTriggereds = new();

        public static void StartListening(EventName eventName, Action<object> listener, bool includedBuffered = false)
        {
            if (includedBuffered && m_eventsTriggereds.ContainsKey(eventName))
            {
                listener.Invoke(m_eventsTriggereds[eventName]);
            }
            if (m_eventParams.TryGetValue(eventName, out Action<object> thisEvent))
            {
                //Add more event to the existing one
                thisEvent += listener;

                //Update the Dictionary
                m_eventParams[eventName] = thisEvent;
            }
            else
            {
                //Add event to the Dictionary for the first time
                thisEvent += listener;
                m_eventParams.Add(eventName, thisEvent);
            }
        }
        public static void StartListening(EventName eventName, Action listener, bool includedBuffered = false)
        {
            if (includedBuffered && m_eventsTriggereds.ContainsKey(eventName))
            {
                listener.Invoke();
            }
            if (m_eventNotParams.TryGetValue(eventName, out Action thisEvent))
            {
                //Add more event to the existing one
                thisEvent += listener;

                //Update the Dictionary
                m_eventNotParams[eventName] = thisEvent;
            }
            else
            {
                //Add event to the Dictionary for the first time
                thisEvent += listener;
                m_eventNotParams.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(EventName eventName, Action<object> listener)
        {
            if (m_eventParams.TryGetValue(eventName, out Action<object> thisEvent))
            {
                //Remove event from the existing one
                thisEvent -= listener;

                //Update the Dictionary
                m_eventParams[eventName] = thisEvent;
            }
        }
        public static void StopListening(EventName eventName, Action listener)
        {
            if (m_eventNotParams.TryGetValue(eventName, out Action thisEvent))
            {
                //Remove event from the existing one
                thisEvent -= listener;

                //Update the Dictionary
                m_eventNotParams[eventName] = thisEvent;
            }
        }

        public static void TriggerEvent(EventName eventName, object eventParam)
        {
            if (m_eventParams.TryGetValue(eventName, out Action<object> thisEvent))
            {
                if (thisEvent != null)
                {
                    thisEvent.Invoke(eventParam);
                }
            }
            m_eventsTriggereds[eventName] = eventParam;
        }
        public static void TriggerEvent(EventName eventName)
        {
            if (m_eventNotParams.TryGetValue(eventName, out Action thisEvent))
            {
                if (thisEvent != null)
                {
                    thisEvent.Invoke();
                }
            }
            m_eventsTriggereds[eventName] = null;
        }
    }

}