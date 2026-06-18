using UnityEngine;

public class Mousemovement : MonoBehaviour
{
    [SerializeField] float mousepositionx;
    [SerializeField] float mousepositiony;

    // Update is called once per frame
    void Update()
    {
        mousepositionx =  (Input.mousePosition.x / Screen.width) - 0.5f; // Reduces the mouvement possible done with the mouse
        mousepositiony =  (Input.mousePosition.y / Screen.height) - 0.5f;
        
        gameObject.transform.position = new Vector3(mousepositionx, mousepositiony, 0); // Moves the gameObject so that the camera has something to track
    }
}
