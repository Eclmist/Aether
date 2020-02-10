using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class AiPlayer : AiActor
{
    public ThirdPersonCharacter m_ThirdPersonCharacter;
    public void Update()
    {
        m_ThirdPersonCharacter.Move(m_Agent.desiredVelocity, false, false);
    }
}
