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

    private Camera mainCamera;

    private Vector3 cellSize;
    private void Start()
    {
        mainCamera = Camera.main;
        
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

                //TODO scale and transform should check and add comments
                tempPrefab.transform.localScale = new Vector3( CalculateScreenSize(cellCount),CalculateScreenSize(cellCount),1);

                cellSize = mainCamera.ScreenToWorldPoint(new Vector3( ((i + .5f) * mainCamera.pixelWidth/cellCount),  (j + .5f ) * mainCamera.pixelHeight/cellCount, mainCamera.nearClipPlane));
                
                tempPrefab.transform.position = new Vector3(cellSize.x, cellSize.y, mainCamera.nearClipPlane + 1f);

                tempGridPrefab.cellTransform = tempPrefab;
                
                tempGridPrefab.x = i;  // assign each objects index value
                tempGridPrefab.y = j;
                
                gridPrefabList.Add(tempGridPrefab); //add to grid prefabs list
            }
        }   
    }

    private float CalculateScreenSize(int count) //divide screen to given cell count
    {
        if (Screen.width > Screen.height) 
        {
            return mainCamera.pixelHeight / (count * 1f); //*1f for float fraction loss
        }
        else
        {
            return mainCamera.pixelWidth / (count * 1f); //return each cell size to set transforms
        }
    }

    [Serializable]
    public class GridPrefab  //grid objects class
    {
        public int x; //index values
        public int y;
        
        public Transform cellTransform;
       
    }
}