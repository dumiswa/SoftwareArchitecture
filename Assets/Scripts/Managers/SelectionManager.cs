using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is responsible for the tile selection and tower selection if the player wants to upgrade it
public class SelectionManager : MonoBehaviour
{
    [SerializeField] private string tileTag = "Tile";
    [SerializeField] private string towerTag = "Tower";

    private Transform currentTile;
    private Transform selectedTile;

    private bool aTileBeingSelected = false;
    private bool aTowerBeingUpgraded = false;

    private ITower selectedTowerToUpgrade;

    [Header("Meterials")]
    [SerializeField] private Material highlightedMaterial;
    [SerializeField] private Material originalMaterial;

    //Subscribing to the UIEventHandler actions
    private void Start()
    {
        UIEventHandler.Instance.onShopClosed += OnSelectionEnabled;
        UIEventHandler.Instance.onTowerApproved += OnTowerApproved;

        UIEventHandler.Instance.onTowerUpgradeClose += OnSelectionEnabled;
    }

    //Unsubscribing from the UIEventHandler actions
    private void OnDisable()
    {
        UIEventHandler.Instance.onShopClosed -= OnSelectionEnabled;
        UIEventHandler.Instance.onTowerApproved -= OnTowerApproved;

        UIEventHandler.Instance.onTowerUpgradeClose -= OnSelectionEnabled;
    }

    void Update()
    {
        RayCheck();
    }

    //This method checks if the raycast is hitting a transform with the needed tag
    void RayCheck()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Transform hitTransform = hit.transform;

            if (hitTransform.CompareTag(tileTag))
            {
                if (!aTileBeingSelected && !hitTransform.GetComponent<SelectableTile>().isTaken && !aTowerBeingUpgraded)
                {
                    CheckIfMouseIsPressedOverATile(hitTransform);

                    if (hitTransform != currentTile)
                    {
                        if (currentTile != null)
                        {
                            SetTheMaterial(currentTile.GetComponent<MeshRenderer>(), originalMaterial);
                        }

                        SetTheMaterial(hitTransform.GetComponent<MeshRenderer>(), highlightedMaterial);
                        currentTile = hitTransform;
                    }
                }
            }

            else if (hitTransform.CompareTag(towerTag))
            {
                if (!aTileBeingSelected)
                {
                    CheckIfMouseIsPressedOverATower(hitTransform);

                    if (currentTile != null && !aTileBeingSelected)
                    {
                        SetTheMaterial(currentTile.GetComponent<MeshRenderer>(), originalMaterial);
                        currentTile = null;
                    }
                }
            }

            else
            {
                if (currentTile != null && !aTileBeingSelected)
                {
                    SetTheMaterial(currentTile.GetComponent<MeshRenderer>(), originalMaterial);
                    currentTile = null;
                }
            }
        }
    }

    //Called when the tower chosen from the shop gets approved, resets the chosen tile
    private void OnTowerApproved()
    {
        OnSelectionEnabled();

        if (selectedTile != null)
        {
            SetTheMaterial(currentTile.GetComponent<MeshRenderer>(), originalMaterial);
            selectedTile = null;
        }
    }

    //This method checks if the mouse has been pressed while being at the tile transfrom
    //Calls UIEventHandler.Instance.OnTileSelected() to transfer the needed tile position
    private void CheckIfMouseIsPressedOverATile(Transform _tileTransform)
    {
        if (Input.GetMouseButtonDown(0))
        {
            UIEventHandler.Instance.OnTileSelected(_tileTransform.position);
            aTileBeingSelected = true;

            selectedTile = _tileTransform;
            BuildingManager.Instance.tile = selectedTile.GetComponent<SelectableTile>();
        }
    }

    //This method checks if the mouse has been pressed while being at the tower transfrom
    //Calls UIEventHandler.Instance.OnTowerSelected() to transfer the needed ITower class
    private void CheckIfMouseIsPressedOverATower(Transform _towerTransform)
    {
        if (Input.GetMouseButtonDown(0))
        {
            UIEventHandler.Instance.OnTowerSelected(_towerTransform.GetComponent<ITower>());
            selectedTowerToUpgrade = _towerTransform.GetComponent<ITower>();
            aTowerBeingUpgraded = true;
        }
    }

    //This method enables the selection again once tile selection/tower upgrade has ended
    private void OnSelectionEnabled()
    {
        aTileBeingSelected = false;
        aTowerBeingUpgraded = false;
        selectedTowerToUpgrade = null;
    }

    //Seeting the correct material to the tile
    void SetTheMaterial(MeshRenderer _meshRenderer, Material _material)
    {
        Material[] materialsArray = _meshRenderer.materials;
        materialsArray[1] = _material;
        _meshRenderer.materials = materialsArray;
    }


}
