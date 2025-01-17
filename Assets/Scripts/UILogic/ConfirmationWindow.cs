using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationWindow : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text messageText;
    public Button confirmButton;
    public Button cancelButton;

    private System.Action onConfirm;
    private System.Action onCancel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panel.SetActive(false);

        confirmButton.onClick.AddListener(HandleConfirm);
        cancelButton.onClick.AddListener(HandleCancel);
    }

    public void ShowConfirmation(string message, System.Action confirmAction, System.Action cancelAction)
    {
        messageText.text = message;

        onConfirm = confirmAction;
        onCancel = cancelAction;

        panel.SetActive(true);
    }

    public void HideConfirmation()
    {
        panel.SetActive(false);

        onConfirm = null;
        onCancel = null;
    }


    private void HandleConfirm()
    {
        onConfirm?.Invoke();

        HideConfirmation();
    }
    private void HandleCancel()
    {
        onCancel?.Invoke();

        HandleCancel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
