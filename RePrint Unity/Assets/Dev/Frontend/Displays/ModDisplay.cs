using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModDisplay : Display
{

    public FloatingDraggableDisplay FloatingDraggableDisplay { get; set; }

    [SerializeField] private ModTooltipDisplay tooltip;

    [SerializeField] private Image backgroundImage;

    [SerializeField] private Image iconImage;

    private BetterSelectable selectable;

    private Mod mod;

    /// <summary>
    /// Sets up the rect transform and travel data.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
        selectable = GetComponent<BetterSelectable>();
        FloatingDraggableDisplay = GetComponent<FloatingDraggableDisplay>();

        if (selectable)
        {
            selectable.SelectEvent.AddListener(Select);
            selectable.DeselectEvent.AddListener(Deselect);
        }
    }

    public void Setup(Mod _mod)
    {
        mod = _mod;

        Color color = UIView.view.RarityDirectory.GetColor(_mod.Rarity);

        backgroundImage.color = color;
        iconImage.sprite = mod.Profile.Icon;

        if (tooltip)
        {
            tooltip.Setup(mod);
        }
    }

    private void Select(int index)
    {
        if (tooltip)
        {
            tooltip.Show();
        }
    }

    private void Deselect(int index)
    {
        if (tooltip)
        {
            tooltip.Hide();
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }
}