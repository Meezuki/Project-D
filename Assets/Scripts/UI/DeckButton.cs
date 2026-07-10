using UnityEngine;
using UnityEngine.UI;

public class DeckButton : MonoBehaviour
{
    private void Start()
    {
        var btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() =>
            {
                if (DeckOverlayUI.Instance != null)
                {
                    DeckOverlayUI.Instance.Show();
                }
            });
        }
    }
}
