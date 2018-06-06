using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the attributes of the player based on the upgrade he has equipped
/// </summary>
public class UpgradeManager : MonoBehaviour {

	

}

class ConcreteSubject : BaseSubject
{
    private string state;

    public string SubjectState
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
        }
    }
}

class ConcreteObserver : BaseObserver
{
    public ConcreteSubject Subject
    {
        get
        {
            return subject;
        }
        set
        {
            subject = value;
        }
    }

    private string name;
    private string observerState;
    private ConcreteSubject subject;

    public ConcreteObserver(ConcreteSubject subject, string name)
    {
        this.subject = subject;
        this.name = name;
    }

    public override void MakeUpdate()
    {
        observerState = subject.SubjectState;
    }
}