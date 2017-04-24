using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//0 nothing
//1 satellite
//2 lab
//3 mine
//4 generator
//5 booster
//6 factory

public class Slot {
    private int index;
    public int building;
    public GameObject parent;
    public GameObject buildingGO;
    public int plutonium = 100;
    public int level = 0;
    public bool enabled = true;
    public string info;

    public Slot(int _index) {
        index = _index;
    }

    public int GetLevel() {
        int genCount = GameManager.instance.GetBoosterAmount(GameManager.instance.GetAdjacentSlots(index));
        return genCount + level;
    }
}

public static class Rates {
    public static int[] labGen = new int[] { 5, 7, 9, 13, 17 };

    public static int[] mineGen = new int[] { 10, 14, 18, 26, 34 };
    public static int[] mineDrain = new int[] { 1, 1, 1, 1, 1 };

    public static int[] boostGen = new int[] { 5, 10, 15, 20, 25 };
    public static int[] boostDrain = new int[] { 5, 6, 7, 8 ,9 };

    public static int[] pwstGen = new int[] { 5, 10, 15, 20, 25 };

    public static int[] facGen = new int[] { 5, 10, 15, 25, 50 };
    public static int[] facDrain = new int[] { 5, 6, 7, 10, 15 };

    public static int labCost = 100;
    public static int mineCost = 150;
    public static int boostCost = 150;
    public static int pwstCost = 200;
    public static int facCost = 250;
}

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public Slot[] slots;
    public GameObject[] prefabs;


    public int currentSlot = 0;

    private float speedUp = 0;

    public int energy;
    public int rsrc;

    public float gameRate = 0.5f;

    public bool mainLoopRunning = true;
    public bool unpaused = false;

    public MenuManager menuManager;

    private int month = 0;
    public int year = 0;

    public Newscaster human;
    public Newscaster alien;

    public GameObject badLaser;
    public GameObject goodLaser;

    public GameObject whiteCurtain;
    public static string overText;

    IEnumerator MainLoop() {
        while (mainLoopRunning) {
            while (unpaused) {
                yield return new WaitForSeconds(gameRate);
                AdvanceGame();
            }
            yield return new WaitForSeconds(gameRate);
        }
        yield return null;
    }

    public void StartGame() {
        unpaused = true;
        human.CheckYear();
        alien.CheckYear();

        CreateBuilding(0, 1);
        StartCoroutine(MainLoop());
    }

    void Awake() {
        instance = this;

        slots = new Slot[6];
        for (int i =0; i < slots.Length; i++) {
            slots[i] = new Slot(i);
            slots[i].parent = transform.GetChild(i).GetChild(0).gameObject;
        }
    }

    IEnumerator LoadEnd() {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
    }

    void GameOver() {
        badLaser.SetActive(true);
        Camera.main.gameObject.GetComponent<CameraShake>().shakeDuration = 2;
        overText = "Failure.";
        StartCoroutine(LoadEnd());
    }

    public void Success() {
        goodLaser.SetActive(true);
        Camera.main.gameObject.GetComponent<CameraShake>().shakeDuration = 2;
        overText = "Success.";
        StartCoroutine(LoadEnd());
    }

    void AdvanceGame() {
        month++;
        if (month > 11) {
            month = 0;
            year++;
            if (year == 30) {
                GameOver();
            }
            human.CheckYear();
            alien.CheckYear();
        }

        for (int i = 0; i < slots.Length; i++) {
            if (slots[i].building == 2) {
                int level = slots[i].GetLevel();
                int amount = Rates.labGen[level];
                slots[i].info = "Plutonite generation:\n    <color=green>•" + amount + " kg/mo</color>";
                rsrc += amount;
            }

            if (slots[i].building == 3) {
                if (slots[i].plutonium > 0) {
                    int level = slots[i].GetLevel();
                    int amount = Rates.mineGen[level];
                    slots[i].info = "Plutonite generation:\n    <color=green>•" + amount + " kg/mo</color>";
                    rsrc += amount;
                    slots[i].plutonium -= Mathf.Min(slots[i].plutonium, Rates.mineDrain[level]);
                }               
            }

            if (slots[i].building == 4) {
                int level = slots[i].GetLevel();
                int amount = Rates.pwstGen[level];
                slots[i].info = "Energy generation:\n   <color=green>• " + amount + " kJ/mo</color>";
                energy += amount;
            }

            if (slots[i].building == 5) {
                int level = slots[i].GetLevel();
                int amount = Rates.boostDrain[level];
                slots[i].info = "Energy drain:\n    <color=red>•" + amount + " kJ/mo</color>";
                if (energy >= amount) {
                    energy -= amount;
                    slots[i].enabled = true;
                } else slots[i].enabled = false;
            }

            if (slots[i].building == 6) {
                int level = slots[i].GetLevel();
                int drainAmount = Rates.facDrain[level];
                int genAmount = Rates.facGen[level];
                slots[i].info = "Plutonite drain:\n    <color=red>•" + drainAmount + " kg/mo</color>";
                slots[i].info += "\nEnergy generation:\n    <color=green>•" + genAmount + " kJ/mo</color>";
                if (rsrc >= drainAmount) {
                    rsrc -= drainAmount;
                    energy += genAmount;
                }
            }

            if (slots[i].building > 1) {
                slots[i].info += "\nBuilding level:\n    <color=green>•" + (slots[i].GetLevel() + 1) + "</color>";
            }
        }
    }

    public Slot[] GetAdjacentSlots(int slot) {
        Slot[] toReturn = new Slot[2];
        toReturn[0] = slots[(slot + 1) % 6];
        slot--;
        if (slot < 0) slot += 6;
        toReturn[1] = slots[slot];
        return toReturn;
    }

    public int GetBoosterAmount(Slot[] adjacent) {
        int count = 0;
        for (int i = 0; i < adjacent.Length; i++) {
            if (adjacent[i].building == 5 && adjacent[i].enabled) count++;
        }
        return count;
    }

    public void RotateLeft() {
        if (speedUp == 0) {
            currentSlot = (currentSlot + 1) % 6;
        }
        menuManager.ReadjustMenu();
    }

    public void RotateRight() {
        if(speedUp == 0) {
            currentSlot--;
            if (currentSlot < 0) currentSlot += 6;
        }
        menuManager.ReadjustMenu();
    }

    public void DestroyBuilding(int slot) {
        if (slots[slot].building > 1) {
            rsrc += 100;
            slots[slot].building = 0;
            slots[slot].level = 0;
            Destroy(slots[slot].buildingGO);
        }
    }

    public int GetBuildingCost(int building) {
        int cost = 0;
        if (building == 2) cost = Rates.labCost;
        if (building == 3) cost = Rates.mineCost;
        if (building == 4) cost = Rates.pwstCost;
        if (building == 5) cost = Rates.boostCost;
        if (building == 6) cost = Rates.facCost;
        return cost;
    }

    public bool CanAffordBuilding(int building) {
        int cost = GetBuildingCost(building);
        return rsrc >= cost;
    }

    public void CreateBuilding(int slot, int building) {
        if (building == 0) return;
        if (slots[slot].building == 0) {
            int cost = GetBuildingCost(building);
            if (rsrc >= cost) {
                rsrc -= cost;
                slots[slot].building = building;
                slots[slot].buildingGO = Instantiate(prefabs[building], slots[slot].parent.transform, false) as GameObject;
            }
        }
    }

    void Update() {
        if (Mathf.Abs((currentSlot * -60) - transform.eulerAngles.z)%360 > 0.001f) {
            speedUp += Time.deltaTime;
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, currentSlot * -60, Time.deltaTime * 500 * speedUp);
            transform.eulerAngles = new Vector3(0, 0, angle);
        } else {
            speedUp = 0;
        }
    }
}
