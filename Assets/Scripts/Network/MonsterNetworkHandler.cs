using System.Collections;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;

[RequireComponent(typeof(AiMonster), typeof(HealthHandler))]
public class MonsterNetworkHandler : MonsterObjectBehavior
{
    private AiMonster m_aiMonster;
    private HealthHandler m_healthHandler;

    private void Start()
    {
        m_aiMonster = GetComponent<AiMonster>();
        m_healthHandler = GetComponent<HealthHandler>();
        if (m_aiMonster == null || m_healthHandler == null)
        {
            Debug.LogError("No health handler and ai monster attached " + this);
        }

        if (NetworkManager.Instance == null)
        {
            this.enabled = false;
            return;
        }
        else
        {
            if (!NetworkManager.Instance.IsServer)
                m_aiMonster.DisableAttack();
        }
    }

    private void Update()
    {
        if (networkObject != null)
        {
            if (NetworkManager.Instance.IsServer)
            {
                // If we are the owner of the object we should send the new position
                // and rotation across the network for receivers to move to in the above code
                networkObject.position = transform.position;
                networkObject.rotation = transform.rotation;
                networkObject.isDead = m_aiMonster.IsDead;
                networkObject.health = m_healthHandler.GetHealth();

                string attackName = m_aiMonster.GetAttack();
                if (attackName.Length > 0)
                    networkObject.SendRpc(RPC_TRIGGER_ATTACK_ANIM, Receivers.Others, attackName);
            }
            else
            {
                transform.position = networkObject.position;
                transform.rotation = networkObject.rotation;
                if (networkObject.isDead)
                {
                    m_aiMonster.OnDeath();
                }
                m_healthHandler.SetHealth(networkObject.health);
            }
        }
    }

    public override void TriggerAttackAnim(RpcArgs args)
    {
        m_aiMonster.GetMonsterAnimation()?.SetAttackTrigger(args.GetNext<string>());
    }
}
