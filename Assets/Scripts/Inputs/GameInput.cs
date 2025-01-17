//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.11.2
//     from Assets/Scripts/Inputs/GameInput.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @GameInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameInput"",
    ""maps"": [
        {
            ""name"": ""TileInteractions"",
            ""id"": ""e31b2e86-efd8-4ae3-9d6b-72408ee28457"",
            ""actions"": [
                {
                    ""name"": ""Hover"",
                    ""type"": ""Value"",
                    ""id"": ""a34f3012-1e97-423d-8348-eca3d27ba4e4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""b42dcdb3-da2b-4d01-91e4-e8609eb46599"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d36093e7-e397-47dd-8f64-160710269c71"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Hover"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""856e676d-2630-403e-bc90-ec4cca279b02"",
                    ""path"": ""<Touchscreen>/primaryTouch/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Hover"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eeaa6edb-802a-4337-b9b3-44add37eaa3e"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""848b74db-b0a2-4ba4-89bd-8d27d32cd38b"",
                    ""path"": ""<Touchscreen>/Press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // TileInteractions
        m_TileInteractions = asset.FindActionMap("TileInteractions", throwIfNotFound: true);
        m_TileInteractions_Hover = m_TileInteractions.FindAction("Hover", throwIfNotFound: true);
        m_TileInteractions_Select = m_TileInteractions.FindAction("Select", throwIfNotFound: true);
    }

    ~@GameInput()
    {
        UnityEngine.Debug.Assert(!m_TileInteractions.enabled, "This will cause a leak and performance issues, GameInput.TileInteractions.Disable() has not been called.");
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // TileInteractions
    private readonly InputActionMap m_TileInteractions;
    private List<ITileInteractionsActions> m_TileInteractionsActionsCallbackInterfaces = new List<ITileInteractionsActions>();
    private readonly InputAction m_TileInteractions_Hover;
    private readonly InputAction m_TileInteractions_Select;
    public struct TileInteractionsActions
    {
        private @GameInput m_Wrapper;
        public TileInteractionsActions(@GameInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Hover => m_Wrapper.m_TileInteractions_Hover;
        public InputAction @Select => m_Wrapper.m_TileInteractions_Select;
        public InputActionMap Get() { return m_Wrapper.m_TileInteractions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TileInteractionsActions set) { return set.Get(); }
        public void AddCallbacks(ITileInteractionsActions instance)
        {
            if (instance == null || m_Wrapper.m_TileInteractionsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_TileInteractionsActionsCallbackInterfaces.Add(instance);
            @Hover.started += instance.OnHover;
            @Hover.performed += instance.OnHover;
            @Hover.canceled += instance.OnHover;
            @Select.started += instance.OnSelect;
            @Select.performed += instance.OnSelect;
            @Select.canceled += instance.OnSelect;
        }

        private void UnregisterCallbacks(ITileInteractionsActions instance)
        {
            @Hover.started -= instance.OnHover;
            @Hover.performed -= instance.OnHover;
            @Hover.canceled -= instance.OnHover;
            @Select.started -= instance.OnSelect;
            @Select.performed -= instance.OnSelect;
            @Select.canceled -= instance.OnSelect;
        }

        public void RemoveCallbacks(ITileInteractionsActions instance)
        {
            if (m_Wrapper.m_TileInteractionsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ITileInteractionsActions instance)
        {
            foreach (var item in m_Wrapper.m_TileInteractionsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_TileInteractionsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public TileInteractionsActions @TileInteractions => new TileInteractionsActions(this);
    public interface ITileInteractionsActions
    {
        void OnHover(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
    }
}
