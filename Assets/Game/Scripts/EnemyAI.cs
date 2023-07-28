using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum eAiAnims
{
    WALK,
    EMERGE,
    IDLE,
    RUN,
    ATTACK
}

public enum eAiType
{
    EASY,
    MEDUIUM,
    HARD,
}

public enum eAIBehavior
{
    IDLE,
    WANDER,
    ATTACK,
    FOLLOW_PLAYER
}

public interface IEnemyAI
{
    public void TakeDecision();
    public void ExecuteDecision();

}

public struct AiAnimationInfos
{
    public eAiAnims State;
    public string AnimationName;
}


public class EnemyAI : MonoBehaviour, IEnemyAI
{
    public NavMeshAgent Agent;
    public GameObject PlayerObj;
    eAIBehavior CurrentBehavior = eAIBehavior.FOLLOW_PLAYER;
    public eAiAnims CurrentAnimState;
    public Animator Animator;
    public List<AiAnimationInfos> AnimInfos;
    private Dictionary<eAiAnims, string> AnimToStringMap;

    public string GetAnimationName(eAiAnims state)
    {
        string toReturn = null;

        if (AnimToStringMap.TryGetValue(state, out toReturn))
        {
            return toReturn;
        }

        return toReturn;
    }

    public void ChangeAnimationState(eAiAnims newState)
    {
        if (CurrentAnimState == newState)
        {
            return;
        }

        string AnimationState = GetAnimationName(newState);

        if (AnimationState != null)
        {
            CurrentAnimState = newState;
            Animator.Play(AnimationState);
        }
        else
        {
            Debug.LogError("Trying play an invalid animation");
        }

    }

    public void ExecuteDecision()
    {
        switch(CurrentBehavior)
        {
            case eAIBehavior.ATTACK:
                {
                    break;
                }
            case eAIBehavior.FOLLOW_PLAYER: 
                {
                    Agent.destination = PlayerObj.transform.position;

                    break;
                }
        }
    }

    public void TakeDecision()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ExecuteDecision();
    }
}
