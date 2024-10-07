using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour
{
    [SerializeField] int _fps;
    private void Start()
    {
        Application.targetFrameRate = _fps;
    }

}
