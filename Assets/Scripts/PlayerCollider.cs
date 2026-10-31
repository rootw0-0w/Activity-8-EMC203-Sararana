using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField] private float _length = 1f;
    [SerializeField] private float _width = 1f;
    
    private float speed = 8f;
    private float jumpForce = 10f;
    public float gravity = -20f;
    
    private bool isGrounded = false; 
    private float groundedTimer = 0f; 
    private float verticalVelocity = 0f;

    public Vector2 max { get; private set; }
    public Vector2 min { get; private set; }

    private void Start()
    {
        if (CollisionManager.Instance != null)
            CollisionManager.Instance.RegisterPlayer(this);
    }
    
    private void OnDestroy()
    {
        if (CollisionManager.Instance != null)
            CollisionManager.Instance.UnregisterPlayer(this);
    }

    private void Update()
    {
        var posX = transform.position.x + _width * .5f;
        var posY = transform.position.y + _length * .5f;
        var negX = transform.position.x - _width * .5f;
        var negY = transform.position.y - _length * .5f;

        max = new Vector2(posX, posY);
        min = new Vector2(negX, negY);

        Debug.DrawLine(new Vector3(min.x, max.y), new Vector3(max.x, max.y), Color.yellow);
        Debug.DrawLine(new Vector3(max.x, max.y), new Vector3(max.x, min.y), Color.yellow);
        Debug.DrawLine(new Vector3(max.x, min.y), new Vector3(min.x, min.y), Color.yellow);
        Debug.DrawLine(new Vector3(min.x, min.y), new Vector3(min.x, max.y), Color.yellow);

        if (transform.position.y < -10f) //when y goes lower=falling so reset
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (isGrounded) //timing ground to avoid repeated entry from update
        {
            groundedTimer -= Time.deltaTime;
            if (groundedTimer <= 0f) isGrounded = false;
        }

        Move();
    }

    public void OnGroundCollision(AABBCollider other) //stop gravity when touched
    {
        if (other.max.y <= this.min.y + 0.2f) 
        {
            isGrounded = true;
            groundedTimer = 0.1f; 
            verticalVelocity = 0f;

            float playerHeight = this.max.y - this.min.y;
            float distanceToBottom = playerHeight / 2f;
            transform.position = new Vector2(transform.position.x, other.max.y + distanceToBottom);
        }
    }

    public void OnObstacleCollision(ObstacleCollider other) //stop when obstacle
    {
        float overlapX = Mathf.Min(this.max.x, other.max.x) - Mathf.Max(this.min.x, other.min.x);
        float overlapY = Mathf.Min(this.max.y, other.max.y) - Mathf.Max(this.min.y, other.min.y);

        if (overlapX < overlapY) //push player from obstacle to stop it but still move
        {
            float pushDir = (this.transform.position.x < other.transform.position.x) ? -1f : 1f;
            this.transform.position += new Vector3(overlapX * pushDir, 0, 0);
        }
        else
        {
            float pushDir = (this.transform.position.y < other.transform.position.y) ? -1f : 1f;
            this.transform.position += new Vector3(0, overlapY * pushDir, 0);
        }
    }

    void Move() 
    { 
        float horizontalInput = Input.GetAxis("Horizontal"); 
        transform.Translate(Vector2.right * Time.deltaTime * speed * horizontalInput);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true) //Jump use space and no falling thru ground
        {
            verticalVelocity = jumpForce; 
            isGrounded = false; 
            groundedTimer = 0f; 
        }

        if (!isGrounded) //gravity
        {
            verticalVelocity += gravity * Time.deltaTime; 
            transform.Translate(Vector2.up * verticalVelocity * Time.deltaTime);
        }
    } 
}