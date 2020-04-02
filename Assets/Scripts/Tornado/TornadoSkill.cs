using UnityEngine;
using System.Collections;
using BeardedManStudios.Forge.Networking.Generated;

/**
 * Class facilitates tornado spell forward movement
 * with each frame.
 */
public class TornadoSkill : SkillsBehavior
{
    [SerializeField]
    private float m_Speed = 5f;

    private Vector3 m_CurrentDirection;
    private bool m_IsSpellActivated;

    private void Start()
    {
        StartCoroutine(ToggleActivateSpell());
        m_CurrentDirection = PlayerManager.Instance.GetLocalPlayer().transform.forward.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (networkObject == null)
            return;

        if (networkObject.IsOwner)
        {
            if (m_IsSpellActivated)
            {
                transform.position += m_CurrentDirection * Time.deltaTime * m_Speed;
                networkObject.position = transform.position;
            }
        }
        else
        {
            transform.position = networkObject.position;
        }
    }

    private IEnumerator ToggleActivateSpell()
    {
        yield return new WaitForSeconds(1.1f);
        m_IsSpellActivated = true;
    }
}
