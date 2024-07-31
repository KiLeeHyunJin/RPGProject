using UnityEngine.InputSystem;

public class KeyController
{
    InputActionAsset inputAsset;
    public KeyController(InputActionAsset inputActions)
    {
        inputAsset = inputActions;
        Init();
    }

    void Init()
    {
        InputActionMap map = inputAsset.FindActionMap("Player");
        if(map == null)
            inputAsset.AddActionMap("Player");

    }


}
