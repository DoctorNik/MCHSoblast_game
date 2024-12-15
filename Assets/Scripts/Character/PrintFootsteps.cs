using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintFootsteps : MonoBehaviour
{
    public GameObject footstepPrefab;
    public GameObject LowfootstepPrefab;
    public float localYOffset = 0.1f; 
    public float raycastDistance = 1.0f; 

    private List<GameObject> footsteps = new List<GameObject>(); 
    public int maxFootsteps = 10; 

    public float fadeDuration = 1.0f; 
    public float delayBeforeFade = 2.0f;

    public float minAngleThreshold;
    public float maxAngleThreshold;

    public bool isLeftStep = true;

    public float stepOffset = 0.1f;
    public bool NotSnow;
    public bool DropSnow;
    public int SteppedWithoutSnow;


    public Vector3 footstepScale = new Vector3(0.8f, 0.8f, 0.8f);
    public void CreateFootstep()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Surface") && !NotSnow)
            {
                float angle = Vector3.Angle(Vector3.up, hit.normal);

                if (maxAngleThreshold > angle && angle > minAngleThreshold || angle == 0)
                {
                    Vector3 footstepPosition = hit.point + Vector3.up * localYOffset;

                    Vector3 offsetDirection = isLeftStep ? -transform.right : transform.right; 
                    footstepPosition += offsetDirection * stepOffset;

                    Quaternion footstepRotation = Quaternion.LookRotation(transform.forward, hit.normal);
                    footstepRotation *= Quaternion.Euler(90f, 0f, 0f);

                    GameObject newFootstep = Instantiate(footstepPrefab, footstepPosition, footstepRotation);

                    newFootstep.transform.localScale = isLeftStep ? footstepScale : new Vector3(-footstepScale.x, footstepScale.y, footstepScale.z);

                    footsteps.Add(newFootstep);
                    StartCoroutine(FadeFootstep(newFootstep));
                    isLeftStep = !isLeftStep;
                }
            }
            else if (hit.collider.CompareTag("Floor") && !DropSnow)
            {
                float angle = Vector3.Angle(Vector3.up, hit.normal);

                if (maxAngleThreshold > angle && angle > minAngleThreshold || angle == 0)
                {
                    Vector3 footstepPosition = hit.point + Vector3.up * localYOffset;

                    Vector3 offsetDirection = isLeftStep ? -transform.right : transform.right; 
                    footstepPosition += offsetDirection * stepOffset;

                    Quaternion footstepRotation = Quaternion.LookRotation(transform.forward, hit.normal);
                    footstepRotation *= Quaternion.Euler(90f, 0f, 0f);

                    GameObject newFootstep = Instantiate(LowfootstepPrefab, footstepPosition, footstepRotation);

                    newFootstep.transform.localScale = isLeftStep ? footstepScale : new Vector3(-footstepScale.x, footstepScale.y, footstepScale.z);

                    footsteps.Add(newFootstep);
                    StartCoroutine(FadeFootstep(newFootstep));
                    isLeftStep = !isLeftStep;
                    IsDroppingSnow();
                }
            }
            else if (hit.collider.CompareTag("Surface") && NotSnow && !DropSnow)
            {
                float angle = Vector3.Angle(Vector3.up, hit.normal);

                if (maxAngleThreshold > angle && angle > minAngleThreshold || angle == 0)
                {
                    Vector3 footstepPosition = hit.point + Vector3.up * localYOffset;

                    Vector3 offsetDirection = isLeftStep ? -transform.right : transform.right;
                    footstepPosition += offsetDirection * stepOffset;

                    Quaternion footstepRotation = Quaternion.LookRotation(transform.forward, hit.normal);
                    footstepRotation *= Quaternion.Euler(90f, 0f, 0f);

                    GameObject newFootstep = Instantiate(LowfootstepPrefab, footstepPosition, footstepRotation);

                    newFootstep.transform.localScale = isLeftStep ? footstepScale : new Vector3(-footstepScale.x, footstepScale.y, footstepScale.z);

                    footsteps.Add(newFootstep);
                    StartCoroutine(FadeFootstep(newFootstep));
                    isLeftStep = !isLeftStep;
                    IsDroppingSnow();
                }
            }
        }
        CheckSteps();
    }
    public void GetSnow()
    {
        DropSnow = false;
        SteppedWithoutSnow = 0;
    }
    public void IsDroppingSnow()
    {
        SteppedWithoutSnow += 1;
        Debug.Log($"{SteppedWithoutSnow}");
        if (SteppedWithoutSnow == 2)
        {
            DropSnow = true;
        }
    }
    public void CheckSteps()
    {
        if (footsteps.Count > maxFootsteps)
        {
            GameObject oldFootstep = footsteps[0];
            footsteps.RemoveAt(0);

            if (oldFootstep != null)
            {
                Destroy(oldFootstep);
            }
        }
    }

    private IEnumerator FadeFootstep(GameObject footstep)
    {
        yield return new WaitForSeconds(delayBeforeFade);

        if(footstep != null)
        {
            Renderer renderer = footstep.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = renderer.material;
                float startAlpha = material.color.a;
                for (float t = 0; t < fadeDuration; t += Time.deltaTime)
                {
                    float normalizedTime = t / fadeDuration;
                    float newAlpha = Mathf.Lerp(startAlpha, 0, normalizedTime); 
                    Color color = material.color;
                    color.a = newAlpha; 
                    material.color = color; 

                    material.SetFloat("_Alpha", newAlpha);
                    yield return null;
                }

                Color finalColor = material.color;
                finalColor.a = 0;
                material.color = finalColor; 
                material.SetFloat("_Alpha", 0); 
            }

            Destroy(footstep);
        }
    }

}
