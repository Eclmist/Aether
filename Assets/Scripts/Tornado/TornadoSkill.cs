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
    private float m_Speed = 15f;

    private Vector3 m_CurrentDirection;

    private void Start()
    {
        m_CurrentDirection = PlayerManager.Instance.GetLocalPlayer().transform.forward.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(MoveTornado());
    }

    private IEnumerator MoveTornado()
    {
        yield return new WaitForSeconds(0.9f);
        if (networkObject != null)
        {
            if (!networkObject.IsOwner)
            {
                transform.position = networkObject.position;
                yield return null;
            }

            // Called by owner of tornado spell
            transform.position += m_CurrentDirection * Time.deltaTime * m_Speed;
            networkObject.position = transform.position;
            yield return null;
        }
    }
}
