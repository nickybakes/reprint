using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private new Camera camera;

    [SerializeField] private Shaker cameraShaker;

    public void Shake(float amount)
    {
        cameraShaker.Shake(amount);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
