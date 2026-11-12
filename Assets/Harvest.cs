using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Harvest : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject harvestButton;      // Canvas theke HarvestButton ekhane diben
    public TextMeshProUGUI foodText;      // Uporer "Food :" Text (TMP) ekhane diben

    private int foodCount = 0;            // Shuru-te food 0 thakbe
    private bool isPlayerInside = false;  // Player box-er vitore ache kina

    private void Start()
    {
        if (harvestButton != null)
        {
            harvestButton.SetActive(false);

            // Button-er default navigation off kora jate click focus-e atke na thake
            Button btn = harvestButton.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                Navigation nav = btn.navigation;
                nav.mode = Navigation.Mode.None;
                btn.navigation = nav;
            }

            // Mouse press (PointerDown) korar sathe sathe jeno 5 kore add hoy
            EventTrigger trigger = harvestButton.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = harvestButton.AddComponent<EventTrigger>();
            }
            trigger.triggers.Clear();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) => { AddFood(); });
            trigger.triggers.Add(entry);
        }

        UpdateFoodUI();
    }

    // LateUpdate sob script-er pore chole, tai FPSController cursor lock korte parbe na
    private void LateUpdate()
    {
        if (isPlayerInside)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Player Box Collider-er vitore dhukle button visible hobe
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;

            if (harvestButton != null)
            {
                harvestButton.SetActive(true);
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Player Box Collider theke ber hoye gele button invisible hobe
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;

            if (harvestButton != null)
            {
                harvestButton.SetActive(false);
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Prottek click-e sathe sathe 5 kore barbe
    public void AddFood()
    {
        foodCount += 5;
        UpdateFoodUI();
    }

    private void UpdateFoodUI()
    {
        if (foodText != null)
        {
            foodText.text = "Food : " + foodCount;
        }
    }
}