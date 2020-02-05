using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class ProjectileSpawn : MonoBehaviour
{
    // The coordinate where the projectile will spawn.
    public GameObject firePoint;

    // The prefab for the muzzle particle effect when the projectile is shot.
    public GameObject muzzlePrefab;

    // The projectile effect itself.
    public GameObject projectilePrefab;

    // The camera for reference to shoot at crosshair.
    public Camera currentCamera;

    // The rate that the player can shoot the projectile.
    public float firerate;

    // Private GameObject the "store" the vfx.
    private GameObject effectToSpawn;

    // The time when the projectile can be fired according to firerate.
    private float timeToFire = 0;

    void Start()
    {
        AetherInput.GetPlayerActions().Fire.performed += SpawnVFX;

        // "Storing" the vfx into private variable.
        effectToSpawn = projectilePrefab;
    }

    void Update()
    {
    }

    public void SpawnVFX(InputAction.CallbackContext ctx)
    {
        ButtonControl button = ctx.control as ButtonControl;

        // If button was not pressed, exit the function and do not spawn the VFX.
        if (!button.wasPressedThisFrame)
        {
            return;
        }

        if (firePoint != null && Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1 / firerate;

            // Creates the muzzle VFX.
            if (muzzlePrefab != null)
            {
                GameObject muzzleVFX = Instantiate(muzzlePrefab, firePoint.transform.position, Quaternion.identity);
                muzzleVFX.transform.forward = gameObject.transform.forward;
                AudioManager.m_Instance.PlaySoundAtPosition("PROJECTILE_Shoot", firePoint.transform.position);
                ParticleSystem particleSystemMuzzle = muzzleVFX.GetComponent<ParticleSystem>();
                if (particleSystemMuzzle != null)
                {
                    Destroy(muzzleVFX, particleSystemMuzzle.main.duration);
                }
                else
                {
                    ParticleSystem particleSystemMuzzleChild = muzzleVFX.transform.GetComponentInChildren<ParticleSystem>();
                    if (particleSystemMuzzleChild != null)
                    {
                        Destroy(muzzleVFX, particleSystemMuzzleChild.main.duration);
                    }
                }
            } else
            {
                Debug.Log("No Muzzle Prefab");
            }
            GameObject vfx;
            // Creates the projectile.
            vfx = Instantiate(effectToSpawn, firePoint.transform.position, Quaternion.identity);
            vfx.transform.LookAt(currentCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, currentCamera.farClipPlane)));
        }
        else
        {
            Debug.Log("No Fire Point");
        }
    }
}
