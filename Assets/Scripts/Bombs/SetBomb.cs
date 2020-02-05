using UnityEngine;
using UnityEngine.InputSystem;

public class SetBomb : MonoBehaviour
{
    [SerializeField]
    private GameObject m_BombPrefab;

    private void Start()
    {
        AetherInput.GetPlayerActions().SetBomb.performed += HandleSetBomb;
    }

    private void HandleSetBomb(InputAction.CallbackContext context)
    {
        Instantiate(m_BombPrefab, transform.position, transform.rotation);
    }
}
