using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is used to open/hide the upgrade window
public class TowerUpgradeManager : MonoBehaviour
{

    [SerializeField] private GameObject upgradePanel;

    private void Awake()
    {
        HideTowerUpgradeCanvas();
    }

    //Subscribing to the UIEventHandler actions
    private void Start()
    {
        UIEventHandler.Instance.onTowerUpgradeOpen += OpenTowerUpgradeCanvas;
        UIEventHandler.Instance.onTowerUpgradeClose += HideTowerUpgradeCanvas;
    }

    //Unsubscribing from the UIEventHandler actions
    private void OnDisable()
    {
        UIEventHandler.Instance.onTowerUpgradeOpen -= OpenTowerUpgradeCanvas;
        UIEventHandler.Instance.onTowerUpgradeClose -= HideTowerUpgradeCanvas;
    }

    //Hide the upgrade window
    private void HideTowerUpgradeCanvas()
    {
        upgradePanel.SetActive(false);
    }

    //Open the upgrade window
    private void OpenTowerUpgradeCanvas()
    {
        upgradePanel.SetActive(true);
    }
}
