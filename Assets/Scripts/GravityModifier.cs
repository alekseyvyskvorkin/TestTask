using UnityEngine;

public class GravityModifier : MonoBehaviour
{
    [SerializeField] private float _gravityForce = 9.81f;

    private void Start()
    {
        Physics.gravity = Vector3.down * _gravityForce;
    }
}
