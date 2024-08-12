using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
public class ComboXXY : IComboRule
{
    private ComboActionCommandFactory _comboActionCommandFactory;

    public int ComboLength { get; private set; }

    public ComboXXY(ComboActionCommandFactory actionCommandFactory)
    {
        _comboActionCommandFactory = actionCommandFactory;
        ComboLength = 3;
    }


    public IGameplayActionCommand GetResultingComboCommand()
    {
        return _comboActionCommandFactory.CreateDashAttackCommand();
    }

    public bool IsFirstConditionMet(Key firstCommand)
    {
        return firstCommand is Key.X;
    }

    public bool IsMatch(IEnumerable<Key> sequence)
    {

        var sequenceArray = sequence.Take(ComboLength).ToArray();

        if (sequenceArray.Length < ComboLength)
        {
            return false;
        }

        var first = sequenceArray[0];
        var second = sequenceArray[1];
        var third = sequenceArray[2];

        return first is Key.X && second is Key.X && third is Key.Y;
    }
}