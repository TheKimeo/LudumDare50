using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildingPrefabFactory : MonoBehaviour
{

    [System.Serializable]
    public class Prefabs
    {
        public GameObject m_ghost;
        public GameObject m_real;
    }
    public Prefabs[] m_buildingPrefabs = new Prefabs[(int)BuildingTypes.COUNT];

    //---------------------------
    void OnValidate()
    {
        if (m_buildingPrefabs.Length != (int)BuildingTypes.COUNT)
        {
            Debug.LogWarning("Don't change the 'm_buildingPrefabs' field's array size!");
            Array.Resize(ref m_buildingPrefabs, (int)BuildingTypes.COUNT);
        }

    }

    //---------------------------
   public GameObject CreateGhost(BuildingTypes i_type, Vector3 i_spawnPos)
    {
        if ((int)i_type < (int)BuildingTypes.COUNT)
        {
            return Instantiate(m_buildingPrefabs[(int)i_type].m_ghost, i_spawnPos, Quaternion.identity);
        }
        return new GameObject();
    }

    //---------------------------
    public GameObject CreateReal(BuildingTypes i_type, Vector3 i_spawnPos)
    {
        if ((int)i_type < (int)BuildingTypes.COUNT)
        {
            return Instantiate(m_buildingPrefabs[(int)i_type].m_real, i_spawnPos, Quaternion.identity);
        }
        return new GameObject();
    }



}
