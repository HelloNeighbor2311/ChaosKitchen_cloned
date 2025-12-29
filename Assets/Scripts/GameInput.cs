using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput instance;

    public PlayerInputAction playerInputAction;
    public event EventHandler onInteractAction;
    public event EventHandler onInteractAlternateAction;
    public event EventHandler onPauseAction;
    public event EventHandler onBindingRebind;

    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public enum Binding
    {
        Move_Up,
        Move_Down, Move_Left, Move_Right, Interact, Interact_Alternate, Pause
    }

    private void Awake()
    {
        instance = this;
        playerInputAction = new PlayerInputAction();
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS)) //Set lại các nút đã lưu trước đó từ file json
        {
            playerInputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        playerInputAction.Player.Enable();
        playerInputAction.Player.Interact.performed += Interact_performed;
        playerInputAction.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputAction.Player.Pause.performed += Pause_performed;

       
       
    }

    private void OnDestroy()
    {
        playerInputAction.Player.Interact.performed -= Interact_performed;
        playerInputAction.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputAction.Player.Pause.performed -= Pause_performed;

        playerInputAction.Dispose();
    }
    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        onInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        onInteractAction?.Invoke(this, EventArgs.Empty);
    }
    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        onPauseAction?.Invoke(this, EventArgs.Empty);   
    }


    public Vector2 MovementNormalized()
    {
        //Đọc thông tin truyền vào từ bàn phím (setting sẵn trong InputAction)
        var inputVector = playerInputAction.Player.Movement.ReadValue<Vector2>();
        
        inputVector = inputVector.normalized;
       
        return inputVector;
    }

    public string getBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                return playerInputAction.Player.Movement.bindings[1].ToDisplayString(); //trả về thứ tự của InputAction movement trong Map, do WASD là tiêu đề, chiếm vị trí bindings[0] 
            case Binding.Move_Down:
                return playerInputAction.Player.Movement.bindings[2].ToDisplayString(); //nên các hành động ở bên trong nó sẽ lần lượt chiếm các vị trí từ 1-4
            case Binding.Move_Left:
                return playerInputAction.Player.Movement.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputAction.Player.Movement.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playerInputAction.Player.Interact.bindings[0].ToDisplayString(); //trả về giá trị thứ tự của InputAction Interact trong map
            case Binding.Interact_Alternate:
                return playerInputAction.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputAction.Player.Pause.bindings[0].ToDisplayString();

        }
    }
    //cài đặt hàm để có thể gắn lại nút bên trong option
    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = playerInputAction.Player.Movement;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playerInputAction.Player.Movement;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInputAction.Player.Movement;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInputAction.Player.Movement;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputAction.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.Interact_Alternate:
                inputAction = playerInputAction.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputAction.Player.Pause;
                bindingIndex = 0;
                break;
        }

        playerInputAction.Player.Disable(); //tắt ActionMap

        inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(callback =>
        {
            
            callback.Dispose(); //tắt callBack phòng trường hợp lỗi memory
            playerInputAction.Player.Enable(); //bật trở lại actionMap
            onActionRebound();  //delegate

            PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputAction.SaveBindingOverridesAsJson()); // lưu các nút vừa thay đổi vào một file json, sau đó là PlayerPrefs
            PlayerPrefs.Save();

            onBindingRebind?.Invoke(this, EventArgs.Empty);

        }).Start();
    }
}
