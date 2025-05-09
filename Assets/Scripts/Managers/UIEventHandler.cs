using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//This class is used to notify its observers about its actions based on UI elements that this class has
public class UIEventHandler : MonoBehaviour
{
    public static UIEventHandler Instance { get; private set; }

    public Action onTileClicked;
    public Action onShopClosed;
    public Action onOptionsOpen;
    public Action onTowerApproved;
    public Action onTowerDisapproved;
    public Action<WeaponScriptableObject> onWeaponSelected;
    public Action<WeaponScriptableObject> onTowerSelected;
    public Action onTowerUpgradeOpen;
    public Action onTowerUpgradeClose;
    public Action onTowerDestroy;
    public Action<Vector3> onTileGetPosition;

    private ITower selectedTowerToUpgrade;
    private WeaponScriptableObject selectedAvailableWeapon;
    private Color originalTextColor;

    [Header("Price colors")]
    [SerializeField] private Color green;
    [SerializeField] private Color red;

    [Header("Weapon scriptable objects")]
    [SerializeField] private WeaponScriptableObject singleAttackWeapon;
    [SerializeField] private WeaponScriptableObject spellingWeapon;
    [SerializeField] private WeaponScriptableObject AOEAttackWeapon;

    [Header("Buttons related")]
    [SerializeField] private Button closeTheShopButton;

    [SerializeField] private Button singleAttackWeaponButton;
    [SerializeField] private Button spellingWeaponButton;
    [SerializeField] private Button AOEAttackWeaponButton;

    [SerializeField] private TMP_Text singleAttackWeaponButtonText;
    [SerializeField] private TMP_Text spellingWeaponButtonText;
    [SerializeField] private TMP_Text AOEAttackWeaponButtonText;

    [SerializeField] private TMP_Text singleAttackWeaponTextCost;
    [SerializeField] private TMP_Text spellingWeaponTextCost;
    [SerializeField] private TMP_Text AOEAttackWeaponTextCost;

    [SerializeField] private Button approvalButton;
    [SerializeField] private Button disapprovalButton;

    [Header("Info panel related")]
    [SerializeField] private TMP_Text waveInfoText;
    [SerializeField] private TMP_Text waveNumberText;
    [SerializeField] private TMP_Text moneyAmountText;

    [SerializeField] private Button restartButton;
    [SerializeField] private Button speedUpButton;

    [Header("End point text")]
    [SerializeField] private TMP_Text endPointText;

    [Header("Upgrade canvas related")]
    [SerializeField] private Image currentTowerImage;
    [SerializeField] private TMP_Text towerName;
    [SerializeField] private TMP_Text towerLevelText;

    [SerializeField] private TMP_Text rangeText;
    [SerializeField] private TMP_Text upgradeableRangeText;
    [SerializeField] private TMP_Text attackIntervalText;
    [SerializeField] private TMP_Text upgradeableAttackIntervalText;

    [SerializeField] private Button upgradeButton;
    [SerializeField] private TMP_Text upgradeButtonText;

    [SerializeField] private Button destroyButton;
    [SerializeField] private Button closeButton;

    [Header("Win/lose canvas related")]
    [SerializeField] private GameObject theEndPanel;
    [SerializeField] private Button restartButtonEndScreen;
    [SerializeField] private TMP_Text conditionText;

    void Awake()
    {
        //This class is used as a singleton in order for other classes to be able to reach this class
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        closeTheShopButton.onClick.AddListener(OnShopClosed);

        singleAttackWeaponButtonText.text = "Damage: " + singleAttackWeapon.damage + "\n" + "Range: " + singleAttackWeapon.range + "\n" + "Attack interval: " + singleAttackWeapon.attackInterval + "\n" + "Attack type: " + singleAttackWeapon.attackTypeDescription;
        spellingWeaponButtonText.text = "Damage: " + spellingWeapon.damage + "\n" + "Range: " + spellingWeapon.range + "\n" + "Attack interval: " + spellingWeapon.attackInterval + "\n" + "Attack type: " + spellingWeapon.attackTypeDescription;
        AOEAttackWeaponButtonText.text = "Damage: " + AOEAttackWeapon.damage + "\n" + "Range: " + AOEAttackWeapon.range + "\n" + "Attack interval: " + AOEAttackWeapon.attackInterval + "\n" + "Attack type: " + AOEAttackWeapon.attackTypeDescription;

        singleAttackWeaponTextCost.text = "$" + singleAttackWeapon.cost;
        spellingWeaponTextCost.text = "$" + spellingWeapon.cost;
        AOEAttackWeaponTextCost.text = "$" + AOEAttackWeapon.cost;

        singleAttackWeaponButton.onClick.AddListener(delegate { OnWeaponSelected(singleAttackWeapon); });
        spellingWeaponButton.onClick.AddListener(delegate { OnWeaponSelected(spellingWeapon); });
        AOEAttackWeaponButton.onClick.AddListener(delegate { OnWeaponSelected(AOEAttackWeapon); });

        approvalButton.onClick.AddListener(OnTowerApproved);
        disapprovalButton.onClick.AddListener(OnTowerDisapproved);

        closeButton.onClick.AddListener(OnUpgradeCanvasClose);

        originalTextColor = waveInfoText.color;

        destroyButton.onClick.AddListener(OnDestroyButton);
        upgradeButton.onClick.AddListener(OnUpgradeButton);

        theEndPanel.SetActive(false);
    }

    private void Start()
    {
        UpdateMoneyText();
        UpdateEnteredEnemiesText(0, GameManager.Instance.amountOfEnemiesThatCanEnter);

        restartButtonEndScreen.onClick.AddListener(GameManager.Instance.RestartTheGame);
        restartButton.onClick.AddListener(GameManager.Instance.RestartTheGame);
        speedUpButton.onClick.AddListener(GameManager.Instance.SetTheSpeed);

        GameManager.Instance.onGameWon += OnGameWon;
        GameManager.Instance.onGameLost += OnGameLost;
    }

    //Unsubscribing from the GameManager actions
    private void OnDisable()
    {
        GameManager.Instance.onGameWon -= OnGameWon;
        GameManager.Instance.onGameLost -= OnGameLost;
    }

    //This method shows the end screen with the text on it: "You lost"
    private void OnGameLost()
    {
        conditionText.text = "You lost";
        theEndPanel.SetActive(true);
    }

    //This method shows the end screen with the text on it: "You won"
    private void OnGameWon()
    {
        conditionText.text = "You won";
        theEndPanel.SetActive(true);
    }

    //This method is called from the SelectionManager to open the shop canvas
    public void OnTileSelected(Vector3 _tilePosition)
    {
        UpdateWeaponAvailability();
        onTileGetPosition?.Invoke(_tilePosition);
        onTileClicked?.Invoke();
    }

    //This method is called from the SelectionManager to open the upgrade window and set the values from the chosen tower to the corresponding UI elements
    public void OnTowerSelected(ITower _tower)
    {
        onTowerUpgradeOpen?.Invoke();

        selectedTowerToUpgrade = _tower;

        currentTowerImage.sprite = selectedTowerToUpgrade.GetWeaponScriptableObject().enemySprite;
        towerName.text = selectedTowerToUpgrade.GetWeaponScriptableObject().weaponName;

        if (selectedTowerToUpgrade.IsFullyUpgraded())
        {
            upgradeButton.enabled = false;
            upgradeButtonText.text = "Fully upgraded!";
            UpdateUpgradeAccessibilityText();

            rangeText.text = "Range: " + _tower.GetCurrentRange().ToString();
            upgradeableRangeText.text = "";

            attackIntervalText.text = "Attack interval: " + _tower.GetCurrentAttackInterval().ToString();
            upgradeableAttackIntervalText.text = "";

            towerLevelText.text = "Lv " + selectedTowerToUpgrade.GetTheCurrentLevel().ToString();
        }
        else
        {
            upgradeButtonText.text = "Upgrade - $" + selectedTowerToUpgrade.GetWeaponScriptableObject().upgradeCost;
            UpdateUpgradeAccessibilityText();

            rangeText.text = "Range: " + _tower.GetCurrentRange().ToString();
            upgradeableRangeText.text = "+ " + selectedTowerToUpgrade.GetWeaponScriptableObject().rangeToAdd.ToString();

            attackIntervalText.text = "Attack interval: " + _tower.GetCurrentAttackInterval().ToString();
            upgradeableAttackIntervalText.text = "- " + selectedTowerToUpgrade.GetWeaponScriptableObject().attackIntervalToSubstract.ToString();

            towerLevelText.text = "Lv " + selectedTowerToUpgrade.GetTheCurrentLevel().ToString();
        }

    }

    //This method destroys the tower if the destroy button has been clicked from the upgrade window
    private void OnDestroyButton()
    {
        if (selectedTowerToUpgrade == null) return;
        selectedTowerToUpgrade.Destroy();
        onTowerDestroy?.Invoke();
        OnUpgradeCanvasClose();
    }

    //This method upgrades the tower(sets the upgarded values), reduces total amount of money based on the upgrade cost
    private void OnUpgradeButton()
    {
        if (selectedTowerToUpgrade == null || GameManager.Instance.totalMoney < selectedTowerToUpgrade.GetWeaponScriptableObject().upgradeCost) return;

        GameManager.Instance.totalMoney -= selectedTowerToUpgrade.GetWeaponScriptableObject().upgradeCost;
        UpdateMoneyText();
        selectedTowerToUpgrade.Upgrade(selectedTowerToUpgrade.GetWeaponScriptableObject().rangeToAdd, selectedTowerToUpgrade.GetWeaponScriptableObject().attackIntervalToSubstract);
        OnUpgradeCanvasClose();
    }

    //This method is called to notify observers that the upgrade window has been closed
    private void OnUpgradeCanvasClose()
    {
        onTowerUpgradeClose?.Invoke();
        upgradeButton.enabled = true;
        selectedTowerToUpgrade = null;
    }

    //This method sets the color of the upgrade text
    //If you don't have enough money it's red, if you have enough money then it's green, if the tower is fully upgraded then it's white
    private void UpdateUpgradeAccessibilityText()
    {
        if (selectedTowerToUpgrade == null) return;

        if (selectedTowerToUpgrade.IsFullyUpgraded())
        {
            upgradeButtonText.color = Color.white;
        }
        else
        {
            upgradeButtonText.color = GameManager.Instance.totalMoney >= selectedTowerToUpgrade.GetWeaponScriptableObject().upgradeCost ? green : red;
        }
    }

    //Sets the timer value
    public void OnTimerSet(string _text, bool _isWaveComing)
    {
        waveInfoText.text = _text;
        waveInfoText.color = _isWaveComing ? red : originalTextColor;
    }

    //Sets the current wave amount text
    public void OnWaveChanged(int _currentWave, int _totalWaves)
    {
        waveNumberText.text = "Wave: " + _currentWave + "/" + _totalWaves;
    }

    //Updates the text of the toatl amount of entered enemies
    public void UpdateEnteredEnemiesText(int _amountOfEnemiesThatHaveEntered, int _amountOfEnemiesThatCanEnter)
    {
        endPointText.text = _amountOfEnemiesThatHaveEntered.ToString() + "/" + _amountOfEnemiesThatCanEnter.ToString();
    }

    //Notifies its obsrevers when the shop has been closed
    private void OnShopClosed()
    {
        onShopClosed?.Invoke();
    }

    //Notifies its observers on which weapon(tower) has been selected in the shop
    private void OnWeaponSelected(WeaponScriptableObject _weaponScriptableObject)
    {
        if (GameManager.Instance.totalMoney >= _weaponScriptableObject.cost)
        {
            selectedAvailableWeapon = _weaponScriptableObject;
            onWeaponSelected?.Invoke(_weaponScriptableObject);
            onOptionsOpen?.Invoke();
        }
    }

    //Notifies its observers on which weapon(tower) has been approved in the shop
    //Reduces the amount of money based on the tower cost
    private void OnTowerApproved()
    {
        if (selectedAvailableWeapon != null)
        {
            GameManager.Instance.totalMoney -= selectedAvailableWeapon.cost;
            UpdateMoneyText();

            onTowerApproved?.Invoke();
        }
    }

    //Notifies its obsrevers when the selected weapon(tower) has been disapproved
    private void OnTowerDisapproved()
    {
        onTowerDisapproved?.Invoke();
    }

    //Sets the tower cost text to green if there's enough money, if not then the cost text is red
    private void UpdateWeaponAvailability()
    {
        singleAttackWeaponTextCost.color = GameManager.Instance.totalMoney >= singleAttackWeapon.cost ? green : red;
        spellingWeaponTextCost.color = GameManager.Instance.totalMoney >= spellingWeapon.cost ? green : red;
        AOEAttackWeaponTextCost.color = GameManager.Instance.totalMoney >= AOEAttackWeapon.cost ? green : red;
    }

    //This method is called whenever the money amount changes
    public void UpdateMoneyText()
    {
        moneyAmountText.text = "Money: $" + GameManager.Instance.totalMoney.ToString();
        UpdateUpgradeAccessibilityText();
        UpdateWeaponAvailability();
    }

    

}
