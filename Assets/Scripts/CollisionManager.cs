using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager Instance { get; private set; }

    public PlayerCollider player { get; private set; }
    private List<AABBCollider> grounds = new List<AABBCollider>();
    private List<ObstacleCollider> obstacles = new List<ObstacleCollider>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterPlayer(PlayerCollider p) { player = p; }
    public void UnregisterPlayer(PlayerCollider p) { if (player == p) player = null; }

    public void RegisterGround(AABBCollider g) { if (!grounds.Contains(g)) grounds.Add(g); }
    public void UnregisterGround(AABBCollider g) { if (grounds.Contains(g)) grounds.Remove(g); }

    public void RegisterObstacle(ObstacleCollider o) { if (!obstacles.Contains(o)) obstacles.Add(o); }
    public void UnregisterObstacle(ObstacleCollider o) { if (obstacles.Contains(o)) obstacles.Remove(o); }

    private void FixedUpdate()
    {
        if (player == null) return;

        foreach (var ground in grounds) //repeat reading for several grounds
        {
            if (Overlaps(player.min, player.max, ground.min, ground.max))
            {
                player.OnGroundCollision(ground);
            }
        }

        foreach (var obstacle in obstacles) //repeat reading for several colliders
        {
            if (Overlaps(player.min, player.max, obstacle.min, obstacle.max))
            {
                player.OnObstacleCollision(obstacle);
            }
        }
    }
    
    private bool Overlaps(Vector2 aMin, Vector2 aMax, Vector2 bMin, Vector2 bMax)
    {
        return aMin.x <= bMax.x && bMin.x <= aMax.x &&
               aMin.y <= bMax.y && bMin.y <= aMax.y;
    }
}