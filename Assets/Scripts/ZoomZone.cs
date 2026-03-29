using Unity.Cinemachine;
using UnityEngine;

public class ZoomZone : MonoBehaviour
{
    [SerializeField] GameObject Cinemachine;
    [SerializeField] float defaultzoom;
    CinemachineCamera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        cam = Cinemachine.GetComponent<CinemachineCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            cam.Lens.OrthographicSize = 5f;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            cam.Lens.OrthographicSize = defaultzoom;
    }
}
