using UnityEngine;
using DG.Tweening;

public class UIService : MonoBehaviour
{
    public CanvasGroup LosePanel => _losePanel;
    public CanvasGroup WinPanel => _winPanel;

    [SerializeField] private float _fadeDuration = 0.3f;

    [SerializeField] private CanvasGroup _winPanel;
    [SerializeField] private CanvasGroup _losePanel;
    [SerializeField] private CanvasGroup _startPanel;

    private void Start()
    {
        _startPanel.gameObject.SetActive(true);
    }

    public void ShowPanel(CanvasGroup group)
    {
        group.gameObject.SetActive(true);
        group.DOFade(1, _fadeDuration).SetUpdate(true);
    }

    public void HidePanel(CanvasGroup group)
    {
        group.DOFade(0, _fadeDuration).OnComplete(() => 
        {
            group.gameObject.SetActive(false);
        });
    }
}
