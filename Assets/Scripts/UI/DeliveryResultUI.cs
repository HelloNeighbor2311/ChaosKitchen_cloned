using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeliveryResultUI : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failColor;
    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failSprite;

    private Animator animator;
    private const string popup = "PopUp";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

        gameObject.SetActive(false);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        backgroundImage.color = successColor;
        iconImage.sprite = successSprite;
        text.text = "DELIVERY\nSUCCESS";
        gameObject.SetActive(true);
        animator.SetTrigger(popup);
    }
    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        backgroundImage.color = failColor;
        iconImage.sprite = failSprite;
        text.text = "DELIVERY\nFAILED";
        gameObject.SetActive(true);
        animator.SetTrigger(popup);
    }
}
