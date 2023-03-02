using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine; 

public class GridGenerator : MonoBehaviour 
{
    [SerializeField] private int cellCount;  //n count in nxn grid
    
    private int tmpCellCount;
    
    [SerializeField] private Transform gridPrefab; //one grid object prefab
    
    [SerializeField] private List<GridPrefab> gridPrefabList = new List<GridPrefab>(); //all created grids
    [SerializeField] private List<XPrefab> XPrefabList = new List<XPrefab>(); //all created x objects

    [SerializeField] private Vector2 clickedArea;
    
    private Transform tempxPrefab;
    
    [Serializable]
    public class GridPrefab //grid objects class
    {
        public int x;  //index values
        public int y;
        public Transform transform;
    } 
    
    [Serializable]
    public class XPrefab //X object class
    {
        public int x;  //index values
        public int y;
        public Transform transform;
    } 

    void Start()
    {
        tmpCellCount = cellCount;
        cam = Camera.main; //set camera
 
        CreateGrid(); // create grid at start
        //TODO for instantiate and destroy, pool system should use
        CreateXObject();  //X prefab reference at start
    }

    private void Update()
    {
        if (cellCount != tmpCellCount)
        {
            tmpCellCount = cellCount;

            ClearGrid(); //remove old grid before creating new one
            CreateGrid();
        }

        if (Input.GetMouseButtonDown(0)) //screen tapped
        {
            Debug.Log("GetMouseButtonDown");

            var gridPrefabObj = gridPrefabList.Find(a => a.x == clickedArea.x && a.y == clickedArea.y);
            if (gridPrefabObj != null)
            {
                tempxPrefab.transform.position = gridPrefabObj.transform.position + Vector3.forward * -.6f;

                XPrefab tempXPrefab = new XPrefab();

                tempXPrefab.x = gridPrefabObj.x;
                tempXPrefab.y = gridPrefabObj.y;
                
                tempXPrefab.transform = tempxPrefab.transform;
                
                XPrefabList.Add(tempXPrefab);
                
                CreateXObject();
                CheckXPattern(); //check in each tap
            }
        }
    }

    private Camera cam; 
    private Vector3 point;

    void OnGUI()
    {
        point = new Vector3();
        Event currentEvent = Event.current;
        Vector2 mousePos = new Vector2(); //current point in tap
 
        mousePos.x = currentEvent.mousePosition.x; //get x position
        mousePos.y = cam.pixelHeight - currentEvent.mousePosition.y; //get y from x

        point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));

        clickedArea.x = (int)(mousePos.x / (cam.pixelWidth / cellCount));
        clickedArea.y = (int)(mousePos.y / (cam.pixelHeight / cellCount));
  
        GUILayout.BeginArea(new Rect(20, 20, 250, 120));
        GUILayout.Label("Screen pixels: " + cam.pixelWidth + ":" + cam.pixelHeight);
        GUILayout.Label("Mouse position: " + mousePos);
        GUILayout.Label("World position: " + point.ToString("F3"));
        GUILayout.EndArea();
    }

    void CreateXObject()
    { 
        tempxPrefab = GameObject.Instantiate(GameManager.Instance.xPrefab);
        
        //TODO scale and responsiveness 
        //tempxPrefab.transform.localScale = new Vector3( CalculateScreenSize(cellCount),CalculateScreenSize(cellCount),1);
    }

    void ClearGrid()
    {
        foreach (var gridPrefab in gridPrefabList)
        {
            Destroy(gridPrefab.transform.gameObject);
        }
        gridPrefabList.Clear();
        
        CreateGrid();
    }
    
    void CreateGrid()
    {
        for (int i = 0; i < cellCount; i++) //rows
        {
            for (int j = 0; j < cellCount; j++) //columns
            {
                GridPrefab tempGridPrefab = new GridPrefab(); //temporary grid object for set transforms
                
                var tmpPrefab = GameObject.Instantiate(gridPrefab);  
               
                //TODO scale of cells and responsiveness
                //tmpPrefab.transform.localScale = new Vector3( CalculateScreenSize(cellCount),CalculateScreenSize(cellCount),1);

                point = cam.ScreenToWorldPoint(new Vector3( ((i + .5f) * cam.pixelWidth/cellCount),  (j + .5f ) * cam.pixelHeight/cellCount, cam.nearClipPlane));
                
                //get world position of cells from screen position, set position according to this

                tmpPrefab.transform.position = new Vector3( point.x ,point.y,cam.nearClipPlane+1f);

                tempGridPrefab.transform = tmpPrefab; //set position of cell from temp prefab
               
                tempGridPrefab.x = i;   //assign each objects index value
                tempGridPrefab.y = j;
                
                gridPrefabList.Add(tempGridPrefab);  //add to grid prefabs list
            }
        } 
    }
 
    float CalculateScreenSize(int cellCount) //divide screen to given cell count
    {
        if (Screen.width > Screen.height)
        {
            return (cam.pixelHeight / (cellCount*1f)); //*1f for float fraction loss
        }
        else
        {
            return (cam.pixelWidth / (cellCount*1f));  //return each cell size to set transforms
        }
    }
    
    void CheckXPattern()
    {
        for (int i = 0; i < cellCount; i++)
        {
            var x = XPrefabList.Where(a => a.x == i);
            var y = XPrefabList.Where(a => a.y == i);
            
            var Listx = x.ToList();
            var Listy = y.ToList();

            if (Listx.Count > 2)
            {
                Debug.Log("ClearX");

                foreach (var gridPrefab in XPrefabList)
                {
                    Destroy(gridPrefab.transform.gameObject);
                }
                XPrefabList.Clear();
            }
            
            if (Listy.Count > 2)
            {
                Debug.Log("ClearY");
                
                foreach (var gridPrefab in XPrefabList)
                {
                    Destroy(gridPrefab.transform.gameObject);
                }
                XPrefabList.Clear();
            }

            //TODO if x's in the different coordinates but 3 of them are collateral
        }
    }
}