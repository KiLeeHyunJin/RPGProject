using System.Collections.ObjectModel;
using System;
using UnityEngine;

public class LoginPanelController : MonoBehaviour
{
    public enum Panel { Login, SignUp, Verify, Reset }

    [SerializeField] LoginPanel loginPanel;
    //[SerializeField] SignUpPanel signUpPanel;
    //[SerializeField] ResetPanel resetPanel;
    //[SerializeField] VerifyPanel verifyPanel;
}
