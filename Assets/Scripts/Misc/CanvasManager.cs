using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button startButton;
    public Button exitButton;
    public Button settingButton;
    public Button returnButton;
    public Button returnToGame;
    public Button returnToMenu;
    public Button tryAgainButton;

    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject settingMenu;

    [Header("Text")]
    public Text liveText;
    public Text healthText;
    public Text staminaText;
    public Text sliderText;

    [Header("Slider")]
    public Slider volSlide;
    public Slider healthSlider;
    public Slider staminaSlider;

    public GameManager playerHealth;
    public Image imageFill;

    private void Awake()
    {
        healthSlider = GetComponent<Slider>();
        staminaSlider = GetComponent<Slider>();
    }
    void Start()
    {
        if (settingButton)
        {
            settingButton.onClick.AddListener(() => showSetMenu());
        }
        if (returnButton)
        {
            returnButton.onClick.AddListener(() => showMainMenu());
        }
        if (startButton)
        {
            startButton.onClick.AddListener(() => startGame());
        }
        if (tryAgainButton)
        {
            tryAgainButton.onClick.AddListener(() => tryAgain());
        }
        if (volSlide && sliderText)
        {
            volSlide.onValueChanged.AddListener((value) => OnSliderValueChange(value));
            sliderText.text = volSlide.value.ToString();
        }
        if (liveText)
        {
            GameManager.instances.onLifeEvent.AddListener((value) => OnLifeValueChange(value));
        }
        if (healthText)
        {
            GameManager.instances.onHealthEvent.AddListener((value) => OnHealthValueChange(value));
        }
        if (staminaText)
        {
            GameManager.instances.onStaminaEvent.AddListener((value) => OnStaminaValueChange(value));
        }
    }
    public void showMainMenu()
    {
        settingMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void showSetMenu()
    {
        settingMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void startGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void tryAgain()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    void OnSliderValueChange(float value)
    {
        sliderText.text = value.ToString();
    }
    void OnLifeValueChange(int value)
    {
        liveText.text = value.ToString();
    }
    void OnHealthValueChange(int value)
    {
        healthText.text = value.ToString();
    }
    void OnStaminaValueChange(int value)
    {
        staminaText.text = value.ToString();
    }

    void Update()
    {
        //if (healthSlider.value <= healthSlider.minValue)
        //{
        //    imageFill.enabled = false;
        //}
        //if (healthSlider.value > healthSlider.minValue && !imageFill.enabled)
        //{
        //    imageFill.enabled = true;
        //}
        //float fillValue = playerHealth.health / playerHealth.maxHp;
        //healthSlider.value = fillValue;

        if (pauseMenu)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                pauseMenu.SetActive(!pauseMenu.activeSelf);
                if (pauseMenu.activeSelf)
                {
                    Time.timeScale = 0f;
                }
                else
                {
                    Time.timeScale = 1f;
                }
            }
        }
    }
}
