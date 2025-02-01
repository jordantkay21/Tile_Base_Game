using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static event Action<Vector2> OnHover;
    public static event Action OnSelect;
    public static event Action<bool> OnCameraControlActivate;

    private GameInput gameInput;

    private void Awake()
    {
        gameInput = new GameInput();
    }

    private void OnEnable()
    {
        gameInput.GameInputs.Enable();

        gameInput.TileInteractions.Enable();

        gameInput.GameInputs.ActivateCameraControls.performed += HandleCameraActivation;
        gameInput.GameInputs.ActivateCameraControls.canceled += HandleCameraActivation;

        gameInput.TileInteractions.Hover.performed += HandleHover;
        gameInput.TileInteractions.Select.performed += HandleSelect;
    }

    private void OnDisable()
    {

        gameInput.GameInputs.ActivateCameraControls.performed -= HandleCameraActivation;
        gameInput.GameInputs.ActivateCameraControls.canceled -= HandleCameraActivation;

        gameInput.TileInteractions.Hover.performed -= HandleHover;
        gameInput.TileInteractions.Select.performed -= HandleSelect;

        gameInput.GameInputs.Disable();
        gameInput.TileInteractions.Disable();
    }

    private void HandleCameraActivation(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            gameInput.TileInteractions.Disable();
            OnCameraControlActivate?.Invoke(true);
        }
        else
        {
            OnCameraControlActivate?.Invoke(false);
            gameInput.TileInteractions.Enable();
        }
    }


    private void HandleHover(InputAction.CallbackContext context)
    {
        Vector2 mousePos = context.ReadValue<Vector2>();
        OnHover?.Invoke(mousePos);
    }
    private void HandleSelect(InputAction.CallbackContext context)
    {
        OnSelect?.Invoke();
    }

}
