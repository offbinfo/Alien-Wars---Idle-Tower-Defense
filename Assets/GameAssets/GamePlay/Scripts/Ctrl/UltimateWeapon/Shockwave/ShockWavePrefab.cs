using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWavePrefab : MonoBehaviour
{
    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

}
