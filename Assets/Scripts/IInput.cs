using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInput
{
    float Horizontal { get; }
    float Vertical { get; }

    float R_Horizontal { get; }
    float R_Vertical { get; }

    bool Shoot { get; }
}
