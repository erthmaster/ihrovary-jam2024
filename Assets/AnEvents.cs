using UnityEngine;
using UnityEngine.Events;

public class AnEvents : MonoBehaviour
{
    public UnityEvent Event1;
    public UnityEvent Event2;

    public void TriggerEvent1()
    {
        Event1.Invoke();
    }
    public void TriggerEvent2()
    {
        Event2.Invoke();
    }
}
