using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public eAnimState CurrentAnimState = eAnimState.IDLE;

    public List<AnimationInfos> AnimInfos;
    private Dictionary<eAnimState, string> AnimToStringMap;

    private Animator Animator;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
