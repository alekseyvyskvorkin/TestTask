using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MainBall : MonoBehaviour
{
    private const float TweenDuration = 0.2f;

    public Ball CurrentBall { get; private set; }

    private float _linePositionY => transform.position.y - (_size / 2) + 0.01f;
    private float _size => transform.localScale.x;
    private Vector3 _shootDirection => _door.transform.position - _shootBallPosition;
    private Vector3 _changeScale => Vector3.one * Time.deltaTime * _changeScalePerSecond;
    private Vector3 _shootBallPosition => new Vector3(transform.position.x, transform.position.y, transform.position.z + _size / 2);

    [SerializeField] private Ball[] _balls;

    [SerializeField] private float _startSize = 2f;
    [SerializeField] private float _loseSize = 0.25f;
    [SerializeField] private float _minBallSize = 0.2f;
    [SerializeField] private float _changeScalePerSecond = 0.3f;

    [SerializeField] private LineRenderer _line;

    [SerializeField] private Transform _createBallPosition;

    private Door _door;

    private void Awake()
    {
        _door = FindObjectOfType<Door>();

        InitBall();
        InitLine();
    }

    public void CreateBall()
    {
        if (transform.localScale.x <= _loseSize)
        {
            CurrentBall = null;
            return;
        }

        foreach (var ball in _balls)
        {
            if (ball.gameObject.activeInHierarchy == false)
            {
                ball.ShootDirection = _shootDirection;
                ball.gameObject.SetActive(true);
                ball.transform.localScale = Vector3.zero;
                ball.transform.position = _createBallPosition.position;
                CurrentBall = ball;
                StartCoroutine(CChangeBallScale());
                break;
            }
        }
    }

    public void OnCompleteTouch()
    {
        StopAllCoroutines();

        if (CurrentBall != null && CurrentBall.transform.localScale.x < _minBallSize)
        {
            Vector3 notEnoughtSize = Vector3.one * (_minBallSize - CurrentBall.transform.localScale.x);
            CurrentBall.transform.DOScale(CurrentBall.transform.localScale + notEnoughtSize, TweenDuration);
            transform.DOScale(transform.localScale - notEnoughtSize, TweenDuration).OnComplete(() => 
            {
                if (IsLoseLevel() == true)
                {
                    FindObjectOfType<InputController>().OnLoseLevel();
                }
            });
        }

        if (CurrentBall != null)
        {
            CurrentBall.transform.DOMove(_shootBallPosition, TweenDuration).OnComplete(() =>
            {
                CurrentBall.Shoot();
                CurrentBall = null;
            });            
        }        
    }

    public bool IsLoseLevel()
    {
        if (transform.localScale.x <= _loseSize)
            return true;

        return false;
    }

    private void InitBall()
    {
        Ray ray = Camera.main.ScreenPointToRay(Vector3.zero);
        Physics.Raycast(ray, out RaycastHit hit, 1000f);

        transform.position = hit.point;
        transform.localScale = Vector3.one * _startSize;
        transform.LookAt(_door.transform.position);
        
        Vector3 offset = (Vector3.forward * (_size / 4)) + (Vector3.right * (_size / 2 * 1.1f)) + (Vector3.up * (_size / 2));
        transform.position += offset;
    }

    private void InitLine()
    {
        _line.SetPosition(0, new Vector3(transform.position.x, _linePositionY, transform.position.z + _size / 2));
        _line.SetPosition(1, new Vector3(_door.transform.position.x, _linePositionY, _door.transform.position.z));
    }

    private IEnumerator CChangeBallScale()
    {
        while (transform.localScale.x > _loseSize)
        {
            transform.localScale -= _changeScale;
            CurrentBall.transform.localScale += _changeScale;

            yield return null;
        }
    }
}