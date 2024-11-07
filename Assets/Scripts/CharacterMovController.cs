using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UEventHandler;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovController : MonoBehaviour
{
    public PlayerInputHandler inputHandler;
    public Rigidbody rb;

    [Header("Float")]
    [Range(0, 2)]
    public float floatHeight = 1.6f;
    public float springStrength = 60f;
    public float dampenStrength = 29f;

    [Header("Ground Check")]
    public float minGroundHeight = 2f;
    public float castDistance = 2f;
    public LayerMask raycastMask = 1;
    public bool isGroundNear;
    public bool isGrounded;
    [SerializeField] private RaycastHit groundHit;
    private List<Collider> groundColliders = new List<Collider>();

    [Header("Move")]
    public float maxAccel = 120f;
    public float maxDeccel = 200f;
    public float goalVelocity = 10f;
    public bool moveOnAir = true;

    [Header("Jump")]
    public float jumpForce = 9.5f;
    public int jumpMaxDurationFrames = 15;
    public float jumpDownForce = 111f;
    public UEvent OnJump = new UEvent();
    [SerializeField] private bool isJumping;
    [SerializeField] private int jumpDurationCounter;

    [Header("Land")]
    public int landingStartCheckFrames = 8;
    public UEvent OnLand = new UEvent();
    [SerializeField] private bool isLanding;
    [SerializeField] private int landStartCheckCounter;

    [Header("Airbourne")]
    public float airDeacFactor = 0f;

    [Header("Stats")]
    public Vector3 vel;
    public Vector3 horizontalVel;
    public float horizontalVelMag;
    public bool isFrozen;

    private bool isMovementFrozen = false; // New boolean to track if movement is frozen
    private UEventHandler eventHandler = new UEventHandler();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // In case the user inputs a float height 
        if (minGroundHeight < floatHeight)
            floatHeight = minGroundHeight;

        if (castDistance < floatHeight * 1.2f)
            castDistance = floatHeight * 1.2f;
    }

    void Start()
    {
        inputHandler.input_jump.Onpressed.Subscribe(eventHandler, Jump);
    }

    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) // Check for B key press
        {
            ToggleMovement();
        }
    }

    private void ToggleMovement()
    {
        isMovementFrozen = !isMovementFrozen; // Toggle the frozen state
        rb.isKinematic = isMovementFrozen; // Set Rigidbody kinematic based on frozen state
    }

    private void FixedUpdate()
    {
        CheckGround();
        CheckStartLanding();
        JumpDownForce();
        CheckEndLanding();
        Float();
        Move();
        AirbourneDecel();
        JumpUpdate();
    }

    private void LateUpdate()
    {
        // Store the Rigidbody velocity at the end of the frame
        vel = rb.velocity;
        horizontalVel = vel;
        horizontalVel.y = 0f;
        horizontalVelMag = horizontalVel.magnitude;
    }

    private void CheckGround()
    {
        // Checks if raycast hits something
        isGroundNear = Physics.Raycast(transform.position, -transform.up, out groundHit, castDistance, raycastMask, QueryTriggerInteraction.Ignore);
        // If it hits something checks if distance is the minimum required
        isGrounded = isGroundNear ? groundHit.distance <= minGroundHeight : false;

        // If raycast do not hit anything  and is  directly contacting the ground
        if ((!isGroundNear || !isGrounded) && groundColliders.Count > 0)
        {
            isGroundNear = true;
            isGrounded = true;
            groundHit.distance = minGroundHeight;
        }
    }

    public float heightHeight = 20f;
    private void Float()
    {
        // Initial validations
        if (!isGroundNear || isJumping) return;

        // Initial parameters
        Vector3 outsideVel = Vector3.zero;

        // If ground object has velocity store it
        if (groundHit.rigidbody != null)
            outsideVel = groundHit.rigidbody.velocity;

        float dirOwnVel = Vector3.Dot(-transform.up, rb.velocity);
        float dirOutsideVel = Vector3.Dot(-transform.up, outsideVel);
        float dirRelation = dirOwnVel - dirOutsideVel;

        float distance = groundHit.distance - floatHeight;

        // Calculate spring force necessary
        float spring = (distance * springStrength) - (dirRelation * dampenStrength);

        // Add float force to character
        rb.AddForce(-transform.up * rb.mass * spring);

        // If ground has rigidbody apply down force to it
        if (outsideVel != Vector3.zero)
        {
            groundHit.rigidbody.AddForceAtPosition(-transform.up * -springStrength, groundHit.point);
        }
    }

    private void Move()
    {
        // Initial validations
        if ((!moveOnAir && !isGrounded) || isFrozen || isMovementFrozen) return; // Add isMovementFrozen check

        // Get input direction and transform it to the camera orientation
        Vector2 move = inputHandler.input_move.value;
        Vector3 transformedMove = inputHandler.playerCamera.transform.TransformDirection(new Vector3(move.x, 0, move.y));
        transformedMove.y = 0;

        // If input is zero goal velocity should be zero too
        float calculatedGoalVelocity = move.magnitude > 0.1f ? goalVelocity : 0;

        // Calculate necessary acceleration to achieve goal velocity
        Vector3 acceleration = (transformedMove * calculatedGoalVelocity - rb.velocity) / Time.fixedDeltaTime;

        // Check if new direction is facing or against current velocity
        float dot = Vector3.Dot(transformedMove, rb.velocity);

        if (dot >= 0 && calculatedGoalVelocity > 0)
            acceleration = Vector3.ClampMagnitude(acceleration, maxAccel); // If positive or zero apply acceleration clamp
        else
            acceleration = Vector3.ClampMagnitude(acceleration, maxDeccel); // If negative apply deceleration clamp

        Vector3 force = rb.mass * acceleration;
        // Make sure Move Force does apply to Y axis
        force.y = 0;

        rb.AddForce(force);
    }

    private void Jump()
    {
        if (!isGroundNear || isJumping || isFrozen) return;

        // Init Jump variables
        isJumping = true;
        landStartCheckCounter = landingStartCheckFrames;
        jumpDurationCounter = jumpMaxDurationFrames;

        // Invoke event to outside listeners
        OnJump.TryInvoke();

        float jumpDif = jumpForce - rb.velocity.y;

        // Add the force
        rb.AddForce(Vector3.up * rb.mass * jumpDif, ForceMode.Impulse);
    }

    private void JumpDownForce()
    {
        if (isGroundNear || (inputHandler.input_jump.value > 0 && jumpDurationCounter > 0) || landStartCheckCounter > 0) return;

        rb.AddForce(Vector3.down * rb.mass * jumpDownForce);
    }

    private void CheckStartLanding()
    {
        if (!isJumping || landStartCheckCounter > 0) return;

        if (rb.velocity.y > 0) return;

        isLanding = true;
        isGrounded = false;
        isJumping = false;
    }

    private void CheckEndLanding()
    {
        if (!isLanding || !isGrounded) return;

        isLanding = false;
        OnLand.TryInvoke();
    }

    private void AirbourneDecel()
    {
        if (isGroundNear || rb.velocity == Vector3.zero) return;
        var force = -rb.velocity * rb.mass * airDeacFactor;
        force.y = 0f;
        rb.AddForce(force);
    }

    private void JumpUpdate()
    {
        if (jumpDurationCounter > 0)
            jumpDurationCounter--;

        if (landStartCheckCounter > 0)
            landStartCheckCounter--;
    }

    public Vector3 getMoveDirection()
    {
        return rb.velocity;
    }

    public void FreezePlayer(bool unfreeze = false)
    {
        isFrozen = !unfreeze;
        rb.isKinematic = !unfreeze;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contactCount <= 0) return;

        // If collided with an already in-contact collider - ignore and return
        if (groundColliders.Contains(collision.collider)) return;

        var averageContactPosition = GetAverageContactPos(collision);

        // If contacts are above the center of the capsule - ignore and return
        if (averageContactPosition.y > transform.position.y) return;

        groundColliders.Add(collision.collider);
    }

    private void OnCollisionExit(Collision collision)
    {
        // If exited a collider that is no longer registered - ignore and return
        if (!groundColliders.Contains(collision.collider)) return;

        groundColliders.Remove(collision.collider);
    }

    private Vector3 GetAverageContactPos(Collision collision)
    {
        Vector3 averageContactPosition = Vector3.zero;

        foreach (var contact in collision.contacts)
        {
            averageContactPosition += contact.point;
        }

        averageContactPosition = averageContactPosition / collision.contactCount;

        return averageContactPosition;
    }
}
