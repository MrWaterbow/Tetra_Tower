using Lean.Transition.Method;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using Sources.Scenes;


public class LoadingScreenView : MonoBehaviour
{
    [SerializeField] private UnityEvent _showEvent;
    [SerializeField] private UnityEvent _hideEvent;

    [Space]

    [SerializeField] private GameObject _gameObject;
    [SerializeField] private LeanEvent _animation;

    private SceneLoader _sceneLoader;

    [Inject]
    private void Construct(SceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }
    private void Awake()
    {
        DontDestroyOnLoad(_gameObject);
    }
    private void OnEnable()
    {
        _sceneLoader.OnLoading += Show;
        _sceneLoader.OnLoadingComplete += Hide;
    }
    private void OnDisable()
    {
        _sceneLoader.OnLoading -= Show;
        _sceneLoader.OnLoadingComplete -= Hide;
    }
    private void Show(UnityAction _onShowingFinish = null)
    {
        _animation.Data.Event.AddListener(_onShowingFinish);
        _showEvent.Invoke();
    }
    private void Hide()
    {
        _hideEvent.Invoke();
    }
}
