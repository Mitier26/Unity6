using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator instance;

    public GameObject 
        roadPrefab, 
        grassPrefab, 
        groundPrefab_1, 
        groundPrefab_2, 
        groundPrefab_3, 
        groundPrefab_4, 
        grass_Bottom_Prefab,
        land_Prefab_1, 
        land_Prefab_2, 
        land_Prefab_3, 
        land_Prefab_4, 
        land_Prefab_5, 
        big_Grass_Prefab, 
        big_Grass_Bottom_Prefab, 
        treePrefab_1, 
        treePrefab_2,
        treePrefab_3, 
        big_Tree_Prefab;

    public GameObject
        road_Holder,
        top_Near_Side_Walk_Holder,
        top_Far_Side_Walk_Holder,
        bottom_Near_Side_Walk_Holder,
        bottom_Far_Side_Walk_Holder;

    public int 
        start_Road_Tile,
        start_Grass_Tile,
        start_Ground3_Tile,
        start_Laden_Tile;

    public List<GameObject>
        road_Tiles,
        top_Near_Grass_Tiles,
        top_Far_Grass_Tiles,
        bottom_Near_Grass_Tiles,
        bottom_Far_Land_F1_Tiles,
        bottom_Far_Land_F2_Tiles,
        bottom_Far_Land_F3_Tiles,
        bottom_Far_Land_F4_Tiles,
        bottom_Far_Land_F5_Tiles;

    // ground1의 위치는 0부터 startGround3Tile까지의 위치를 가짐
    public int[] pos_For_Top_Ground_1;

    public int[] pos_For_Top_Ground_2;
    public int[] pos_For_Top_Ground_4;
    public int[] pos_For_Top_Big_Grass;

    // position form tree1 on top near greaa from 0 to startGroundTile
    public int[] pos_For_Top_Tree_1;
    public int[] pos_For_Top_Tree_2;
    public int[] pos_For_Top_Tree_3;

    public int pos_For_Road_Tile_1;
    public int pos_For_Road_Tile_2;
    public int pos_For_Road_Tile_3;
    public int[] pos_For_Bottom_Big_Grass;
    public int[] pos_For_Bottom_Tree_1;
    public int[] pos_For_Bottom_Tree_2;
    public int[] pos_For_Bottom_Tree_3;

    [HideInInspector]
    public Vector3 
        last_Pos_Of_Road_Tile,
        last_Pos_Of_Top_Near_Grass,
        last_Pos_Of_Top_Far_Grass,
        last_Pos_Of_Bottom_Near_Grass,
        last_Pos_Of_Bottom_Far_Land_F1,
        last_Pos_Of_Bottom_Far_Land_F2,
        last_Pos_Of_Bottom_Far_Land_F3,
        last_Pos_Of_Bottom_Far_Land_F4,
        last_Pos_Of_Bottom_Far_Land_F5;

    [HideInInspector]
    public int 
        last_Order_Of_Road,
        last_Order_Of_Top_Near_Grass,
        last_Order_Of_Top_Far_Grass,
        last_Order_Of_Bottom_Near_Grass,
        last_Order_Of_Bottom_Far_Land_F1,
        last_Order_Of_Bottom_Far_Land_F2,
        last_Order_Of_Bottom_Far_Land_F3,
        last_Order_Of_Bottom_Far_Land_F4,
        last_Order_Of_Bottom_Far_Land_F5;

    void Awake()
    {
        MakeInstance();
    }
    

    void MakeInstance() 
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(gameObject);
        }
    }
}
