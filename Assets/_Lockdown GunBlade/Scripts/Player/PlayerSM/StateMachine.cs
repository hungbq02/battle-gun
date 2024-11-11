
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState currentState;


    void Start()
    {
        currentState = GetInitialState();
        currentState?.Enter();
    }
    void Update()
    {
        currentState?.HandleInput();
        currentState?.UpdateLogic();
    }

    void LateUpdate()
    {
        currentState?.UpdatePhysics();
    }

    protected virtual BaseState GetInitialState()
    {
        return null;
    }

    public void ChangeState(BaseState newState)
    {
        currentState?.Exit();

        currentState = newState;
        newState.Enter();
    }
/*    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10f, 10f, 200f, 100f));
        string content = currentState != null ? currentState.ToString() : "(no current state)";
        GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
        GUILayout.EndArea();
    }*/

}
