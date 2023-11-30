using System;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine.VFX;

namespace Components.CharacterControllers
{
     using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ShipController : MonoBehaviour
    {
        [Header("Player")] 
        [Tooltip("Чтобы не переворачивался в мертвую петлю")]
        public Vector2 MinMaxXRotation;
        
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 20.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        public float RotationSmoothTime = 1f;
        
        [Header("Acceleration and deceleration")]
        public float SpeedAcceleration = 10.0f;

        public float SpeedDeceleration = 5.0f;
        
        [Header("Movement for FirstPerson View")]
        [Tooltip("Rotation speed of the character")]
        public float RotationSpeed = 1.0f;

        public float NoTurn = 0.01f;

        [Header("Animations")] 
        public Animator ArmatureAnimator;

        public float SpeedXAnimValueLerpTime = 1f;
        
        [Header("Gravity & Ground")]
        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;
        
        [Tooltip("Time required to pass before entering the nitro state")]
        public float NitroTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;
        
        [Tooltip("Эффект нитро")]
        public VisualEffect nitroEffect;

        public AudioSource nitroSound;

        // [Tooltip("How far in degrees can you move the camera up")]
        // public float TopClamp = 70.0f;
        //
        // [Tooltip("How far in degrees can you move the camera down")]
        // public float BottomClamp = -30.0f;
        //
        // [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        // public float CameraAngleOverride = 0.0f;
        //
        // [Tooltip("For locking the camera position on all axis")]
        // public bool LockCameraPosition = false;
        //
        // public bool DoCameraClamp = true;
        
        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        public float _speed;
        private float _animationBlendX;
        private float _animationBlendZ;
        private float _animationBlendLerpedX;
        private float _animationBlendLerpedZ;
        
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _nitroTimeoutDelta;
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animSpeedX;
        private int _animSpeedZ;
        
        // private int _animIDSpeed;
        // private int _animIDGrounded;
        // private int _animIDJump;
        // private int _animIDFreeFall;
        // private int _animIDMotionSpeed;

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;
        private Vector3 _cachedInputDirection;
        
        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }


        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _hasAnimator = ArmatureAnimator != null;
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM 
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
            _nitroTimeoutDelta = 0;
            nitroSound.Play(0);
        }

        private bool nitroEffectIsPlaying = false;
        
        private void Update()
        {
            //JumpAndGravity();
            //GroundedCheck();
            Move();
            if (_input.sprint && _nitroTimeoutDelta <= 0.0f)
            {
                nitroEffect.Play();
                if (!nitroSound.isPlaying)
                {
                    nitroSound.Play(0);
                }
                _nitroTimeoutDelta = NitroTimeout;
            }
            if (_nitroTimeoutDelta >= 0.0f)
            {
                _nitroTimeoutDelta -= Time.deltaTime;
            }

            if (!_input.sprint)
            {
                nitroSound.Stop();
            }
        }

        private void LateUpdate()
        {
            Rotation();
        }

        private void AssignAnimationIDs()
        {
            _animSpeedX = Animator.StringToHash("SpeedX");
            _animSpeedZ = Animator.StringToHash("SpeedZ");
            
            //_animIDSpeed = Animator.StringToHash("Speed");
            // _animIDGrounded = Animator.StringToHash("Grounded");
            // _animIDJump = Animator.StringToHash("Jump");
            // _animIDFreeFall = Animator.StringToHash("FreeFall");
            // _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        // private void GroundedCheck()
        // {
        //     // set sphere position, with offset
        //     Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
        //         transform.position.z);
        //     Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
        //         QueryTriggerInteraction.Ignore);
        //
        //     // update animator if using character
        //     if (_hasAnimator)
        //     {
        //         ArmatureAnimator.SetBool(_animIDGrounded, Grounded);
        //     }
        // }

        private void Rotation()
        {
            var t = transform;
            var delta = new Vector2(
                Mathf.Abs(_input.look.x) < NoTurn 
                    ? 0 
                    : _input.look.x * RotationSpeed, 
                Mathf.Abs(_input.look.y) < NoTurn 
                    ? 0 
                    : _input.look.y * RotationSpeed);

            var tRotation = t.rotation;
            var smoothX = Mathf.Lerp(tRotation.eulerAngles.x, tRotation.eulerAngles.x + delta.y, Time.deltaTime * RotationSmoothTime);
            var smoothY = Mathf.Lerp(tRotation.eulerAngles.y, tRotation.eulerAngles.y + delta.x, Time.deltaTime * RotationSmoothTime);
            
            Quaternion newRotation = new Quaternion();
            newRotation.eulerAngles = new Vector3(smoothX, smoothY, 0);
            
            if (newRotation.eulerAngles.x > 90 && newRotation.eulerAngles.x < MinMaxXRotation.x + 360)
            {
                newRotation.eulerAngles = new Vector3(
                    MinMaxXRotation.x,
                    smoothY,
                    0);
            }
            else
            if (newRotation.eulerAngles.x is >= 0 and < 90)
            {
                newRotation.eulerAngles = new Vector3(
                    Mathf.Clamp(newRotation.eulerAngles.x, 0, MinMaxXRotation.y),
                    smoothY,
                    0);
            }
            transform.rotation = newRotation;
        }
        // private void Rotation()
        // {
        //     FPRotation();
        // }
        
        // private void TPCameraRotation()
        // {
        //     // if there is an input and camera position is not fixed
        //     if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        //     {
        //         //Don't multiply mouse input by Time.deltaTime;
        //         float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
        //
        //         _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
        //         _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
        //     }
        //
        //     // clamp our rotations so our values are limited 360 degrees
        //     _cinemachineTargetYaw = DoCameraClamp
        //         ? ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue)
        //         : Clamp360(_cinemachineTargetYaw);
        //     _cinemachineTargetPitch = DoCameraClamp
        //         ? ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp)
        //         : Clamp360(_cinemachineTargetPitch);
        //
        //     // Cinemachine will follow this target
        //     CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
        //         _cinemachineTargetYaw, 0.0f);
        // }

        // private void FPRotation()
        // {
        //     // if there is an input
        //     if (_input.look.sqrMagnitude >= _threshold)
        //     {
        //         //Don't multiply mouse input by Time.deltaTime
        //         float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				    //
        //         //_cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
        //         //_rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;
        //
        //         // clamp our pitch rotation
        //         //_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
        //
        //         // Update Cinemachine camera target pitch
        //         //CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);
        //
        //         //TPCameraRotation();
        //         // rotate the player left and right
        //         //transform.Rotate(Vector3.up * _rotationVelocity);
        //     }
        // }
        
        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
            var targetAnimationSpeed = targetSpeed;
            var speedAcceleration = SpeedAcceleration;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon
            
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
            
            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero)
            {
                targetSpeed = 0.0f;
                speedAcceleration = SpeedDeceleration;
            }

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = _controller.velocity.magnitude; 
            //for only horizontal speed: new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            
            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * speedAcceleration);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            FPMove();
            //TPMove();
            
            // update animator if using character
            
            var velocity = _controller.velocity;
            var localVelocity = transform.InverseTransformDirection(velocity);
            Debug.Log("LocalVelocity: " + localVelocity);
            
            _animationBlendX = localVelocity.x / targetAnimationSpeed;
            _animationBlendZ = localVelocity.z / targetAnimationSpeed;
            Debug.Log("AnimBlend: " + _animationBlendX);
            
            if (_animationBlendX < 0.01f && _animationBlendX > 0.01f) _animationBlendX = 0f;
            if (_animationBlendZ < 0.01f && _animationBlendZ > 0.01f) _animationBlendZ = 0f;
            
            if (_hasAnimator)
            {
                Debug.Log("Blend: " + _animationBlendX);
                Debug.Log("SIGN: " + Math.Sign(localVelocity.x));
                _animationBlendLerpedX = Mathf.Lerp(_animationBlendLerpedX, _animationBlendX,
                    Time.deltaTime * SpeedXAnimValueLerpTime);
                var speedXValue = _animationBlendLerpedX;
        
                ArmatureAnimator.SetFloat(_animSpeedX, speedXValue);
            }
        }


        // private void TPMove()
        // {
        //     Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
        //
        //     // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        //     // if there is a move input rotate player when the player is moving
        //     if (_input.move != Vector2.zero)
        //     {
        //         _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
        //                           _mainCamera.transform.eulerAngles.y;
        //         float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
        //             RotationSmoothTime);
        //
        //         // rotate to face input direction relative to camera position
        //         transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        //     }
        //     
        //     Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        //
        //     // move the player
        //     _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
        //                      new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        // }
        
        private void FPMove()
        {
            var t = transform;
            
            if (_input.move != Vector2.zero)
            {
                _cachedInputDirection = t.right * _input.move.x + t.forward * _input.move.y;
            }
            
            
            _controller.Move(_cachedInputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                // if (_hasAnimator)
                // {
                //     ArmatureAnimator.SetBool(_animIDJump, false);
                //     ArmatureAnimator.SetBool(_animIDFreeFall, false);
                // }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    // if (_hasAnimator)
                    // {
                    //     ArmatureAnimator.SetBool(_animIDJump, true);
                    // }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    // if (_hasAnimator)
                    // {
                    //     ArmatureAnimator.SetBool(_animIDFreeFall, true);
                    // }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }
        
        private static float Clamp360(float lfAngle)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;

            return lfAngle;
        }
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }
    }
}
