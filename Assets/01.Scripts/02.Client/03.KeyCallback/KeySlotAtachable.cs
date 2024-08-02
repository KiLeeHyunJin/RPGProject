using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KeyAttachable : MonoBehaviour
{
    public abstract void Started();
    public abstract void Performed();
    public abstract void Canceled();
}
