using System;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] 
    private int cellCount;  //n count in nxn grid

    [SerializeField]
    private Transform gridPrefab; //one grid object prefab

    [SerializeField] 
    private List<GridPrefab> gridPrefabList; //all created grids
    
    private void Start()
    {
        CreateGrid(); // create grid at start
    }

    private void CreateGrid()
    {
        for (int i = 0; i < cellCount; i++) //rows
        {
            for (int j = 0; j < cellCount; j++) //columns
            {
                GridPrefab tempGridPrefab = new GridPrefab(); //temporary grid object for set transforms

                var tempPrefab = Instantiate(gridPrefab); //create each cell(grid objects)

                tempGridPrefab.x = i;  // assign each objects index value
                tempGridPrefab.y = j;
                
                gridPrefabList.Add(tempGridPrefab); //add to grid prefabs list
            }
        }   
    }

    [Serializable]
    public class GridPrefab  //grid objects class
    {
        public int x; //index values
        public int y;
    }
}