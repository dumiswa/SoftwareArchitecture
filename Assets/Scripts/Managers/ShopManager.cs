using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is used to open/hide shop/shop elements
public class ShopManager : MonoBehaviour
{
    //By using these these booleans the player can decide which towers will be displayed in the shop
    [Header("Weapons to be bought in the shop")]
    [SerializeField] private bool toHaveASingleAttackWeapon;
    [SerializeField] private bool toHaveASpellingkWeapon;
    [SerializeField] private bool toHaveAoeWeapon;

    [SerializeField] private GameObject[] weaponButtonHolders;

    [Header("UI elements")]
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject shopPanel;

    void Awake()
    {
        HideShopCanvas();
        HideOptions();

        weaponButtonHolders[0].SetActive(toHaveASingleAttackWeapon);
        weaponButtonHolders[1].SetActive(toHaveASpellingkWeapon);
        weaponButtonHolders[2].SetActive(toHaveAoeWeapon);
    }

    //Subscribing to the UIEventHandler actions
    private void Start()
    {
        UIEventHandler.Instance.onTileClicked += OpenShopCanvas;
        UIEventHandler.Instance.onShopClosed += HideShopCanvas;
        UIEventHandler.Instance.onOptionsOpen += OpenOptions;
        UIEventHandler.Instance.onTowerApproved += HideShopCanvas;
        UIEventHandler.Instance.onTowerDisapproved += HideOptions;
    }

    //Unsubscribing from the UIEventHandler actions
    private void OnDisable()
    {
        UIEventHandler.Instance.onTileClicked -= OpenShopCanvas;
        UIEventHandler.Instance.onShopClosed -= HideShopCanvas;
        UIEventHandler.Instance.onOptionsOpen -= OpenOptions;
        UIEventHandler.Instance.onTowerApproved -= HideShopCanvas;
        UIEventHandler.Instance.onTowerDisapproved -= HideOptions;
    }

    //Hide the shop
    private void HideShopCanvas()
    {
        shopPanel.SetActive(false);
        HideOptions();
    }

    //Open the shop
    private void OpenShopCanvas()
    {
        shopPanel.SetActive(true);
    }

    //Hide approval and disapproval buttons
    private void HideOptions()
    {
        optionsPanel.SetActive(false);
    }

    //Open approval and disapproval buttons
    private void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }

}
