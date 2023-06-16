using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

internal sealed class ResetScene : MonoBehaviour
{
    public void SceneReset()
    {
        DOTween.KillAll();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
