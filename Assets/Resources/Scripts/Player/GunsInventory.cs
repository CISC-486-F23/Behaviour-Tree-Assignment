using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class GunsInventory : MonoBehaviour {
    List<GameObject> gunsInInventory = new List<GameObject>();
    private GameObject pickedGun;
    private int activeWeapon = 0;
    private bool wait = true;

    private Transform hand;

    // For the interface
    private Image gunImage;
    private GameObject cartridgePanel;
    private GameObject ammoBagPanel;

    public Shoot ActiveGun
    {
        get { return HasGun ? gunsInInventory[activeWeapon].GetComponent<Shoot>() : null; }
    }

    public bool HasGun
    {
        get { return gunsInInventory.Count > 0; }
    }

    public bool CanShoot
    {
        get { return HasGun && ActiveGun.Loaded; }
    }

    public bool CanReload
    {
        get { return HasGun && !ActiveGun.FullyLoaded; }
    }

    public bool AmmoFull
    {
        get { return HasGun && !ActiveGun.AbleToRefill; }
    }

    public bool OutOfAmmo
    {
        get { return !HasGun || ActiveGun.OutOfAmmo; }
    }
    
    void Start() {
        hand = GameObject.Find("Hand").GetComponent<Transform>();
        gunImage = GameObject.Find("GunImage").GetComponent<Image>();
        cartridgePanel = GameObject.Find("CartridgePanel").transform.Find("Image").gameObject;
        ammoBagPanel = GameObject.Find("AmmoBagPanel").transform.Find("Image").gameObject;
    }

    IEnumerator waitToChange() {
        wait = false;
        yield return new WaitForSeconds(0.25f);
        wait = true;
    }

    public void ChangeWeapon(int delta) {
        if (wait && gunsInInventory.Count > 1){
            StartCoroutine(waitToChange());
        }
        
        gunsInInventory[activeWeapon].GetComponent<Shoot>().Reloading = false;
        gunsInInventory[activeWeapon].SetActive(false);
        
        if (activeWeapon == 0 && delta < 0)
            activeWeapon = gunsInInventory.Count - 1;
        else if (activeWeapon == gunsInInventory.Count - 1 && delta > 0)
            activeWeapon = 0;
        else
            activeWeapon += delta;

        gunsInInventory[activeWeapon].transform.position = hand.position;
        gunsInInventory[activeWeapon].GetComponent<Shoot>().RestartValues();
        gunsInInventory[activeWeapon].SetActive(true);

        UpdateGUI();
        gunsInInventory[activeWeapon].GetComponent<Shoot>().UpdateGUI();

        SoundManager.PlaySound("ShotgunLoad");
    }
    
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("ItemGun")) {
            string path = "Prefabs/Guns/Gun" + Regex.Replace(other.gameObject.name, "[^.0-9]", "");
            pickedGun = (GameObject) Resources.Load(path, typeof(GameObject));
            
            if (gunsInInventory.Count > 0) {
                gunsInInventory[activeWeapon].SetActive(false);
                gunsInInventory[activeWeapon].GetComponent<Shoot>().Reloading = false;
            }
            
            gunsInInventory.Add(Instantiate(pickedGun, transform.position, Quaternion.identity));

            activeWeapon = gunsInInventory.Count - 1;

            Destroy(other.gameObject);
            
            UpdateGUI();
            ammoBagPanel.SetActive(true);
            cartridgePanel.SetActive(true);

            SoundManager.PlaySound("LeatherInventory");
        }
    }

    private void UpdateGUI() {
        gunImage.sprite = gunsInInventory[activeWeapon].transform.Find("Gun").GetComponent<SpriteRenderer>().sprite;
        gunImage.color = new Color(gunImage.color.r, gunImage.color.g, gunImage.color.b, 1f);
    }

    public GameObject GetActiveWeapon() {
        if (gunsInInventory.Count > 0)
            return gunsInInventory[activeWeapon];
        return null;
    }
}
