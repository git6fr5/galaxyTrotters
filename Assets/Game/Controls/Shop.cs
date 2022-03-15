/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Shop : MonoBehaviour {

    /* --- Data --- */
    [System.Serializable]
    public class ShopItem {
        [HideInInspector] private Item m_Item;
        [SerializeField] private string m_Name;
        [SerializeField] private int m_Value;
        [Space(2), Header("Switches")]
        [SerializeField] public bool isPurchase;

        public ShopItem(Item item) {
            m_Item = item;
            m_Name = item.Name;
            m_Value = item.Value;
            isPurchase = false;
        }

        public void Get() {
            GameObject newObject = Instantiate(m_Item.gameObject, m_Item.transform.position, Quaternion.identity, null);
            newObject.SetActive(true);
        }
    }

    /* --- Properties --- */
    public List<ShopItem> m_ShopItems;

    //* --- Unity --- */
    // Runs once before the first frame.
    void Start() {
        Init();
    }

    private void Init() {
        // Set up the shop.
        m_ShopItems = new List<ShopItem>();
        foreach (Transform child in transform) {
            Item item = child.GetComponent<Item>();
            if (item != null) {
                m_ShopItems.Add(new ShopItem(item));
            }
        }
    }

    // Runs once every frame.
    void Update() {
        for (int i = 0; i < m_ShopItems.Count; i++) {
            if (m_ShopItems[i].isPurchase) {
                m_ShopItems[i].Get();
                m_ShopItems[i].isPurchase = false;
            }
        }
    }

}
