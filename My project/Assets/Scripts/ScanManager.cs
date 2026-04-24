using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class ScanManager : MonoBehaviour
{
    [Header("Scanning")]
    public float scanRange = 100f;
    public Camera playerCamera;
    public LayerMask scanMask;

    [Header("Scan Popup")]
    public GameObject scanPopup;
    public TMPro.TextMeshProUGUI popupNameText;
    public UnityEngine.UI.Image popupImage;
    [Header("Journal")]
    public GameObject journalPanel;
    public Image entryImage;
    public TMPro.TextMeshProUGUI entryNameText;
    public TMPro.TextMeshProUGUI entryDescriptionText;
    public Button prevButton;
    public Button nextButton;

    [Header("Audio")]
    public AudioClip pageTurnSound;
    public AudioClip bookCloseSound;
    public AudioClip quillScratchSound;

    private AudioSource audioSource;

    // Internal
    private List<ScannableData> scannedEntries = new List<ScannableData>();
    private int currentPage = 0;
    public static bool journalOpen = false;
    private Coroutine popupCoroutine;

    [System.Serializable]
    public class ScannableData
    {
        public string displayName;
        public string description;
        public Sprite image;
    }

    void Start()
    {
        scanPopup.SetActive(false);
        journalPanel.SetActive(false);
        prevButton.onClick.AddListener(PrevPage);
        nextButton.onClick.AddListener(NextPage);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
            TryScanning();

        if (Keyboard.current.jKey.wasPressedThisFrame)
            ToggleJournal();
    }

    void TryScanning()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = playerCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, scanRange, scanMask))
        {
            Scannable target = hit.collider.GetComponent<Scannable>();
            if (target != null)
            {
                ShowPopup(target.displayName, target.journalImage);

                // Only add if not already scanned
                if (!scannedEntries.Exists(e => e.displayName == target.displayName))
                {
                    ScannableData data = new ScannableData
                    {
                        displayName = target.displayName,
                        description = target.description,
                        image = target.journalImage
                    };
                    scannedEntries.Add(data);
                }
            }
        }
    }

    void ShowPopup(string name, Sprite image)
    {
        popupNameText.text = name;

        if (popupImage != null)
        {
            popupImage.sprite = image;
            popupImage.enabled = image != null;
        }

        scanPopup.SetActive(true);
        audioSource.PlayOneShot(quillScratchSound);

        if (popupCoroutine != null) StopCoroutine(popupCoroutine);
        popupCoroutine = StartCoroutine(HidePopupAfterDelay(2.5f));
    }

    IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        scanPopup.SetActive(false);
    }

    void ToggleJournal()
    {
        journalOpen = !journalOpen;
        journalPanel.SetActive(journalOpen);

        Cursor.lockState = journalOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = journalOpen;

        if (journalOpen)
        {
            audioSource.PlayOneShot(pageTurnSound);
            if (scannedEntries.Count > 0)
            {
                currentPage = Mathf.Clamp(currentPage, 0, scannedEntries.Count - 1);
                DisplayPage(currentPage);
            }
        }
        else
        {
            audioSource.PlayOneShot(bookCloseSound);
        }
    }

    void DisplayPage(int index)
    {
        if (scannedEntries.Count == 0) return;

        ScannableData entry = scannedEntries[index];

        entryNameText.text = entry.displayName;
        entryDescriptionText.text = entry.description;

        if (entryImage != null)
        {
            if (entry.image != null)
            {
                entryImage.sprite = entry.image;
                entryImage.color = new Color(1, 1, 1, 1);
            }
            else
            {
                entryImage.sprite = null;
                entryImage.color = new Color(1, 1, 1, 0);
            }
        }

        prevButton.interactable = index > 0;
        nextButton.interactable = index < scannedEntries.Count - 1;
    }

    void NextPage()
    {
        if (currentPage < scannedEntries.Count - 1)
        {
            currentPage++;
            DisplayPage(currentPage);
            audioSource.PlayOneShot(pageTurnSound);
        }
    }

    void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            DisplayPage(currentPage);
            audioSource.PlayOneShot(pageTurnSound);
        }
    }
}