using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AABBCollider : MonoBehaviour
{
    [SerializeField] private float _length;
    [SerializeField] private float _width;

    public Vector2 max { get; private set; }
    public Vector2 min { get; private set; }

    private void Start()
    {
        if (CollisionManager.Instance != null)
        {
            CollisionManager.Instance.Register(this);
        }
    }
    
    private void OnDestroy()
    {
        if (CollisionManager.Instance != null)
        {
            CollisionManager.Instance.Unregister(this);
        }
    }


    private void Update()
    {
        var posX = this.transform.position.x + _width * .5f;
        var posY = this.transform.position.y + _length * .5f;
        
        var negX = this.transform.position.x - _width * .5f;
        var negY = this.transform.position.y - _length * .5f;

        max = new Vector2(posX, posY);
        min = new Vector2(negX, negY);
        
        Debug.DrawLine(new Vector3(min.x, max.y), new Vector3(max.x, max.y), Color.yellow);
        Debug.DrawLine(new Vector3(max.x, max.y), new Vector3(max.x, min.y), Color.yellow);
        Debug.DrawLine(new Vector3(max.x, min.y), new Vector3(min.x, min.y), Color.yellow);
        Debug.DrawLine(new Vector3(min.x, min.y), new Vector3(min.x, max.y), Color.yellow);
    }

    public void OnCollision(AABBCollider other)
    {
        Debug.Log($"Hit {other.name}");
    }
}