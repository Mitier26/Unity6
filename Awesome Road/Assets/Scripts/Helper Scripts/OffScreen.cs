using System;
using UnityEngine;

public class OffScreen : MonoBehaviour
{
    private SpriteRenderer sprite_Renderer;

    private void Awake()
    {
        sprite_Renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        
        if(!GeometryUtility.TestPlanesAABB(planes, sprite_Renderer.bounds))
        {
            if (transform.position.x - Camera.main.transform.position.x < 0.0f)
            {
                CheckTile();
            }
        }
        
    }

    void CheckTile()
    {
        if (this.tag == MyTags.ROAD)
        {
            Change(ref MapGenerator.instance.last_Pos_Of_Road_Tile, new Vector3(1.5f, 0f, 0f),
                ref MapGenerator.instance.last_Order_Of_Road);
        } else  if (this.tag == MyTags.TOP_NEAR_GRASS)
        {
            Change(ref MapGenerator.instance.last_Pos_Of_Top_Near_Grass, new Vector3(1.2f, 0f, 0f),
                ref MapGenerator.instance.last_Order_Of_Top_Near_Grass);
        } else  if (this.tag == MyTags.TOP_FAR_GRASS)
        {
            Change(ref MapGenerator.instance.last_Pos_Of_Top_Far_Grass, new Vector3(4.8f, 0f, 0f),
                ref MapGenerator.instance.last_Order_Of_Top_Far_Grass);
        } else  if (this.tag == MyTags.BOTTOM_NEAR_GRASS)
        {
            Change(ref MapGenerator.instance.last_Pos_Of_Bottom_Near_Grass, new Vector3(1.2f, 0f, 0f),
                ref MapGenerator.instance.last_Order_Of_Bottom_Near_Grass);
        } else  if (this.tag == MyTags.BOTTOM_FAR_LAND_1)
        {
            Change(ref MapGenerator.instance.last_Pos_Of_Bottom_Far_Land_F1, new Vector3(1.6f, 0f, 0f),
                ref MapGenerator.instance.last_Order_Of_Bottom_Far_Land_F1);
        } else  if (this.tag == MyTags.BOTTOM_FAR_LAND_2)
        {
            Change(ref MapGenerator.instance.last_Pos_Of_Bottom_Far_Land_F2, new Vector3(1.6f, 0f, 0f),
                ref MapGenerator.instance.last_Order_Of_Bottom_Far_Land_F2);
        } else  if (this.tag == MyTags.BOTTOM_FAR_LAND_3)
        {
            Change(ref MapGenerator.instance.last_Pos_Of_Bottom_Far_Land_F3, new Vector3(1.6f, 0f, 0f),
                ref MapGenerator.instance.last_Order_Of_Bottom_Far_Land_F3);
        } else  if (this.tag == MyTags.BOTTOM_FAR_LAND_4)
        {
            Change(ref MapGenerator.instance.last_Pos_Of_Bottom_Far_Land_F4, new Vector3(1.6f, 0f, 0f),
                ref MapGenerator.instance.last_Order_Of_Bottom_Far_Land_F4);
        } else  if (this.tag == MyTags.BOTTOM_FAR_LAND_5)
        {
            Change(ref MapGenerator.instance.last_Pos_Of_Bottom_Far_Land_F5, new Vector3(1.6f, 0f, 0f),
                ref MapGenerator.instance.last_Order_Of_Bottom_Far_Land_F5);
        }
    }

    void Change(ref Vector3 pos, Vector3 offset, ref int orderLayer)
    {
        transform.position = pos;
        pos += offset;
        
        sprite_Renderer.sortingOrder = orderLayer;

        orderLayer++;

    }
}
