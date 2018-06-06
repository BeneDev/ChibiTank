using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for any subject, which will get observed by an observer
/// </summary>
abstract class BaseSubject
{
    private List<BaseObserver> observers = new List<BaseObserver>();

    // Add a new observer
    public void Attach(BaseObserver observer)
    {
        observers.Add(observer);
    }

    // Remove a certain observer
    public void Detach(BaseObserver observer)
    {
        observers.Remove(observer);
    }

    // Notify all his observers about internal changes
    public void Notify()
    {
        foreach (BaseObserver o in observers)
        {
            o.MakeUpdate();
        }
    }
}
