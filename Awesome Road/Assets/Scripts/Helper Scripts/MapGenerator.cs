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

    void Start()
    {
        Initialize();
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

    void Initialize() {
        InitializePlatform(roadPrefab, ref last_Pos_Of_Road_Tile, roadPrefab.transform.position,
        start_Road_Tile, road_Holder, ref road_Tiles, ref last_Order_Of_Road, new Vector3(1.5f, 0f, 0f));
    }   // Initialize

    void InitializePlatform(GameObject prefab, ref Vector3 last_Pos, Vector3 last_Pos_Of_Tile, 
    int amountTile, GameObject holder, ref List<GameObject> list_tile, ref int last_Order, Vector3 offset) 
    {
        int orderInLayer = 0;
        last_Pos = last_Pos_Of_Tile;

        for (int i = 0; i < amountTile; i++) {

            GameObject clone = Instantiate(prefab, last_Pos, prefab.transform.rotation) as GameObject;
            clone.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer;

            if(clone.tag == MyTags.TOP_NEAR_GRESS) {

            }else if (clone.tag == MyTags.BOTTOM_NEAR_GRASS) {

            }else if (clone.tag == MyTags.BOTTOM_FAR_LAND_2) {

            }else if (clone.tag == MyTags.TOP_NEAR_GRESS) {

            }

            clone.transform.SetParent(holder.transform);
            list_tile.Add(clone);

            orderInLayer += 1;
            last_Order = orderInLayer;

            last_Pos += offset;

        } // For loop

    }   // InitializePlatform
}
