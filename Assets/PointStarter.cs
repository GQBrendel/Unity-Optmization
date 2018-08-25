﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;


struct Line
{
    public Vector3 first;
    public Vector3 second;
}
public class PointStarter : MonoBehaviour {

    Pair<float, float>[] a = { new Pair<float, float>(-13,  0.5f), new Pair<float, float>(-10, -11.5f), new Pair<float, float>(-10 , 9),
    new Pair<float, float>(-4.5f,  -2), new Pair<float, float>(-1,  8.5f), new Pair<float, float>(0.5f,  6), new Pair<float, float>(0.5f,  -12),
    new Pair<float, float>(2,  12.5f), new Pair<float, float>(3.5f,  11), new Pair<float, float>(6.5f,  3.2f), new Pair<float, float>(7,  -10),
    new Pair<float, float>(9,  -5), new Pair<float, float>(11.5f,  -4)};

    public GameObject pointPrefab;
    GameObject go;
    public List<GameObject> points = new List<GameObject>();
    public List<Transform> hullPoints = new List<Transform>();

    void Start () {        

        foreach (Pair<float, float> pointPair in a)
        {
            Vector3 pos = new Vector3(pointPair.First, 0, pointPair.Second);
                go = Instantiate(pointPrefab, pos, Quaternion.identity, this.transform) as GameObject;
                points.Add(go);
        }
        int n = 13;
        printHull(a, n);

    }
    
// Stores the result (points of convex hull)
Stack<Pair<float, float>> hullStack = new Stack<Pair<float, float>>();    

    // Returns the side of point p with respect to line
    // joining points p1 and p2.
    int findSide(Pair<float, float> l1, Pair<float, float> l2, Pair<float, float> l)
    {       
        float val = (l.Second - l1.Second) * (l2.First - l1.First) -
                  (l2.Second - l1.Second) * (l.First - l1.First);

       // Debug.Log("L1: (" + l1.First + ", " + l1.Second + ") ");
      //  Debug.Log("L2: (" + l2.First + ", " + l2.Second + ") ");
      //  Debug.Log("L: (" + l.First + ", " + l.Second + ") ");
       // Debug.Log("Val: " + val);

        if (val > 0)
            return 1;
        if (val < 0)
            return -1;
        
        return 0;
    }
    // Returns the square of distance between
    // p1 and p2.
    float dist(Pair<float, float> p, Pair<float, float> q)
    {
        return (p.Second - q.Second) * (p.Second - q.Second) +
            (p.First - q.First) * (p.First - q.First);
    }

    // returns a value proportional to the distance
    // between the point p and the line joining the
    // points p1 and p2
    float lineDist(Pair<float, float> p1, Pair<float, float> p2, Pair<float, float> p)
    {
        return Mathf.Abs((p.Second - p1.Second) * (p2.First - p1.First) -
                (p2.Second - p1.Second) * (p.First - p1.First));
    }

    // End points of line L are p1 and p2. side can have value
    // 1 or -1 specifying each of the parts made by the line L
    void quickHull(Pair<float, float>[] a, int n, Pair<float, float> p1, Pair<float, float> p2, int side)
    {
        int ind = -1;
        float max_dist = 0;

        // finding the point with maximum distance
        // from L and also on the specified side of L.
        for (int i = 0; i < n; i++)
        {
            float temp = lineDist(p1, p2, a[i]);
            if (findSide(p1, p2, a[i]) == side && temp > max_dist)
            {
                ind = i;
                max_dist = temp;
            }
        }

        // If no point is found, add the end points
        // of L to the convex hull.
        if (ind == -1)
        {
            hullStack.Push(p1);
            hullStack.Push(p2);
            
            return;
        }
 
        // Recur for the two parts divided by a[ind]
        quickHull(a, n, a[ind], p1, -findSide(a[ind], p1, p2));
        quickHull(a, n, a[ind], p2, -findSide(a[ind], p2, p1));
    }
    void printHull(Pair<float, float>[] a, int n)
    {
        // a[i].second -> y-coordinate of the ith point
        if (n < 3)
        {
            Debug.Log("Convex hull not possible");
            return;
        }

        // Finding the point with minimum and
        // maximum x-coordinate
        int min_x = 0, max_x = 0;
        for (int i = 1; i < n; i++)
        {
            if (a[i].First < a[min_x].First)
                min_x = i;
            if (a[i].First > a[max_x].First)
                max_x = i;
        }

        // Recursively find convex hull points on
        // one side of line joining a[min_x] and
        // a[max_x]
        quickHull(a, n, a[min_x], a[max_x], 1);

        // Recursively find convex hull points on
        // other side of line joining a[min_x] and
        // a[max_x]
        quickHull(a, n, a[min_x], a[max_x], -1);


       foreach (GameObject point in points)
       {
            bool belong = false;
            foreach (Pair<float, float> hull in hullStack)
            {
                if (point.transform.position.x == hull.First && point.transform.position.z == hull.Second)
                {
                    belong = true;
                }
            }
            if (belong)
            {
                hullPoints.Add(point.transform);
            }
       }

        hullPoints = hullPoints.OrderBy(go => go.position.x).ToList();


        Debug.Log("The points in Convex Hull are:\n");
        for (int i = 0; i < hullPoints.Count; i++)
        {
            if(i == hullPoints.Count - 1)
            {
                drawLaser(hullPoints[i], hullPoints[0]);
            }
            else
            {
                drawLaser(hullPoints[i], hullPoints[i + 1]);
            }

            Debug.Log("(" + hullPoints[i].position.x + ", " + hullPoints[i].position.z + ") ");
        }
        /*
        Debug.Log("The points in Convex Hull are:\n");
        while (hullStack.Count != 0)
        {
            Debug.Log("(" + hullStack.Peek().First + ", " + hullStack.Peek().Second + ") ");
            hullStack.Pop();
        }*/

    }
    void drawLaser(Transform start, Transform end)
    {
        LineRenderer laser;
        laser = start.GetComponent<LineRenderer>();

        Vector3 startPos, endPos;
        startPos = start.position;
        endPos = end.position;

        laser.SetPosition(0, startPos);
        laser.SetPosition(1, endPos);
    }

}

public class Pair<T, U>
{
    public Pair()
    {
    }

    public Pair(T first, U second)
    {
        this.First = first;
        this.Second = second;
    }

    public T First { get; set; }
    public U Second { get; set; }
};
