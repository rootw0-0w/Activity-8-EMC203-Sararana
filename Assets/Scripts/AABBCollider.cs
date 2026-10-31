using UnityEngine;

public class AABBCollider : MonoBehaviour
{
    [SerializeField] private float _length = 1f;
    [SerializeField] private float _width = 1f;

    public Vector2 max { get; private set; }
    public Vector2 min { get; private set; }

    private void Start()
    {
        if (CollisionManager.Instance != null)
            CollisionManager.Instance.RegisterGround(this);
    }
    
    private void OnDestroy()
    {
        if (CollisionManager.Instance != null)
            CollisionManager.Instance.UnregisterGround(this);
    }

    private void Update()
    {
        var posX = transform.position.x + _width * .5f;
        var posY = transform.position.y + _length * .5f;
        var negX = transform.position.x - _width * .5f;
        var negY = transform.position.y - _length * .5f;

        max = new Vector2(posX, posY);
        min = new Vector2(negX, negY);
    }
}