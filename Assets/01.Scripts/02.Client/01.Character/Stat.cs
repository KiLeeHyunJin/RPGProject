using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    public Stat(int _str, int _def, int _man, int _luk)
    {
        str = _str;
        def = _def;
        man = _man;
        luk = _luk;
    }
    public int def;
    public int luk;
    public int man;
    public int str;
}
