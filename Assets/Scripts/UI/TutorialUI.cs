using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyMoveUp;
    [SerializeField] private TextMeshProUGUI keyMoveDown;
    [SerializeField] private TextMeshProUGUI keyMoveLeft;
    [SerializeField] private TextMeshProUGUI keyMoveRight;
    [SerializeField] private TextMeshProUGUI keyInteract;
    [SerializeField] private TextMeshProUGUI keyInteractAlternate;
    [SerializeField] private TextMeshProUGUI keyPause;

    private void Start()
    {
        GameInput.instance.onBindingRebind += GameInput_OnBindingRebind;
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStartChanged;


        UpdateVisual();
        Show();
        
    }
    
    private void GameInput_OnBindingRebind(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }
    private void KitchenGameManager_OnStartChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.isCountdown())
        {
            Hide();
        }
    }

    private void UpdateVisual()
    {
        keyMoveUp.text = GameInput.instance.getBindingText(GameInput.Binding.Move_Up);
        keyMoveDown.text = GameInput.instance.getBindingText(GameInput.Binding.Move_Down);
        keyMoveLeft.text = GameInput.instance.getBindingText(GameInput.Binding.Move_Left);
        keyMoveRight.text = GameInput.instance.getBindingText(GameInput.Binding.Move_Right);
        keyInteract.text = GameInput.instance.getBindingText(GameInput.Binding.Interact);
        keyInteractAlternate.text = GameInput.instance.getBindingText(GameInput.Binding.Interact_Alternate);
        keyPause.text = GameInput.instance.getBindingText(GameInput.Binding.Pause);

    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
