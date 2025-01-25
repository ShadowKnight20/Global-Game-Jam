using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Required for SceneManager
public class ResourceManager : MonoBehaviour
{
    // Resource variables
    public int wood;
    public int stone;
    public int gold;
    public int food;

    public int employed;
    public int unemployed;
    public int population;

    // UI Texts
    public Text woodText;
    public Text stoneText;
    public Text goldText;
    public Text foodText;

    public Text populationText;
    public Text unemployedText;
    public Text employedText;

    private static ResourceManager _instance; // Private instance variable

    public static ResourceManager Instance // Public property to access the instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ResourceManager>(); // Find the instance in the scene
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Make this object persistent
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Register for scene loaded
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unregister on disable
    }

    void Start()
    {
        LoadResources(); // Load resources when the game starts
        FindUIElements(); // Find UI elements dynamically
        UpdateUI();
    }

    void Update()
    {
        PreventNegativeResources();
        UpdateUI();
    }

    void PreventNegativeResources()
    {
        if (unemployed < 0) unemployed = 0; // Prevent negative unemployed
    }

    void FindUIElements()
    {
        woodText = GameObject.Find("WoodText")?.GetComponent<Text>();
        stoneText = GameObject.Find("StoneText")?.GetComponent<Text>();
        goldText = GameObject.Find("GoldText")?.GetComponent<Text>();
        foodText = GameObject.Find("FoodText")?.GetComponent<Text>();

        populationText = GameObject.Find("PopulationText")?.GetComponent<Text>();
        unemployedText = GameObject.Find("UnemployedText")?.GetComponent<Text>();
        employedText = GameObject.Find("EmployedText")?.GetComponent<Text>();
    }

    void UpdateUI()
    {
        unemployed = population - employed;
        if (woodText != null) woodText.text = "Wood: " + wood;
        if (stoneText != null) stoneText.text = "Stone: " + stone;
        if (goldText != null) goldText.text = "Gold: " + gold;
        if (foodText != null) foodText.text = "Food: " + food;
        if (populationText != null) populationText.text = "Population: " + population;
        if (employedText != null) employedText.text = "Employed: " + employed;
        if (unemployedText != null) unemployedText.text = "Unemployed: " + unemployed;
    }

    public void AddWood() { wood += 5; SaveResources(); }
    public void AddStone() { stone += 5; SaveResources(); }
    public void AddGold() { gold += 5; SaveResources(); }
    public void AddFood() { food += 5; SaveResources(); }
    public void AddPopulation() { population += 1; SaveResources(); }
    public void AddEmployed() { employed += 1; SaveResources(); }

    public void RemoveWood() { if (wood > 0) wood -= 5; SaveResources(); }
    public void RemoveStone() { if (stone > 0) stone -= 5; SaveResources(); }
    public void RemoveGold() { if (gold > 0) gold -= 5; SaveResources(); }
    public void RemoveFood() { if (food > 0) food -= 5; SaveResources(); }
    public void RemovePopulation() { if (population > 0) population -= 1; SaveResources(); }
    public void RemoveEmployed() { if (employed > 0) employed -= 1; SaveResources(); }

    void SaveResources()
    {
        PlayerPrefs.SetInt("Wood", wood);
        PlayerPrefs.SetInt("Stone", stone);
        PlayerPrefs.SetInt("Gold", gold);
        PlayerPrefs.SetInt("Population", population);
        PlayerPrefs.SetInt("Employed", employed);
        PlayerPrefs.SetInt("Food", food);
        PlayerPrefs.Save();
    }

    void LoadResources()
    {
        wood = PlayerPrefs.GetInt("Wood", 0);
        stone = PlayerPrefs.GetInt("Stone", 0);
        gold = PlayerPrefs.GetInt("Gold", 0);
        food = PlayerPrefs.GetInt("Food", 0);

        population = PlayerPrefs.GetInt("Population", 0);
        employed = PlayerPrefs.GetInt("Employed", 0);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindUIElements();
        UpdateUI();
    }
}