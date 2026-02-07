using UnityEngine;

public class CharacterVisual : MonoBehaviour
{

    private GameObject model;

    private MeshRenderer meshRenderer;

    private GameObject meshCenterOffsetObject;

    public Vector3 MeshCenter
    {
        get
        {
            return meshRenderer.bounds.center + meshCenterOffsetObject.transform.localPosition;
        }
    }

    public void SetupCharacterVisual(CharacterVisualData data)
    {
        model = Instantiate(data.model, transform);
        meshRenderer = model.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = model.GetComponentInChildren<MeshRenderer>();
        }

        meshCenterOffsetObject = new GameObject("Mesh Center Offset");
        meshCenterOffsetObject.transform.parent = model.transform;
        meshCenterOffsetObject.transform.localPosition = data.meshCenterOffset;
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
