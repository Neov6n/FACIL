using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class XboxCursor : MonoBehaviour
{
    [SerializeField] 
    private PlayerInput playerInput;
    [SerializeField]
    private RectTransform cursorTransform;
    [SerializeField]
    private RectTransform canvasRectTransform;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private float cursorSpeed = 1000f;
    [SerializeField]
    private float padding = 35f;

    private levelManager theLevelManager;
    private bool previousMouseState;
    private Mouse virtualMouse;
    private Mouse currentMouse;
    private Camera mainCamera;

    private string previousControlScheme = "Keyboard&Mouse";
    private const string gamepadScheme = "Xbox";
    private const string mouseScheme = "Keyboard&Mouse";
    
    private void OnEnable()
    {
        theLevelManager = FindObjectOfType<levelManager>();

        currentMouse = Mouse.current;

        if(virtualMouse == null)
        {
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        } else if (!virtualMouse.added)
        {
            InputSystem.AddDevice(virtualMouse);
        }

        InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

        if(cursorTransform != null)
        {
            Vector2 position = cursorTransform.anchoredPosition;
            InputState.Change(virtualMouse.position, position);
        }
        
        InputSystem.onAfterUpdate += UpdateMotion;
        playerInput.onControlsChanged += OnControlsChanged;
    }

    private void OnDisable()
    {
        InputSystem.onAfterUpdate -= UpdateMotion;
        playerInput.onControlsChanged -= OnControlsChanged;
        playerInput.user.UnpairDevice(virtualMouse);
        if (virtualMouse != null && virtualMouse.added) InputSystem.RemoveDevice(virtualMouse);

    }

    private void UpdateMotion()
    {
        if (virtualMouse == null || Gamepad.current == null)
        {
            return;
        }

        Vector2 deltaValue = Gamepad.current.rightStick.ReadValue();
        deltaValue *= cursorSpeed * Time.deltaTime;

        Vector2 currentPosition = virtualMouse.position.ReadValue();
        Vector2 newPosition = currentPosition + deltaValue;

        newPosition.x = Mathf.Clamp(newPosition.x, padding, Screen.width - padding);
        newPosition.y = Mathf.Clamp(newPosition.y, padding, Screen.height - padding);

        InputState.Change(virtualMouse.position, newPosition);
        InputState.Change(virtualMouse.delta, deltaValue);

        bool aButtonIsPressed = Gamepad.current.xButton.IsPressed();
        //bool aButtonIsPressed = theLevelManager.fire1TimeHeld == 1;
        if (previousMouseState != aButtonIsPressed)
        {
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
            //mouseState.WithButton(MouseButton.Left, Gamepad.current.rightTrigger.IsPressed());
            InputState.Change(virtualMouse, mouseState);
            previousMouseState = aButtonIsPressed;
        }

        AnchorCursor(newPosition);
    }

    private void AnchorCursor(Vector2 position)
    {
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, 
            position, canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera, out anchoredPosition);
        cursorTransform.anchoredPosition = anchoredPosition;
    }

    private void OnControlsChanged(PlayerInput input)
    {
        if (playerInput.currentControlScheme == mouseScheme && previousControlScheme != mouseScheme)
        {
            theLevelManager.usingXboxController = false;
            cursorTransform.gameObject.SetActive(false);
            if (theLevelManager.menuScene)
            {
                Cursor.visible = true; //unless outside of menu
            } else if (theLevelManager.inMenu)
            {
                Cursor.visible = true;
            }
            currentMouse.WarpCursorPosition(virtualMouse.position.ReadValue());
            previousControlScheme = mouseScheme;
        } else if(playerInput.currentControlScheme == gamepadScheme && previousControlScheme != gamepadScheme)
        { 
            theLevelManager.usingXboxController = true;
            if (theLevelManager.menuScene)
            {
                cursorTransform.gameObject.SetActive(true); //unless outside of menu
            } else if (theLevelManager.inMenu)
            {
                cursorTransform.gameObject.SetActive(true);
            }
            Cursor.visible = false;
            InputState.Change(virtualMouse.position, currentMouse.position.ReadValue());
            AnchorCursor(currentMouse.position.ReadValue());
            previousControlScheme = gamepadScheme;
        }
    }
}
