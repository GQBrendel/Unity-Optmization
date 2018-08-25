using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Lina
{
    Vector3 p1;
    Vector3 p2;
}
public class PointStarter : MonoBehaviour {


    Vector3[] positions = { new Vector3(-13, 0, 0.5f), new Vector3(-10,0,-11.5f), new Vector3(-10, 0, 9),
        new Vector3(-4.5f, 0, -2), new Vector3(-1, 0, 8.5f), new Vector3(0.5f, 0, 6), new Vector3(0.5f, 0, -12),
        new Vector3(2, 0, 12.5f), new Vector3(3.5f, 0, 11), new Vector3(6.5f, 0, 3.2f), new Vector3(7, 0, -10),
        new Vector3(9, 0, -5), new Vector3(11.5f, 0, -4)}; 


    public GameObject pointPrefab;
    GameObject go;

    void Start () {
        
        foreach(Vector3 pos in positions)
        {

            Instantiate(pointPrefab, pos, Quaternion.identity, this.transform);// as GameObject;
        }

    }
}
