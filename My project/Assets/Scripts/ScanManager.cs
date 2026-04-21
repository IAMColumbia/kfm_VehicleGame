using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScanManager : MonoBehaviour
{
    [Header("Scanning")]
    public float scanRange = 20f;
    public Camera playerCamera;

    [Header("UI References")]
    public GameObject scanPopup;
    public TMPro.TextMeshProUGUI popupDescriptionText;
    public TMPro.TextMeshProUGUI popupNameText;
    public GameObject journalPanel;
    public Transform journalEntryContainer;
    public GameObject journalEntryPrefab;

    private List<string> scannedEntries = new List<string>();
    private Coroutine popupCoroutine;
    public static bool journalOpen = false;

    void Start()
    {
        scanPopup.SetActive(false);
        journalPanel.SetActive(false);
    }

    void Update()
    {
        // Scan on left click
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryScanning();
        }

        // Toggle journal with J
        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            ToggleJournal();
        }
    }

void TryScanning()
{
    Vector2 mousePos = Mouse.current.position.ReadValue();
    Ray ray = playerCamera.ScreenPointToRay(mousePos);

    Debug.Log($"Scanning from {mousePos}, ray direction: {ray.direction}");

    if (Physics.Raycast(ray, out RaycastHit hit, scanRange))
    {
        Debug.Log($"Hit: {hit.collider.gameObject.name}");

        Scannable target = hit.collider.GetComponent<Scannable>();
        if (target != null)
        {
            Debug.Log($"Scannable found: {target.displayName}");
            ShowPopup(target.displayName, target.description);

            if (!scannedEntries.Contains(target.displayName))
            {
                scannedEntries.Add(target.displayName);
                AddJournalEntry(target.displayName, target.description, target.journalImage);
            }
        }
        else
        {
            Debug.Log("Hit object has no Scannable component");
        }
    }
    else
    {
        Debug.Log("Raycast hit nothing");
    }
}

    void ShowPopup(string name, string description)
    {
        popupNameText.text = name;
        popupDescriptionText.text = description;
        scanPopup.SetActive(true);

        if (popupCoroutine != null) StopCoroutine(popupCoroutine);
        popupCoroutine = StartCoroutine(HidePopupAfterDelay(2.5f));
    }

    IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        scanPopup.SetActive(false);
    }

    void AddJournalEntry(string entryName, string description, Sprite image)
    {
        GameObject entry = Instantiate(journalEntryPrefab, journalEntryContainer);
        var texts = entry.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        texts[0].text = entryName;
        if (texts.Length > 1) texts[1].text = description;

        // Find the Image component and assign the sprite
        Image entryImage = entry.GetComponentInChildren<Image>();
        if (entryImage != null && image != null)
        {
            entryImage.sprite = image;
            entryImage.enabled = true;
        }
    }

    void ToggleJournal()
    {
        journalOpen = !journalOpen;
        journalPanel.SetActive(journalOpen);

        // Pause/unpause cursor for journal navigation
        Cursor.lockState = journalOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = journalOpen;
    }

    
}