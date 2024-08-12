using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

public class ComboMatchEngine
{
    readonly private ComboActionQueueController _actionQueueManager;
    private List<IComboRule> _comboRules;
    readonly private int _minimumComboLength;

    public ComboMatchEngine(ComboActionQueueController manager, int minimumComboLength)
    {
        _actionQueueManager = manager;
        _minimumComboLength = minimumComboLength;
        InitializeComboRules();
    }

    public void SetComboList(IComboRule[] comboArrays)
    {
        _comboRules = new(comboArrays);
    }

    private void InitializeComboRules()
    {
        _comboRules = new();

    }

    //시작 가능한 첫 콤보가 있는지 확인한다.
    internal bool CanStartCombo(Queue<Key> comboSequence)
    {
        return DoesFirstActionStartCombo(comboSequence);
    }

    public void CheckAndExecuteCombo()
    {
        //입력키를 가져온다.
        Queue<Key> comboSequence = _actionQueueManager.GetComboSequence();

        //최소한의 입력 개수보다 적다면 종료한다.
        if (comboSequence.Count < _minimumComboLength) 
        { 
            return; 
        }

        //현재 키의 콤보를 가져온다.
        IGameplayActionCommand comboCommand = CheckSequenceForCombo(comboSequence);

        //있다면
        if (comboCommand != null)
        {
            //콤보를 가져온다.
            ExecuteComboAction(comboCommand);
            //ComboEventSystem.OnNewCombo.Invoke();
            //콤보를 초기화한다.
            _actionQueueManager.ClearComboSequence();
        }
    }

    private bool DoesFirstActionStartCombo(Queue<Key> comboSequence)
    {
        //순회하면서 시작 가능한 콤보 확인
        foreach (var rule in _comboRules)
        {
            if (rule.IsFirstConditionMet(comboSequence.Peek()))
            {
                return true;
            }
        }
        return false;
    }

    private IGameplayActionCommand CheckSequenceForCombo(Queue<Key> comboSequence)
    {
        //입력키를 순회한다.
        for (int startIndex = 0; startIndex <= comboSequence.Count; startIndex++)
        {
            //입력키의 부분시퀀스를 가져온다.
            IEnumerable<Key> subsequence = GetSubsequence(comboSequence, startIndex);

            //모든 콤보를 순회한다.
            foreach (IComboRule rule in _comboRules)
            {
                //맞는지 확인한다.
                if (rule.IsMatch(subsequence))
                {
                    //해당 콤보를 반환한다.
                    return rule.GetResultingComboCommand();
                }
            }
        }
        return null;
    }

    private IEnumerable<Key> GetSubsequence(Queue<Key> comboSequence, int startIndex)
    {
        return comboSequence.Skip(startIndex);
    }

    private void ExecuteComboAction(IGameplayActionCommand comboCommand)
    {
        comboCommand.ExecuteAction();
    }
}
