using System;
using Unity.Cinemachine;
using UnityEngine;

public class ZoomZone : MonoBehaviour
{
    [SerializeField] GameObject Cinemachine;
    [SerializeField] float defaultzoom;
    [SerializeField] float biggerzoom;
    float targetzoom;
    CinemachineCamera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        cam = Cinemachine.GetComponent<CinemachineCamera>();
        targetzoom = defaultzoom;
    }
    void Update()
    {
        zoomaction();
    }

    void zoomaction()
    {
        cam.Lens.OrthographicSize = Mathf.Lerp(cam.Lens.OrthographicSize, targetzoom, 5f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            targetzoom = biggerzoom;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            targetzoom = defaultzoom;
    }
}
