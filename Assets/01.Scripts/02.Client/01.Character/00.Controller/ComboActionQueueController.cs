using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComboActionQueueController
{
    [SerializeField]
    private float _maxTimeForComboExecution = 0.5f;
    [SerializeField]
    private float _minimumTimeBetweenComboInputs = 0.5f;
    [SerializeField]
    private float _comboTimeAddedWithNewCommand = 0.5f;
    [SerializeField]
    private int _maxComboLengthBeforeTrim = 6;

    private float _timeSinceLastComboInput;
    private float _timeSinceFirstComboInput;
    private Queue<Key> _comboSequence;
    private ComboMatchEngine _comboMatchEngine;
    private int _minimumComboLength = 2;
    private int _thresholdToExtendComboTime;

    public ComboActionQueueController()
    {
        _comboSequence = new Queue<Key>();
        _comboMatchEngine = new ComboMatchEngine(this, _minimumComboLength);
    }

    public void Update()
    {
        ProcessComboUpdate(Time.deltaTime);
    }


    private void ProcessComboUpdate(float deltaTime)
    {
        //입력한게 없다면 종료
        if (_comboSequence.Count == 0) return;

        //입력 대기시간 증가
        _timeSinceFirstComboInput += deltaTime;
        _timeSinceLastComboInput += deltaTime;

        //최대시간 경과시 초기화
        if (_timeSinceFirstComboInput > _maxTimeForComboExecution)
        {
            ClearComboSequence();
        }
    }

    public void AddCommandToComboSequence(Key key)
    {
        //기존 입력키 개수 확인 및 시작할 수 있는 콤보 확인
        ClearSequenceIfCannotStartCombo();
        //콤보개수 확인 초과시 맨 먼저 입력 키 삭제
        DequeueOldestCommandIfNecessary();

        //입력 제한 시간 확인
        if (IsTooLateForCombo())
        {
            //초기화
            ClearComboSequence();
            return;
        }

        //입력 개수가 
        if (ShouldExtendComboTime())
        {
            //입력 대기 경과시간 일부 삭제
            _timeSinceFirstComboInput -= _comboTimeAddedWithNewCommand;
        }
        //키 대입
        EnqueueCommandAndResetTimers(key);
        //콤보 체크
        _comboMatchEngine.CheckAndExecuteCombo();
    }


    private void ClearSequenceIfCannotStartCombo()
    {
        //콤보 입력수가 0개 이상인데
        //콤보 시작과 매칭이 안된다면 
        if (_comboSequence.Count > 0 && !_comboMatchEngine.CanStartCombo(_comboSequence))
        {
            //초기화
            ClearComboSequence();
            return;
        }
    }

    private void DequeueOldestCommandIfNecessary()
    {
        //콤보 개수가 최대 입력개수와 같거나 넘는다면 맨 밑에것을 제거
        if (_comboSequence.Count >= _maxComboLengthBeforeTrim)
        {
            _comboSequence.Dequeue();
        }
    }

    //입력 시간을 초기화했다면 
    private bool IsTooLateForCombo()
    {
        if (_timeSinceLastComboInput < _minimumTimeBetweenComboInputs)
        {
            return false;
        }
        return true;
    }

    private void EnqueueCommandAndResetTimers(Key key)
    {
        _comboSequence.Enqueue(key);

        //입력한 키가 1개라면 첫 입력
        if (IsFirstCommandInSequence())
        {
            _timeSinceFirstComboInput = 0;
        }
        //마지막 입력시간으로부터 경과시간 초기화
        _timeSinceLastComboInput = 0;
    }

    //첫 입력이라면 true
    private bool IsFirstCommandInSequence()
    {
        return _comboSequence.Count == 1;
    }


    //현재 명령과 콤보 시퀀스를 기반으로 콤보 시간을 연장할지 결정
    private bool ShouldExtendComboTime()
    {
        return _comboSequence.Count >= _thresholdToExtendComboTime;
    }

    public Queue<Key> GetComboSequence()
    {
        return _comboSequence;
    }

    //초기화
    public void ClearComboSequence()
    {
        _comboSequence.Clear();
        _timeSinceFirstComboInput = 0;
        _timeSinceLastComboInput = 0;
    }
}
