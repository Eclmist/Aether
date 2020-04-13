using System.Collections;
using UnityEngine;

public class ExplodableObject : MonoBehaviour
{
    private float m_BombExplosionDelay = 3f;
    private float m_ExplosionRadius = 30.0f;

    private void Start()
    {
        StartCoroutine("WaitForBombDelay");

    }

    IEnumerator WaitForBombDelay()
    {
        yield return new WaitForSeconds(m_BombExplosionDelay);
        Explode();
        Destroy(this.gameObject);
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius);
        foreach (Collider c in colliders)
        {
            if (c.CompareTag("Player"))
            {
                Vector3 finalPosition = c.transform.position + (c.transform.position - this.gameObject.transform.position).normalized * 2;
                finalPosition.y = c.transform.position.y;
                c.transform.position = Vector3.Lerp(c.transform.position, finalPosition, 0.6f);
                PlayerAnimation playerAnimation = c.GetComponent<PlayerAnimation>();
                if (playerAnimation != null)
                {
                    // TODO: Handle animations in a more scalable way
                    //playerAnimation.MakePlayerFall();
                }
            }
        }
    }
}
