using UnityEngine;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider), typeof(MeshRenderer))]
public class Ball : MonoBehaviour
{
    public Vector3 ShootDirection { get; set; }

    private float _size => transform.localScale.x;

    [SerializeField] private ParticleSystem _ps;

    [SerializeField] private float _explosionColliderRadius = 1f;
    [SerializeField] private float _colliderRadiusSpeedChange = 2f;
    [SerializeField] private float _shootForce = 1.5f;   

    private Rigidbody _rb;
    private SphereCollider _collider;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<SphereCollider>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        _rb.isKinematic = true;
        _collider.isTrigger = false;
        _collider.radius = 0.5f;
        _meshRenderer.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Obstacle>(out var obstacle))
        {
            OnContactWithObstacle();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<Obstacle>(out var obstacle))
        {
            obstacle.DestroyObstacle();
        }

        if (collider.TryGetComponent<Door>(out var door))
        {
            door.Open();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent<Door>(out var door))
        {
            gameObject.SetActive(false);
        }
    }

    public void Shoot()
    {
        _rb.isKinematic = false;
        _rb.velocity = ShootDirection * _size * _shootForce;
    }

    private async Task OnContactWithObstacle()
    {
        _rb.isKinematic = true;
        _collider.isTrigger = true;
        _meshRenderer.enabled = false;
        _ps.Play();

        while (_collider.radius < _explosionColliderRadius)
        {
            _collider.radius += Time.deltaTime * _colliderRadiusSpeedChange;
            await UniTask.Yield();
        }

        gameObject.SetActive(false);
    }
}
