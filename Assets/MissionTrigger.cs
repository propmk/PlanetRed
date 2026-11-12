using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MissionTrigger : MonoBehaviour
{
    [Header("UI Objects")]
    public GameObject missionPanel;      // Mission-1 panel (machine er kache gele auto khulbe)
    public GameObject collectTimeText;   // "Resources collect time 5 hours"
    public GameObject radiationText;     // "Radiation is increasing need to shelter"
    public GameObject abnormalHeartText; // NEW: "Abnormal Heart Condition Detected..."

    [Header("UI Text Displays")]
    public Text healthText;              // Health Display Text
    public Text armorText;               // Protective Armor Display Text

    [Header("Scene Settings")]
    public string gameOverSceneName = "mission fail";

    [Header("Player Stats")]
    public int health = 100;
    public int armor = 100;

    [Header("Timings")]
    public float collectTextDuration = 5f;     // text koto second thakbe
    public float armorDecreaseInterval = 10f;  // proti 10 sec e
    public int armorDecreaseAmount = 1;        // 1 kore armor kombe
    public float radiationDelay = 60f;         // 1 minute por
    public float healthDecreaseInterval = 1f;  // proti 1 sec e
    public int healthDecreaseAmount = 2;       // 2 kore health kombe

    private bool missionStarted = false;
    private bool isGameOver = false;
    private bool abnormalWarningShown = false; // Flag to track if warning is shown

    void Start()
    {
        if (missionPanel != null) missionPanel.SetActive(false);
        if (collectTimeText != null) collectTimeText.SetActive(false);
        if (radiationText != null) radiationText.SetActive(false);

        // Shurute abnormal heart text hide thakbe
        if (abnormalHeartText != null) abnormalHeartText.SetActive(false);

        UpdateUI();
    }

    void UpdateUI()
    {
        if (healthText != null) healthText.text = "Health : " + health;
        if (armorText != null) armorText.text = "Protective Armor Durability : " + armor;
    }

    // Machine er kache gele panel auto khulbe
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isGameOver && !missionStarted)
        {
            ShowPanel();
        }
    }

    // Machine theke sore gele panel vanish, tarpor timer shuru
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isGameOver)
        {
            if (missionPanel != null) missionPanel.SetActive(false);

            if (!missionStarted)
            {
                missionStarted = true;
                StartCoroutine(CollectTextRoutine());
                StartCoroutine(ArmorDecreaseRoutine());
                StartCoroutine(RadiationSequence());
            }
        }
    }

    void ShowPanel()
    {
        if (missionPanel == null) return;

        // Panel er parent gulo disable thakle shegulo active kore dey
        Transform t = missionPanel.transform;
        while (t != null)
        {
            t.gameObject.SetActive(true);
            t = t.parent;
        }

        // Panel ke shobar upore ane
        missionPanel.transform.SetAsLastSibling();
    }

    // "Resources collect time 5 hours" -> 5 second thakbe
    IEnumerator CollectTextRoutine()
    {
        if (collectTimeText != null) collectTimeText.SetActive(true);
        yield return new WaitForSeconds(collectTextDuration);
        if (collectTimeText != null) collectTimeText.SetActive(false);
    }

    // Proti 10 second e armor 1 kore kombe
    IEnumerator ArmorDecreaseRoutine()
    {
        while (!isGameOver && armor > 0)
        {
            yield return new WaitForSeconds(armorDecreaseInterval);

            armor -= armorDecreaseAmount;
            if (armor < 0) armor = 0;

            UpdateUI();
        }
    }

    // 1 minute por radiation text + health komano shuru
    IEnumerator RadiationSequence()
    {
        yield return new WaitForSeconds(radiationDelay);

        if (!isGameOver)
        {
            if (radiationText != null) radiationText.SetActive(true);
            StartCoroutine(HealthDecreaseRoutine());
        }
    }

    // Proti second e health 2 kore kombe
    IEnumerator HealthDecreaseRoutine()
    {
        while (!isGameOver && health > 0)
        {
            yield return new WaitForSeconds(healthDecreaseInterval);

            health -= healthDecreaseAmount;

            // Health jodi 80 er niche name ebong aage theke warning show na kora hoy
            if (health < 80 && !abnormalWarningShown)
            {
                abnormalWarningShown = true;
                if (abnormalHeartText != null) abnormalHeartText.SetActive(true);
            }

            if (health <= 0)
            {
                health = 0;
                UpdateUI();
                TriggerGameOver();
                break;
            }

            UpdateUI();
        }
    }

    void TriggerGameOver()
    {
        isGameOver = true;
        SceneManager.LoadScene(gameOverSceneName);
    }
}