using System;
using Infrastructure.Services.Input;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		public readonly InputData Data = new();

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(Data.cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnLockRotation(InputValue value)
		{
			LockRotationInput(value.isPressed);
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
		
		public void OnFlashlight(InputValue value)
		{
			FlashlightInput(value.isPressed);
		}

		public void OnInteract(InputValue value)
		{
			InteractInput();
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			Data.move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			Data.look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			Data.jump = newJumpState;
		}
		
		public void FlashlightInput(bool newFlashlightState)
		{
			Data.TriggerFlashlightClickEvent();
			Data.flashlight = newFlashlightState;
		}

		public void InteractInput()
		{
			Data.TriggerInteractClickEvent();
		}
		
		public void SprintInput(bool newSprintState)
		{
			Data.sprint = newSprintState;
		}

		public void LockRotationInput(bool lockRotationState)
		{
			Data.lockRotation = lockRotationState;
		}
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(Data.cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}