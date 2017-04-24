using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public string[] confirmTexts;

    public GameObject buildMenu;
    public GameObject confirmMenu;
    private GameObject currentMenu;
    public GameObject optionsMenu;
    public GameObject satelliteMenu;
    public Text confirmText;

    public Text upgradeText;
    public Text destroyText;

    public Animator animator;

    public Text infoText;

    private int confirmBuilding;

    private bool aboutToDestroy = false;

    public Button upgradeButton;

    string firstText;

    private int currentUpgradeCost = 75;

    public GameObject menu1;
    public GameObject menu2;
    private int tutState = 0;

    void Awake() {
        currentMenu = satelliteMenu;
        for (int i = 0; i < confirmTexts.Length; i++) {
            confirmTexts[i] = confirmTexts[i].Replace("<br>", "\n");
        }
        firstText = destroyText.text;
        menu1.SetActive(true);
    }

    public void ReadjustMenu() {
        int building = GameManager.instance.slots[GameManager.instance.currentSlot].building;
        if (building == 0) {
            SwitchToBuild();
        } else if (building == 1) {
            SwitchToDeploy();
        } else {
            SwitchToOptions();
        }

        ReadjustInfo();
    }

    public void AdvanceMenu() {
        if (tutState == 0) {
            tutState++;
            menu1.SetActive(false);
            menu2.SetActive(true);
        } else if (tutState == 1) {
            menu2.SetActive(false);
            GameManager.instance.StartGame();
        }
    }

    void ReadjustInfo() {
        Slot slot = GameManager.instance.slots[GameManager.instance.currentSlot];
        infoText.text = "Region info:\n";
        if (slot.building == 0) {
            infoText.text += "Plutonite Density:\n    <color=green>•" + slot.plutonium + "%</color>";
        } else if (slot.building == 1) {
            infoText.text += "Gain a total of\n10,000 kJ and press deploy to transmit the message!";
        } else {
            infoText.text += "Plutonite Density:\n    <color=green>•" + slot.plutonium + "%</color>\n" + slot.info;
        }
    }

    void Update() {
        ReadjustInfo();
    }

    public void SwitchToConfirm(int buildingType) {
        if (currentMenu == buildMenu) {
            confirmText.text = confirmTexts[buildingType];
            confirmBuilding = buildingType;
            currentMenu.SetActive(false);
            currentMenu = confirmMenu;
            confirmMenu.SetActive(true);
        }
    }

    public void CancelConfirm() {
        confirmBuilding = 0;
        SwitchToBuild();
    }

    public void ConfirmSelection() {
        if (GameManager.instance.CanAffordBuilding(confirmBuilding)) {
            GameManager.instance.CreateBuilding(GameManager.instance.currentSlot, confirmBuilding);
            SwitchToOptions();
        }
    }

    public void ToggleDrawer() {
        animator.SetBool("OnScreen", !animator.GetBool("OnScreen"));
    }

    public void SwitchToBuild() {
        aboutToDestroy = false;
        currentMenu.SetActive(false);
        currentMenu = buildMenu;
        buildMenu.SetActive(true);
    }

    public void SwitchToDeploy() {
        currentMenu.SetActive(false);
        currentMenu = satelliteMenu;
        satelliteMenu.SetActive(true);
    }

    public void Upgrade() {
        if (GameManager.instance.rsrc >= currentUpgradeCost && GameManager.instance.slots[GameManager.instance.currentSlot].level < 2) {
            GameManager.instance.rsrc -= currentUpgradeCost;
            GameManager.instance.slots[GameManager.instance.currentSlot].level++;
            CheckText();
           
        }
    }

    private void CheckText() {
        Slot current = GameManager.instance.slots[GameManager.instance.currentSlot];
        if (current.level == 0) currentUpgradeCost = 75;
        else if (current.level == 1) currentUpgradeCost = 200;
        if (current.building == 5) {
            upgradeText.text = "The Booster cannot be upgraded.";
            upgradeButton.image.color = Color.red;
            upgradeButton.interactable = false;
        } else if (current.level == 2) {
            upgradeText.text = "Current level: " + (current.level + 1) + "/3";
            upgradeButton.image.color = Color.red;
            upgradeButton.interactable = false;
        } else {
            upgradeText.text = "Current level: " + (current.level + 1) + "/3\nCost: " + currentUpgradeCost + " pl";
            upgradeButton.image.color = Color.white;
            upgradeButton.interactable = true;
        }
    }

    public void SwitchToOptions() {
        destroyText.text = firstText;
        CheckText();
        
        aboutToDestroy = false;
        currentMenu.SetActive(false);
        currentMenu = optionsMenu;
        optionsMenu.SetActive(true);
    }

    public void ConfirmDestroy() {
        if (!aboutToDestroy) {
            destroyText.text = "Press again to confirm.";
            aboutToDestroy = true;
        } else {
            GameManager.instance.DestroyBuilding(GameManager.instance.currentSlot);
            SwitchToBuild();
            //animator.SetBool("OnScreen", false);
        }
    }

    public void SwitchToSlot(int newSlot) {
        if(GameManager.instance.slots[newSlot].building == 0) {
            SwitchToBuild();
        } else if (GameManager.instance.slots[newSlot].building >= 1) {
            SwitchToOptions();
        }
    }

}
