using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectChannel : MonoBehaviour
{
    [SerializeField] int serverNum;
    public void EnterServer()
    {
        ServerDataManager.Instance.EnterServer(serverNum);
    }
}
