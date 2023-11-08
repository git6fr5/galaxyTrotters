/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Galaxy.Models;

namespace Galaxy {

    ///<summary>
    ///
    ///<summary>
    public class MeshIO : MonoBehaviour {

        public string filename = "";
        private string filepath = "C:\\Users\\git6f\\galaxyTrotters\\Assets\\Visuals\\Models\\";

        public void SaveMesh(MeshGenerator generator) {
            string json = JsonUtility.ToJson(generator.meshSettings);
            System.IO.File.WriteAllText(filepath + filename + ".json", json);
        }

        public void ReadMesh(MeshGenerator generator) {
            string json = System.IO.File.ReadAllLines(filepath + filename + ".json")[0];
            generator.meshSettings = JsonUtility.FromJson<MeshSettings>(json);
        }

    }
}