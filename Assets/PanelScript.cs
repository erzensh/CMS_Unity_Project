using UnityEngine;
using UnityEngine.UI;

public class PanelScript : MonoBehaviour
{
    public Canvas canvas; // Drag your Canvas GameObject here in the Unity Editor
    public Text nameText;
    public Text descriptionText;
    public Text urlText;

    public void UpdatePanelData(string name, string description, string url)
    {
        // Update the UI elements with the provided data
        nameText.text = "Name: " + name;
        descriptionText.text = "Description: " + description;
        urlText.text = "URL: " + url;
    }
    public void CreatePanel(string name, string description, string url)
    {
        GameObject panel = new GameObject("Panel");
        panel.AddComponent<CanvasRenderer>();

        RectTransform rectTransform = panel.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(300, 150);

        Image image = panel.AddComponent<Image>();
        image.color = new Color(1, 1, 1, 0.8f);

        Text nameText = CreateText(panel.transform, $"Name: {name}", 16, TextAnchor.UpperLeft, new Vector2(10, -10));
        Text descriptionText = CreateText(panel.transform, $"Description: {description}", 14, TextAnchor.UpperLeft, new Vector2(10, -40));
        Text urlText = CreateText(panel.transform, $"URL: {url}", 14, TextAnchor.UpperLeft, new Vector2(10, -70));

        panel.transform.SetParent(canvas.transform, false);
    }

    private Text CreateText(Transform parent, string content, int fontSize, TextAnchor anchor, Vector2 anchoredPosition)
    {
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(parent, false);

        Text textComponent = textObject.AddComponent<Text>();
        textComponent.text = content;
        textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        textComponent.fontSize = fontSize;
        textComponent.alignment = anchor;

        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchoredPosition = anchoredPosition;

        return textComponent;
    }
}
