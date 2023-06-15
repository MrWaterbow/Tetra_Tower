using UnityEngine;
using UnityEngine.SceneManagement;

internal sealed class ResetScene : MonoBehaviour
{
    public void SceneReset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
