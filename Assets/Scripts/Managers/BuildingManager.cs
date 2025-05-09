using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class that is responsible for placing the towers when you're selecting them in the shop window
public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    [SerializeField] private Vector3 offset;
    [SerializeField] private GameObject rangeCylinderPrefab;
    private GameObject rangeCylinder;

    private Vector3 tilePosition;
    private GameObject currentInstantiatedTower;

    private float rangeOfTheTower;

    [HideInInspector]public SelectableTile tile;

    //This class is used as a singleton in order for other classes to be able to reach this class
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    //Subscribing to the UIEventHandler actions
    private void Start()
    {
        UIEventHandler.Instance.onWeaponSelected += PlaceATower;
        UIEventHandler.Instance.onTileGetPosition += SetTheTilePosition;
        UIEventHandler.Instance.onShopClosed += RemoveTheTower;
        UIEventHandler.Instance.onTowerApproved += ApprovePlacedTower;
    }

    //Unsubscribing from the UIEventHandler actions
    private void OnDisable()
    {
        UIEventHandler.Instance.onWeaponSelected -= PlaceATower;
        UIEventHandler.Instance.onTileGetPosition -= SetTheTilePosition;
        UIEventHandler.Instance.onShopClosed -= RemoveTheTower;
        UIEventHandler.Instance.onTowerApproved -= ApprovePlacedTower;
    }

    //This method was implemented for testing purposes, to see the actual range of the tower
    private void OnDrawGizmos()
    {
        if (currentInstantiatedTower != null)
        {
            Gizmos.DrawWireSphere(currentInstantiatedTower.transform.position, rangeOfTheTower);
        }
    }

    //This method approves the tower that has been selcted from the shop
    private void ApprovePlacedTower()
    {
        if (tile == null) return;
        currentInstantiatedTower.GetComponent<ITower>().OnBuild(tile);
        currentInstantiatedTower = null;
        Destroy(rangeCylinder);
    }

    //This method is called in order to where the player has clicked, in order to know the tile position
    private void SetTheTilePosition(Vector3 _tilePosition)
    {
        tilePosition = _tilePosition;
    }

    //This method is used change the range cylinder size based on the chosen tower from the shop
    private void SetTheRangeCylinder(float _range)
    {
        rangeCylinder.transform.localScale = new Vector3(_range * 2, rangeCylinder.transform.localScale.y, _range * 2);
    }

    //Remove the tower that was chosen via shop, if the player closes the shop
    private void RemoveTheTower()
    {
        if (currentInstantiatedTower != null)
        {
            Destroy(currentInstantiatedTower);
            Destroy(rangeCylinder);
        }

        tile = null;
    }

    //Called when the player is clicking on different weapons in the shop
    //It removes the old one and places a new chosen tower
    private void PlaceATower(WeaponScriptableObject _weaponScriptableObject)
    {
        if (currentInstantiatedTower == null)
        {
            rangeOfTheTower = _weaponScriptableObject.range;
            rangeCylinder = Instantiate(rangeCylinderPrefab, tilePosition, Quaternion.identity);
            SetTheRangeCylinder(rangeOfTheTower);
            currentInstantiatedTower = Instantiate(_weaponScriptableObject.weaponPrefab, tilePosition + offset, _weaponScriptableObject.weaponPrefab.transform.rotation);
        }
        else
        {
            Destroy(currentInstantiatedTower);
            rangeOfTheTower = _weaponScriptableObject.range;
            SetTheRangeCylinder(rangeOfTheTower);
            currentInstantiatedTower = Instantiate(_weaponScriptableObject.weaponPrefab, tilePosition + offset, _weaponScriptableObject.weaponPrefab.transform.rotation);
        }

        
    }
}
