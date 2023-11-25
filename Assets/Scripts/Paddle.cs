using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Paddle : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }

    public float speed = 7f;

    public bool useDynamicBounce = false;

    // Testing for Tristan
    public GameObject scorePanelIcon;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void ResetPosition()
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.position = new Vector2(rigidbody.position.x, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (useDynamicBounce && collision.gameObject.CompareTag("Ball"))
        {
            //Rigidbody2D ball = collision.rigidbody;
            //Collider2D paddle = collision.otherCollider;

            //// Gather information about the collision
            //Vector2 ballDirection = ball.velocity.normalized;
            //Vector2 contactDistance = ball.transform.position - paddle.bounds.center;
            //Vector2 surfaceNormal = collision.GetContact(0).normal;
            //Vector3 rotationAxis = Vector3.Cross(Vector3.up, surfaceNormal);

            //// Rotate the direction of the ball based on the contact distance
            //// to make the gameplay more dynamic and interesting
            //float maxBounceAngle = 75f;
            //float bounceAngle = (contactDistance.y / paddle.bounds.size.y) * maxBounceAngle;
            //ballDirection = Quaternion.AngleAxis(bounceAngle, rotationAxis) * ballDirection;

            //// Re-apply the new direction to the ball
            //ball.velocity = ballDirection * ball.velocity.magnitude;
        }
    }

}
