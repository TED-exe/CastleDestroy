using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictShootTrajectory : MonoBehaviour
{
    private LineRenderer trajectoryLine;
    [Range(1, 50)]
    [SerializeReference] private int linePoint = 25;
    [Range(0.01f, 1f)]
    [SerializeField] private float timeBetweenPoint = 0.2f;
    [SerializeField] private Transform hitMarker;

    private void Awake()
    {
        trajectoryLine = GetComponent<LineRenderer>();
    }
    public void DrawProjection(Transform bulletSpawnPlace, float currShotPower, float bulletMass)
    {
        trajectoryLine.enabled = true;
        trajectoryLine.positionCount = Mathf.CeilToInt(linePoint / timeBetweenPoint) + 1;

        Vector3 startPosition = bulletSpawnPlace.position;
        Vector3 startVelocity = currShotPower * bulletSpawnPlace.up / bulletMass;
        int i = 0;
        trajectoryLine.SetPosition(i, startPosition);
        for (float time = 0; time < linePoint; time += timeBetweenPoint)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2 * time * time);

            trajectoryLine.SetPosition(i, point);

            Vector3 lastPosition = trajectoryLine.GetPosition(i - 1);

            if (Physics.Raycast(lastPosition, (point - lastPosition).normalized, out RaycastHit hit, (point - lastPosition).magnitude))
            {
                trajectoryLine.SetPosition(i, hit.point);
                trajectoryLine.positionCount = i + 1;
                HitMarkUpdate(hit);
                break;
            }
            hitMarker.gameObject.SetActive(false);
        }
    }
    public void OffLinereRendererAndMark()
    {
        trajectoryLine.enabled = false;
        hitMarker.gameObject.SetActive(false);
    }
    public void OnLinereRendererAndMark()
    {
        trajectoryLine.enabled = true;
        hitMarker.gameObject.SetActive(true);
    }
    private void HitMarkUpdate(RaycastHit hit)
    {
        float offset = 0.025f;
        hitMarker.gameObject.SetActive(true);

        Vector3 hitNormal = hit.normal;
        Vector3 upDirection = Vector3.up; // Może być dowolny kierunek "góra" w przestrzeni, w zależności od potrzeb.
        Vector3 rightDirection = Vector3.Cross(hitNormal, upDirection).normalized;
        Vector3 forwardDirection = Vector3.Cross(rightDirection, hitNormal).normalized;

        Quaternion rotation = Quaternion.LookRotation(forwardDirection, hitNormal);

        hitMarker.position = hit.point + hitNormal * offset;
        hitMarker.rotation = rotation;
    }
}
