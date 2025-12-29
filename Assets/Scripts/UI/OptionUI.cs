using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class OptionUI : MonoBehaviour
{
    [SerializeField] private Button musicBtn;
    [SerializeField] private Button soundEffectBtn;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button moveUpBtn;
    [SerializeField] private Button moveDownBtn;
    [SerializeField] private Button moveLeftBtn;
    [SerializeField] private Button moveRightBtn;
    [SerializeField] private Button interactBtn;
    [SerializeField] private Button interactAlternateBtn;
    [SerializeField] private Button pauseBtn;

    [SerializeField] private TextMeshProUGUI soundEffectText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAlternateText;
    [SerializeField] private TextMeshProUGUI pauseText;

    [SerializeField] private Transform pressToRebindTransform;

    private Action onCloseBtnAction;

    public static OptionUI Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
        musicBtn.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        soundEffectBtn.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeBtn.onClick.AddListener(() =>
        {
            Hide();
            onCloseBtnAction();
        });

        //cài đặt hàm rebind cho nút
        moveUpBtn.onClick.AddListener(() => { rebindBinding(GameInput.Binding.Move_Up); });
        moveDownBtn.onClick.AddListener(() => { rebindBinding(GameInput.Binding.Move_Down); });
        moveLeftBtn.onClick.AddListener(() => { rebindBinding(GameInput.Binding.Move_Left); });
        moveRightBtn.onClick.AddListener(() => { rebindBinding(GameInput.Binding.Move_Right); });
        interactBtn.onClick.AddListener(() => { rebindBinding(GameInput.Binding.Interact); });
        interactAlternateBtn.onClick.AddListener(() => { rebindBinding(GameInput.Binding.Interact_Alternate); });
        pauseBtn.onClick.AddListener(() => { rebindBinding(GameInput.Binding.Pause); });


    }
    private void Start()
    {
        KitchenGameManager.Instance.OnGameResumed += KitchenManager_OnGameResumed;

        UpdateVisual();
        HideToRebindKey();
        Hide();
    }

    private void KitchenManager_OnGameResumed(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        soundEffectText.text ="Sound Effect: " +  Mathf.Round(SoundManager.Instance.GetVolume()*10f);
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);

        moveUpText.text = GameInput.instance.getBindingText(GameInput.Binding.Move_Up); //Thay đổi ký tự trên nút bên trong gameScene
        moveDownText.text = GameInput.instance.getBindingText(GameInput.Binding.Move_Down);
        moveLeftText.text = GameInput.instance.getBindingText(GameInput.Binding.Move_Left);
        moveRightText.text = GameInput.instance.getBindingText(GameInput.Binding.Move_Right);
        interactText.text = GameInput.instance.getBindingText(GameInput.Binding.Interact);
        interactAlternateText.text = GameInput.instance.getBindingText(GameInput.Binding.Interact_Alternate);
        pauseText.text = GameInput.instance.getBindingText(GameInput.Binding.Pause);

    }

    public void Show(Action onCloseBtnAction)
    {
        this.onCloseBtnAction = onCloseBtnAction;
        gameObject.SetActive(true);
        soundEffectBtn.Select();
    }
    public void Hide() {
        gameObject.SetActive(false);
    }

    private void ShowToRebindKey()
    {
        pressToRebindTransform.gameObject.SetActive(true);
    }
    private void HideToRebindKey() {
        pressToRebindTransform.gameObject.SetActive(false);
    }

    private void rebindBinding(GameInput.Binding binding) {
        ShowToRebindKey();
        GameInput.instance.RebindBinding(binding, ()=> { 
            HideToRebindKey(); 
            UpdateVisual();
        }); //cài đặt nút

    }
}

