using UnityEngine;
using UnityEngine.SceneManagement;

public class OverlayLoader : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadSceneAsync("Overlay", LoadSceneMode.Additive);
    }
}
