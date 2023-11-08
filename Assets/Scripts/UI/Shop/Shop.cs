/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Galaxy.UI.Labels;
using GalaxyObject = Galaxy.Objects.GalaxyObject;

namespace Galaxy.UI {

    /// <summary>
    /// A shop from which items can be purchased.
    /// </summary>
    public class Shop : MonoBehaviour {

        /* --- Static --- */
        public static float ButtonRadius = 0.35f;

        [System.Serializable]
        public struct ShopItem {
            public GalaxyObject ItemObject;
            public float Cost;
        }
        public List<ShopItem> m_ShopItems;

        /* --- Properties --- */
        public List<Item> m_Items;
        public Item m_ItemBase;

        //* --- Unity --- */
        // Runs once before the first frame.
        void Start() {
            // Set up the shopping list.
            m_Items = new List<Item>();

            // Create the item labels from the shop items.
            int index = 0;
            float scale = 3f;
            foreach (ShopItem shopItem in m_ShopItems) {

                Item item = Instantiate(m_ItemBase.gameObject).GetComponent<Item>();

                item.gameObject.SetActive(true);
                item.transform.parent = transform;
                item.Name = shopItem.ItemObject.gameObject.name;
                item.Cost = shopItem.Cost;
                item.ItemObject = shopItem.ItemObject;
                item.transform.localPosition = Vector2.right * scale * index;

                // shopItem.SetUI(transform, ButtonRadius);
                index += 1;
                m_Items.Add(item);

            }
        }

    }

}
