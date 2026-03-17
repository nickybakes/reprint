using UnityEngine;

public class CharacterVisual : MonoBehaviour
{

    [SerializeField] private Material unselectedOutlineMaterial;
    [SerializeField] private Material selectedOutlineMaterial;

    private GameObject model;
    private MeshRenderer selectionOutlineMeshRenderer;

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

        GameObject selectionOutlineObject = Instantiate(data.model, transform);
        selectionOutlineMeshRenderer = selectionOutlineObject.GetComponent<MeshRenderer>();
        if (selectionOutlineMeshRenderer == null)
        {
            selectionOutlineMeshRenderer = selectionOutlineObject.GetComponentInChildren<MeshRenderer>();
        }

        selectionOutlineMeshRenderer.material = unselectedOutlineMaterial;
        selectionOutlineMeshRenderer.gameObject.SetActive(false);
    }

    public void ShowSelectable()
    {
        ShowUnselected();
        selectionOutlineMeshRenderer.gameObject.SetActive(true);
    }

    public void HideSelectable()
    {
        selectionOutlineMeshRenderer.gameObject.SetActive(false);
    }

    public void ShowUnselected()
    {
        selectionOutlineMeshRenderer.material = unselectedOutlineMaterial;
    }

    public void ShowSelected()
    {
        selectionOutlineMeshRenderer.material = selectedOutlineMaterial;
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
