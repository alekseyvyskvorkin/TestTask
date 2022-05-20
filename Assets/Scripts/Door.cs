using UnityEngine;
using Cysharp.Threading.Tasks;

public class Door : MonoBehaviour
{
    private const string OpenAnimation = "Open";

    [SerializeField] private ParticleSystem _confeti;

    private Animator _animator;

    private UIService _service;

    private void Awake()
    {
        _service = FindObjectOfType<UIService>();
        _animator = GetComponent<Animator>();
    }

    public async void Open()
    {
        _animator.SetTrigger(OpenAnimation);
        _confeti.Play();
        await UniTask.Delay(1000);
        Time.timeScale = 0f;
        _service.ShowPanel(_service.WinPanel);
    }
}