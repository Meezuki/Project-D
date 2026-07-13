using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem instance;
    public static TooltipSystem Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("TooltipSystem");
                instance = go.AddComponent<TooltipSystem>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    private GameObject tooltipObject;
    private RectTransform rectTransform;
    private TMP_Text headerText;
    private TMP_Text contentText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        CreateTooltipUI();
    }

    private void CreateTooltipUI()
    {
        // Create Canvas root for Tooltip
        GameObject canvasObj = new GameObject("TooltipCanvas");
        canvasObj.transform.SetParent(this.transform, false);
        
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999; // Ensure it renders on top of everything

        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        // Create Panel Object
        tooltipObject = new GameObject("TooltipPanel");
        tooltipObject.transform.SetParent(canvasObj.transform, false);
        tooltipObject.SetActive(false);

        rectTransform = tooltipObject.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(250, 100);
        rectTransform.pivot = new Vector2(0f, 0f); // Bottom-left pivot

        // Add Background Image
        Image bgImage = tooltipObject.AddComponent<Image>();
        bgImage.color = new Color(0.12f, 0.12f, 0.12f, 0.95f);

        // Content Size Fitter
        ContentSizeFitter fitter = tooltipObject.AddComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // Vertical Layout Group
        VerticalLayoutGroup layout = tooltipObject.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(12, 12, 12, 12);
        layout.spacing = 6;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        LayoutElement rootLayout = tooltipObject.AddComponent<LayoutElement>();
        rootLayout.preferredWidth = 260;

        // Header Text
        GameObject headerObj = new GameObject("Header");
        headerObj.transform.SetParent(tooltipObject.transform, false);
        headerText = headerObj.AddComponent<TextMeshProUGUI>();
        headerText.fontSize = 16;
        headerText.fontStyle = FontStyles.Bold;
        headerText.color = new Color(1f, 0.84f, 0f); // Gold/Yellow
        headerText.text = "";

        // Content Text
        GameObject contentObj = new GameObject("Content");
        contentObj.transform.SetParent(tooltipObject.transform, false);
        contentText = contentObj.AddComponent<TextMeshProUGUI>();
        contentText.fontSize = 14;
        contentText.color = Color.white;
        contentText.text = "";

        // CanvasGroup to prevent raycasting
        CanvasGroup canvasGroup = tooltipObject.AddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    public void Show(string content, string header = "")
    {
        if (tooltipObject == null) return;

        tooltipObject.SetActive(true);

        if (string.IsNullOrEmpty(header))
        {
            headerText.gameObject.SetActive(false);
        }
        else
        {
            headerText.gameObject.SetActive(true);
            headerText.text = header;
        }

        contentText.text = content;
        
        Canvas.ForceUpdateCanvases();
        UpdatePosition();
    }

    public void Hide()
    {
        if (tooltipObject != null)
        {
            tooltipObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (tooltipObject != null && tooltipObject.activeSelf)
        {
            UpdatePosition();
        }
    }

    private void UpdatePosition()
    {
        Vector2 mousePos = Input.mousePosition;
        float offsetX = 15f;
        float offsetY = 15f;
        
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        if (mousePos.x + width + offsetX > screenWidth)
        {
            offsetX = -width - 15f;
        }
        if (mousePos.y + height + offsetY > screenHeight)
        {
            offsetY = -height - 15f;
        }

        rectTransform.position = mousePos + new Vector2(offsetX, offsetY);
    }
}
