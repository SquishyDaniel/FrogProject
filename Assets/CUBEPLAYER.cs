using UnityEngine;
using UnityEngine.InputSystem;

public class CUBEPLAYER : MonoBehaviour
{
    public float jumpIntensity = 5f;
    public Rigidbody playerRigidbody;
    public Transform mainCamera;
    public float turnSmoothing = 0.1f;
    public float turnVelocity = 0.5f;
    InputAction moveAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      Debug.Log("Pretty cool");
      
      moveAction = InputSystem.actions.FindAction("Move");   

      // https://docs.unity3d.com/6000.6/Documentation/ScriptReference/CursorLockMode.None.html
      Cursor.lockState = CursorLockMode.Locked; 
    }

    // Update is called once per frame
    void Update() 
    {

    }

    // Update at a fixed interval, necessary for physics.
    void FixedUpdate()
    {
      Vector2 moveValue = moveAction.ReadValue<Vector2>();
      Vector3 direction = new Vector3(moveValue.x, 0f, moveValue.y).normalized;

      if (direction.magnitude >= 0.1f) {
        // Referenced from here:
        // https://youtu.be/4HpC--2iowE&t=659
        // https://github.com/SyrupMedia/media.syrup.unity.player/blob/master/Assets/Scripts/Player/Player.cs

        // Euler angles and quaternion explanation:
        // https://youtu.be/zjMuIxRvygQ

        // Calculate the movement angle relative to the camera, translated from radius to degrees
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnSmoothing);
      
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        playerRigidbody.AddForce(moveDirection*25f);
      }
    }
    
    public void OnJump() {
      Debug.Log("Jump");
      playerRigidbody.AddForce(Vector3.up*jumpIntensity, ForceMode.Impulse);
    }

    // public void OnMove(InputAction.CallbackContext value) {
    //    Vector2 inputMovement = value.ReadValue<Vector2>();
    //    Debug.Log(inputMovement);
    // }

    // public void OnMove(InputAction.CallbackContext context) {
    //   Debug.Log("Moved");
    // }
}
