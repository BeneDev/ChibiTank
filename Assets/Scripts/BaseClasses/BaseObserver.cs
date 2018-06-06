using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for any observer based object, which has to react to changes in his subjects, he is observing
/// </summary>
abstract class BaseObserver
{
    // Will react to the changes in the subjects
    public abstract void MakeUpdate();
}