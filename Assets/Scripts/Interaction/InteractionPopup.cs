using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class InteractionPopup : MonoBehaviour
{
    [SerializeField] private GameObject _popupPanel;
    [SerializeField] private TextMeshProUGUI _promptText;
    [SerializeField] private TextMeshProUGUI _keyText; // menampilkan "[E]"
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("Animation")]
    [SerializeField] private float _fadeDuration = 0.2f;

    private Coroutine _fadeCoroutine;

    private void Awake()
    {
        _popupPanel.SetActive(false);
        _canvasGroup.alpha = 0f;
    }

    public void ShowPopup(string prompt)
    {
        _promptText.text = prompt;
        if (_keyText != null) _keyText.text = "[E]";

        _popupPanel.SetActive(true);

        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(Fade(0f, 1f));
    }

    public void HidePopup()
    {
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(FadeAndDisable());
    }

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / _fadeDuration);
            yield return null;
        }
        _canvasGroup.alpha = to;
    }

    private IEnumerator FadeAndDisable()
    {
        yield return Fade(1f, 0f);
        _popupPanel.SetActive(false);
    }
}