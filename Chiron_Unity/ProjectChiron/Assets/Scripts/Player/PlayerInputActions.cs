// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Player/PlayerInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Character"",
            ""id"": ""2c69ad3d-b174-4fc6-8459-54dcd1253810"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""154a1746-5178-473b-af0b-97cbc83a137d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""d236a0b0-15f4-414a-9866-18fe357d73c8"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""b955709d-7d0f-4b41-bae1-a3d82d95953b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""29715d9b-21e0-4b0a-a214-c0c099282727"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TeleportEnable"",
                    ""type"": ""Button"",
                    ""id"": ""98e14f1e-71fe-49af-a870-153b55f8a117"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SlowField"",
                    ""type"": ""Button"",
                    ""id"": ""894133c5-c4b0-44a3-9ac1-d76ebca11546"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Hack"",
                    ""type"": ""Button"",
                    ""id"": ""3d50e07e-68ef-4a99-8e43-115bc4ef1d07"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""WeaponPower"",
                    ""type"": ""Button"",
                    ""id"": ""732fbd58-2295-475e-acf6-e4b385fbff1a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwitchMobility"",
                    ""type"": ""Button"",
                    ""id"": ""9b504112-ae15-42a1-a394-bd73c5674a11"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwitchOffense"",
                    ""type"": ""Button"",
                    ""id"": ""2b966703-03e8-4eda-90e6-b8b691d6034b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwitchDefense"",
                    ""type"": ""Button"",
                    ""id"": ""e904a7cd-31d4-4424-92a1-931c74642a7c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8e396305-8a2a-4214-8035-067b6ed7ce43"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""78c79942-7487-4fda-92d7-e3f90c98c2bd"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""2568a87c-22e6-4b6b-9ce0-cb75fdf92e9d"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""50538569-dd85-47bf-895b-812f447ed403"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""fcb79a50-f4e6-4020-b3d8-7a2e865426b8"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""5e65679d-6d63-4766-b0d5-12bcfa02b99c"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""5ab61f6f-cd56-445e-a1a7-d251abae2f29"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d2af041f-8f60-4a3d-b240-ba299ab0063d"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6fe2daf7-6850-4352-b708-701f9715165c"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SlowField"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4841be22-601a-418a-87c1-fe5921fc08c4"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchMobility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ffd09ac3-1a03-4dfb-9895-edb9fae1886c"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchOffense"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2df7c1ea-067f-4881-8918-7948353a430a"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchDefense"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f871e50e-9f63-43fc-8a6d-7c4ac7c57cec"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WeaponPower"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c620295e-b85c-4a26-93cd-67a5de47e97b"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TeleportEnable"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2d9e29f8-56a5-4760-9dad-3832d83f2f18"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Hack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Teleport"",
            ""id"": ""1f54e97b-98d6-4ac4-8e75-ca3669239474"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""a9e70fe7-3594-4945-8c8e-95916e7141b4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ConfirmLocation"",
                    ""type"": ""Button"",
                    ""id"": ""c5c7c50a-67f3-4983-b49c-b2e26beb37e5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SelectLocation"",
                    ""type"": ""Value"",
                    ""id"": ""09866401-fb12-405b-948b-b39b651a51ab"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""34ed562b-02dc-4824-8319-42dce71970fa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""534e3112-d3bf-4224-bf83-4b7ca0d54e41"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""cbfe730c-4d74-41ea-98ab-9479e719d914"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""dba95156-eaac-4fc5-ba4f-f77a0cd4f569"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""7e0d5628-4d51-450b-99bb-6b972e96a3ec"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""15858264-997c-4c65-aa7c-b9a1d2b12e0d"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5d107576-5d42-469f-8503-e1eddb2b69f3"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ConfirmLocation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""623cb90b-8fe7-4ed9-8388-5e198a25e848"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectLocation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3086d44c-582d-4342-95b3-778120ce7290"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Character
        m_Character = asset.FindActionMap("Character", throwIfNotFound: true);
        m_Character_Move = m_Character.FindAction("Move", throwIfNotFound: true);
        m_Character_Aim = m_Character.FindAction("Aim", throwIfNotFound: true);
        m_Character_Shoot = m_Character.FindAction("Shoot", throwIfNotFound: true);
        m_Character_Dash = m_Character.FindAction("Dash", throwIfNotFound: true);
        m_Character_TeleportEnable = m_Character.FindAction("TeleportEnable", throwIfNotFound: true);
        m_Character_SlowField = m_Character.FindAction("SlowField", throwIfNotFound: true);
        m_Character_Hack = m_Character.FindAction("Hack", throwIfNotFound: true);
        m_Character_WeaponPower = m_Character.FindAction("WeaponPower", throwIfNotFound: true);
        m_Character_SwitchMobility = m_Character.FindAction("SwitchMobility", throwIfNotFound: true);
        m_Character_SwitchOffense = m_Character.FindAction("SwitchOffense", throwIfNotFound: true);
        m_Character_SwitchDefense = m_Character.FindAction("SwitchDefense", throwIfNotFound: true);
        // Teleport
        m_Teleport = asset.FindActionMap("Teleport", throwIfNotFound: true);
        m_Teleport_Move = m_Teleport.FindAction("Move", throwIfNotFound: true);
        m_Teleport_ConfirmLocation = m_Teleport.FindAction("ConfirmLocation", throwIfNotFound: true);
        m_Teleport_SelectLocation = m_Teleport.FindAction("SelectLocation", throwIfNotFound: true);
        m_Teleport_Cancel = m_Teleport.FindAction("Cancel", throwIfNotFound: true);
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

    // Character
    private readonly InputActionMap m_Character;
    private ICharacterActions m_CharacterActionsCallbackInterface;
    private readonly InputAction m_Character_Move;
    private readonly InputAction m_Character_Aim;
    private readonly InputAction m_Character_Shoot;
    private readonly InputAction m_Character_Dash;
    private readonly InputAction m_Character_TeleportEnable;
    private readonly InputAction m_Character_SlowField;
    private readonly InputAction m_Character_Hack;
    private readonly InputAction m_Character_WeaponPower;
    private readonly InputAction m_Character_SwitchMobility;
    private readonly InputAction m_Character_SwitchOffense;
    private readonly InputAction m_Character_SwitchDefense;
    public struct CharacterActions
    {
        private @PlayerInputActions m_Wrapper;
        public CharacterActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Character_Move;
        public InputAction @Aim => m_Wrapper.m_Character_Aim;
        public InputAction @Shoot => m_Wrapper.m_Character_Shoot;
        public InputAction @Dash => m_Wrapper.m_Character_Dash;
        public InputAction @TeleportEnable => m_Wrapper.m_Character_TeleportEnable;
        public InputAction @SlowField => m_Wrapper.m_Character_SlowField;
        public InputAction @Hack => m_Wrapper.m_Character_Hack;
        public InputAction @WeaponPower => m_Wrapper.m_Character_WeaponPower;
        public InputAction @SwitchMobility => m_Wrapper.m_Character_SwitchMobility;
        public InputAction @SwitchOffense => m_Wrapper.m_Character_SwitchOffense;
        public InputAction @SwitchDefense => m_Wrapper.m_Character_SwitchDefense;
        public InputActionMap Get() { return m_Wrapper.m_Character; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CharacterActions set) { return set.Get(); }
        public void SetCallbacks(ICharacterActions instance)
        {
            if (m_Wrapper.m_CharacterActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMove;
                @Aim.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnAim;
                @Shoot.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnShoot;
                @Dash.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnDash;
                @TeleportEnable.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnTeleportEnable;
                @TeleportEnable.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnTeleportEnable;
                @TeleportEnable.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnTeleportEnable;
                @SlowField.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnSlowField;
                @SlowField.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnSlowField;
                @SlowField.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnSlowField;
                @Hack.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnHack;
                @Hack.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnHack;
                @Hack.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnHack;
                @WeaponPower.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnWeaponPower;
                @WeaponPower.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnWeaponPower;
                @WeaponPower.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnWeaponPower;
                @SwitchMobility.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnSwitchMobility;
                @SwitchMobility.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnSwitchMobility;
                @SwitchMobility.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnSwitchMobility;
                @SwitchOffense.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnSwitchOffense;
                @SwitchOffense.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnSwitchOffense;
                @SwitchOffense.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnSwitchOffense;
                @SwitchDefense.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnSwitchDefense;
                @SwitchDefense.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnSwitchDefense;
                @SwitchDefense.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnSwitchDefense;
            }
            m_Wrapper.m_CharacterActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @TeleportEnable.started += instance.OnTeleportEnable;
                @TeleportEnable.performed += instance.OnTeleportEnable;
                @TeleportEnable.canceled += instance.OnTeleportEnable;
                @SlowField.started += instance.OnSlowField;
                @SlowField.performed += instance.OnSlowField;
                @SlowField.canceled += instance.OnSlowField;
                @Hack.started += instance.OnHack;
                @Hack.performed += instance.OnHack;
                @Hack.canceled += instance.OnHack;
                @WeaponPower.started += instance.OnWeaponPower;
                @WeaponPower.performed += instance.OnWeaponPower;
                @WeaponPower.canceled += instance.OnWeaponPower;
                @SwitchMobility.started += instance.OnSwitchMobility;
                @SwitchMobility.performed += instance.OnSwitchMobility;
                @SwitchMobility.canceled += instance.OnSwitchMobility;
                @SwitchOffense.started += instance.OnSwitchOffense;
                @SwitchOffense.performed += instance.OnSwitchOffense;
                @SwitchOffense.canceled += instance.OnSwitchOffense;
                @SwitchDefense.started += instance.OnSwitchDefense;
                @SwitchDefense.performed += instance.OnSwitchDefense;
                @SwitchDefense.canceled += instance.OnSwitchDefense;
            }
        }
    }
    public CharacterActions @Character => new CharacterActions(this);

    // Teleport
    private readonly InputActionMap m_Teleport;
    private ITeleportActions m_TeleportActionsCallbackInterface;
    private readonly InputAction m_Teleport_Move;
    private readonly InputAction m_Teleport_ConfirmLocation;
    private readonly InputAction m_Teleport_SelectLocation;
    private readonly InputAction m_Teleport_Cancel;
    public struct TeleportActions
    {
        private @PlayerInputActions m_Wrapper;
        public TeleportActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Teleport_Move;
        public InputAction @ConfirmLocation => m_Wrapper.m_Teleport_ConfirmLocation;
        public InputAction @SelectLocation => m_Wrapper.m_Teleport_SelectLocation;
        public InputAction @Cancel => m_Wrapper.m_Teleport_Cancel;
        public InputActionMap Get() { return m_Wrapper.m_Teleport; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TeleportActions set) { return set.Get(); }
        public void SetCallbacks(ITeleportActions instance)
        {
            if (m_Wrapper.m_TeleportActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_TeleportActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_TeleportActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_TeleportActionsCallbackInterface.OnMove;
                @ConfirmLocation.started -= m_Wrapper.m_TeleportActionsCallbackInterface.OnConfirmLocation;
                @ConfirmLocation.performed -= m_Wrapper.m_TeleportActionsCallbackInterface.OnConfirmLocation;
                @ConfirmLocation.canceled -= m_Wrapper.m_TeleportActionsCallbackInterface.OnConfirmLocation;
                @SelectLocation.started -= m_Wrapper.m_TeleportActionsCallbackInterface.OnSelectLocation;
                @SelectLocation.performed -= m_Wrapper.m_TeleportActionsCallbackInterface.OnSelectLocation;
                @SelectLocation.canceled -= m_Wrapper.m_TeleportActionsCallbackInterface.OnSelectLocation;
                @Cancel.started -= m_Wrapper.m_TeleportActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_TeleportActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_TeleportActionsCallbackInterface.OnCancel;
            }
            m_Wrapper.m_TeleportActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @ConfirmLocation.started += instance.OnConfirmLocation;
                @ConfirmLocation.performed += instance.OnConfirmLocation;
                @ConfirmLocation.canceled += instance.OnConfirmLocation;
                @SelectLocation.started += instance.OnSelectLocation;
                @SelectLocation.performed += instance.OnSelectLocation;
                @SelectLocation.canceled += instance.OnSelectLocation;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
            }
        }
    }
    public TeleportActions @Teleport => new TeleportActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    public interface ICharacterActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnTeleportEnable(InputAction.CallbackContext context);
        void OnSlowField(InputAction.CallbackContext context);
        void OnHack(InputAction.CallbackContext context);
        void OnWeaponPower(InputAction.CallbackContext context);
        void OnSwitchMobility(InputAction.CallbackContext context);
        void OnSwitchOffense(InputAction.CallbackContext context);
        void OnSwitchDefense(InputAction.CallbackContext context);
    }
    public interface ITeleportActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnConfirmLocation(InputAction.CallbackContext context);
        void OnSelectLocation(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
    }
}
