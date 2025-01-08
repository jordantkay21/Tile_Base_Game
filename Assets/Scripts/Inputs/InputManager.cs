using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static event Action<Vector2> OnHover;
    public static event Action OnSelect;

    private GameInput gameInput;

    private void Awake()
    {
        gameInput = new GameInput();
    }

    private void OnEnable()
    {
        gameInput.TileInteractions.Enable();

        gameInput.TileInteractions.Hover.performed += HandleHover;
        gameInput.TileInteractions.Select.performed += HandleSelect;
    }

    private void OnDisable()
    {

        gameInput.TileInteractions.Hover.performed -= HandleHover;
        gameInput.TileInteractions.Select.performed -= HandleSelect;
        
        gameInput.TileInteractions.Enable();
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
