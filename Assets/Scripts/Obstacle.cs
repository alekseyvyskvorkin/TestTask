using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private ParticleSystem _ps;
    [SerializeField] private int _destroyTime = 500;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public async Task DestroyObstacle()
    {
        _meshRenderer.enabled = false;
        _ps.Play();
        await Task.Delay(_destroyTime);
        gameObject.SetActive(false);
    }
}
