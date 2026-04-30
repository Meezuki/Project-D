using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [Header("UI References")]
    public Image fadeImage;
    public float fadeDuration = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Transisi saat game pertama kali dibuka
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0, 0, 0, 1);
        fadeImage.DOFade(0f, fadeDuration).OnComplete(() => fadeImage.gameObject.SetActive(false));
    }

    // Fungsi ini yang akan kita panggil dari tombol UI atau script lain
    public void LoadScene(string sceneName)
    {
        StartCoroutine(TransitionCoroutine(sceneName));
    }

    private IEnumerator TransitionCoroutine(string sceneName)
    {
        // 1. Mulai memudar ke hitam
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(1f, fadeDuration);

        // 2. Tunggu sampai layar benar-benar hitam pekat
        yield return new WaitForSeconds(fadeDuration);

        // 3. Mulai load scene di latar belakang (Asynchronous)
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Tahan di layar hitam selama scene belum 100% termuat
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // 4. Scene sudah siap! Sekarang pudarkan kembali hitamnya ke transparan
        fadeImage.DOFade(0f, fadeDuration).OnComplete(() => fadeImage.gameObject.SetActive(false));
    }
}