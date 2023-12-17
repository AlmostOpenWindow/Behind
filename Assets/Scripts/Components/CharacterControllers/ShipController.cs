using System;
using System.Collections;
using System.Collections.Generic;
using Components.Common;
using Infrastructure.Services.Input;
using UnityEngine.VFX;
using Utils;

namespace Components.CharacterControllers
{
    using UnityEngine;
#if ENABLE_INPUT_SYSTEM
    using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

    [RequireComponent(typeof(CharacterController))]
// #if ENABLE_INPUT_SYSTEM 
//     [RequireComponent(typeof(PlayerInput))]
// #endif
    public class ShipController : MonoBehaviour, IUpdater, ILateUpdater
    {
        [Header("Player")] [Tooltip("Чтобы не переворачивался в мертвую петлю")]
        public Vector2 MinMaxXRotation;

        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 20.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        public float RotationSmoothTime = 1f;

        public float TiltSpeed = 100f;

        [Tooltip("Absolute value of max TiltAngle")]
        public float TiltAngleMax = 90f;

        public float TiltAngleMin = 5f;
        
        [Range(0.0f, 3.0f)] 
        public float TiltAngleMultiplier = 1f;
        
        [Header("Acceleration and deceleration")]
        public float SpeedAcceleration = 10.0f;

        public float SpeedDeceleration = 5.0f;

        [Header("Movement for FirstPerson View")] [Tooltip("Rotation speed of the character")]
        public float RotationSpeed = 1.0f;

        public float NoTurn = 0.01f;

        [Header("Animations")] public Animator ArmatureAnimator;

        public float SpeedXAnimValueLerpTime = 1f;

        public GameObject ShipModel;

        public Vector2 MinMaxShipModelRotationX;

        public Vector2 MinMaxShipModelRotationY;

        public float ShipModelRotationSpeed = 10f;

        [Header("Gravity & Ground")] [Space(10)] [Tooltip("The height the player can jump")]
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

        [Tooltip("Useful for rough ground")] public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("Эффект нитро")] public VisualEffect nitroEffect;

        [Tooltip("Эффект задних двигателей")] public List<VisualEffect> backEnginesEffect;

        [Tooltip("Эффект боковых двигателей")] public List<VisualEffect> sideEnginesEffect;

        public AudioSource nitroSound;

        public AudioSource flightSound;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        public bool DoCameraClamp = true;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        public float _speed;
        private float _animationBlendX;
        private float _animationBlendZ;
        private float _animationBlendLerpedX;
        private float _animationBlendLerpedZ;
        
        private Quaternion _rotationVelocity;
        private Quaternion _tiltVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _nitroTimeoutDelta;
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animSpeedX;
        private int _animSpeedZ;
        
#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
#endif
        private CharacterController _controller;
        private IInputService _input;
        private GameplayCamera.GameplayCamera _gameplayCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;
        private Vector3 _cachedInputDirection;

        private bool _constructed;
        
        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _input.PlayerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }

        public void Construct(IInputService inputService, GameplayCamera.GameplayCamera gameplayCamera)
        {
            _input = inputService;
            _gameplayCamera = gameplayCamera;

            _constructed = true;
        }
        
        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _hasAnimator = ArmatureAnimator != null;
            _controller = GetComponent<CharacterController>();
            
            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
            _nitroTimeoutDelta = 0;
        }

        public void OnUpdate()
        {
            if (!_constructed)
                return;
            
            Move();

            if (_input.Data.move != Vector2.zero)
            {
                foreach (var visualEffect in backEnginesEffect)
                {
                    visualEffect.enabled = true;
                }

                if (!flightSound.isPlaying)
                {
                    flightSound.Play(0);
                    StartCoroutine(FadeAudioSource.StartFade(flightSound, 0.3f, 0.1f));
                }
            }

            if (_input.Data.sprint && _nitroTimeoutDelta <= 0.0f)
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

            if (_input.Data.move == Vector2.zero)
            {
                foreach (var visualEffect in backEnginesEffect)
                {
                    visualEffect.enabled = false;
                }

                StartCoroutine(SecondTask(FadeAudioSource.StartFade(flightSound, 0.3f, 0), flightSound.Stop));
            }

            if (!_input.Data.sprint)
            {
                nitroSound.Stop();
                //StartCoroutine(SecondTask(FadeAudioSource.StartFade(nitroSound, 0.5f, 0), nitroSound.Stop));
            }
        }

        public void OnLateUpdate()
        {
            if (!_constructed)
                return;
            
            Rotation();
        }

        private IEnumerator SecondTask(IEnumerator first, Action action)
        {
            yield return StartCoroutine(first);
            action();
        }

        private void AssignAnimationIDs()
        {
            _animSpeedX = Animator.StringToHash("SpeedX");
            _animSpeedZ = Animator.StringToHash("SpeedZ");
        }

        private float GetTiltShipAngle()
        {
            var camT = _gameplayCamera.transform;
            
            Vector3 targetDir = camT.position + camT.forward * 100f - transform.position;
            Vector3 forward = transform.forward;
            float angle = Vector3.SignedAngle(targetDir, forward, Vector3.up) * TiltAngleMultiplier;
            angle = Mathf.Clamp(angle, -TiltAngleMax, TiltAngleMax);
            
            return Mathf.Abs(angle) < TiltAngleMin
                ? 0.0f 
                : angle;
        }
        
        private void Rotation()
        {
            var rotation = transform.rotation;
            var desiredRotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, 0.0f);
            
            if (!_input.Data.lockRotation)
            {
                if (_input.Data.move != Vector2.zero)
                {
                    var tiltAngle = GetTiltShipAngle();
                    Debug.Log("TILT: " + tiltAngle);
                    desiredRotation = _gameplayCamera.MainCamera.transform.rotation;
                    desiredRotation *= Quaternion.AngleAxis(tiltAngle, Vector3.forward);
                }
            }
            
            transform.rotation = QuaternionUtil.SmoothDamp(transform.rotation, desiredRotation, ref _rotationVelocity,
                RotationSmoothTime);
            
            TPCameraRotation();
        }

        private void RotationToLook(Transform rotatable, float rotationSpeed, Vector2? minMaxAnglesX,
            Vector2? minMaxAnglesY, bool freezeZ)
        {
            var t = rotatable;
            var delta = new Vector2(
                Mathf.Abs(_input.Data.look.x) < NoTurn
                    ? 0
                    : _input.Data.look.x * rotationSpeed,
                Mathf.Abs(_input.Data.look.y) < NoTurn
                    ? 0
                    : _input.Data.look.y * rotationSpeed);

            var tRotation = t.localRotation;
            var smoothX = Mathf.Lerp(tRotation.eulerAngles.x, tRotation.eulerAngles.x + delta.y,
                Time.deltaTime * RotationSmoothTime);
            var smoothY = Mathf.Lerp(tRotation.eulerAngles.y, tRotation.eulerAngles.y + delta.x,
                Time.deltaTime * RotationSmoothTime);

            var newRotation = new Quaternion
            {
                eulerAngles = new Vector3(smoothX, smoothY, 0)
            };

            SetMinMaxAngles(ref newRotation, new Vector2(smoothX, smoothY), minMaxAnglesX, minMaxAnglesY, freezeZ);
            rotatable.localRotation = newRotation;
        }

        private void SetMinMaxAngles(ref Quaternion rotation, Vector2 smoothXY, Vector2? minMaxAnglesX,
            Vector2? minMaxAnglesY, bool freezeZ)
        {
            if (minMaxAnglesX.HasValue)
            {
                if (rotation.eulerAngles.x > 90 && rotation.eulerAngles.x < minMaxAnglesX.Value.x + 360)
                {
                    rotation.eulerAngles = new Vector3(
                        minMaxAnglesX.Value.x,
                        smoothXY.y,
                        freezeZ ? 0.0f : rotation.eulerAngles.z);
                }
                else if (rotation.eulerAngles.x is >= 0 and < 90)
                {
                    rotation.eulerAngles = new Vector3(
                        Mathf.Clamp(rotation.eulerAngles.x, 0, minMaxAnglesX.Value.y),
                        smoothXY.y,
                        freezeZ ? 0.0f : rotation.eulerAngles.z);
                }
            }

            if (minMaxAnglesY.HasValue)
            {
                if (rotation.eulerAngles.y > 90 && rotation.eulerAngles.y < minMaxAnglesY.Value.x + 360)
                {
                    rotation.eulerAngles = new Vector3(
                        smoothXY.x,
                        minMaxAnglesY.Value.x,
                        freezeZ ? 0.0f : rotation.eulerAngles.z);
                }
                else if (rotation.eulerAngles.y is >= 0 and < 90)
                {
                    rotation.eulerAngles = new Vector3(
                        smoothXY.x,
                        Mathf.Clamp(rotation.eulerAngles.y, 0, minMaxAnglesY.Value.y),
                        freezeZ ? 0.0f : rotation.eulerAngles.z);
                }
            }
        }
        
        private void TPCameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.Data.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.Data.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.Data.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = DoCameraClamp
                ? ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue)
                : Clamp360(_cinemachineTargetYaw);
            _cinemachineTargetPitch = DoCameraClamp
                ? ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp)
                : Clamp360(_cinemachineTargetPitch);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }
        

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _input.Data.sprint ? SprintSpeed : MoveSpeed;
            var targetAnimationSpeed = targetSpeed;
            var speedAcceleration = SpeedAcceleration;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            float inputMagnitude = _input.Data.analogMovement ? _input.Data.move.magnitude : 1f;

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.Data.move == Vector2.zero)
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
            
            // update animator if using character
            var velocity = _controller.velocity;
            var localVelocity = transform.InverseTransformDirection(velocity);

            _animationBlendX = localVelocity.x / targetAnimationSpeed;
            _animationBlendZ = localVelocity.z / targetAnimationSpeed;

            if (_animationBlendX < 0.01f && _animationBlendX > 0.01f) _animationBlendX = 0f;
            if (_animationBlendZ < 0.01f && _animationBlendZ > 0.01f) _animationBlendZ = 0f;

            if (_hasAnimator)
            {
                var sign = Math.Sign(localVelocity.x);
                if (_input.Data.move.x < 0)
                {
                    sideEnginesEffect[1].enabled = true;
                    sideEnginesEffect[0].enabled = false;
                }
                else if (_input.Data.move.x > 0)
                {
                    sideEnginesEffect[1].enabled = false;
                    sideEnginesEffect[0].enabled = true;
                }
                else
                {
                    sideEnginesEffect[0].enabled = false;
                    sideEnginesEffect[1].enabled = false;
                }
             
                _animationBlendLerpedZ = Mathf.Lerp(_animationBlendLerpedZ, _animationBlendZ,
                    Time.deltaTime * SpeedXAnimValueLerpTime);
                _animationBlendLerpedX = Mathf.Lerp(_animationBlendLerpedX, _animationBlendX,
                    Time.deltaTime * SpeedXAnimValueLerpTime);

                ArmatureAnimator.SetFloat(_animSpeedX, _animationBlendLerpedX);
                ArmatureAnimator.SetFloat(_animSpeedZ, _animationBlendLerpedZ);
            }
        }

        private void FPMove()
        {
            var t = transform;

            if (_input.Data.move != Vector2.zero)
            {
                _cachedInputDirection = t.right * _input.Data.move.x + t.forward * _input.Data.move.y;
            }


            _controller.Move(_cachedInputDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
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
                if (_input.Data.jump && _jumpTimeoutDelta <= 0.0f)
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
                _input.Data.jump = false;
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