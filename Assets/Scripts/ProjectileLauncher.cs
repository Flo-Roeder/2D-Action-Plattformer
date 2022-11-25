using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform launchPoint;

    public void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, projectilePrefab.transform.rotation);
        Vector3 originScale= projectile.transform.localScale;
        projectile.transform.localScale = new Vector3(transform.localScale.x, originScale.y, originScale.z);
    }
}
