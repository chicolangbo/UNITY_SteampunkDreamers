using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public enum StateName
{
    Speed, // �ӷ� ���� + ���� �ü����� ���� �� �̵� -> ���� ������ �Ǹ� Launch�� ����
    Angle, // ���� ���� ���� ���� ���� ���� -> Gliding���� ����(���� ���� bool�� ����)
    Gliding, // Ȱ�� -> floor�� �浹�ϸ� Landing���� ����
    Landing // ����
}

public class StateMachine
{
    public BaseState CurrentState { get; private set; }
    private Dictionary<StateName, BaseState> states = new Dictionary<StateName, BaseState>();

    public StateMachine(StateName stateName, BaseState state)
    {
        AddState(stateName, state);
        CurrentState = GetState(stateName);
    }

    public void AddState(StateName stateName, BaseState state)
    {
        if(!states.ContainsKey(stateName))
        {
            states.Add(stateName, state);
        }
    }

    public BaseState GetState(StateName stateName)
    {
        if (states.TryGetValue(stateName, out BaseState state))
        {
            return state;
        }
        return null;
    }

    public void DeleteState(StateName stateName)
    {
        if(states.ContainsKey(stateName))
        {
            states.Remove(stateName);
        }
    }

    public void ChangeState(StateName stateName)
    {
        CurrentState?.OnExitState(); // ���� ���� ����
        CurrentState = GetState(stateName);
        CurrentState?.OnEnterState(); // ���� ���� ����
    }

    public void UpdateState()
    {
        CurrentState?.OnUpdateState();
    }

    public void FixedUpdateState()
    {
        CurrentState?.OnFixedUpdateState();
    }
}
