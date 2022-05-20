using UnityEngine;

public class InputController : MonoBehaviour
{
    private UIService _service;

    private MainBall _mainBall;

    private Touch _touch;

    private void Awake()
    {
        _service = FindObjectOfType<UIService>();
        _mainBall = FindObjectOfType<MainBall>();
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);

            if (_touch.phase == TouchPhase.Began && _mainBall.CurrentBall == null)
            {
                _mainBall.CreateBall();
            }

            if (_touch.phase == TouchPhase.Moved || _touch.phase == TouchPhase.Stationary)
            {
                if (_mainBall.IsLoseLevel() == true)
                {
                    OnLoseLevel();
                }
            }

            if (_touch.phase == TouchPhase.Ended || _touch.phase == TouchPhase.Canceled)
            {
                _mainBall.OnCompleteTouch();
            }
        }
    }

    public void OnLoseLevel()
    {
        _service.ShowPanel(_service.LosePanel);
        Time.timeScale = 0;
        enabled = false;
    }
}
