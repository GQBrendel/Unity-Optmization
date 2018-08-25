using System.Collections;
using System.Collections.Generic;
using UnityEngine;


struct Line
{
    public Vector3 first;
    public Vector3 second;
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
      //      Instantiate(pointPrefab, pos, Quaternion.identity, this.transform);// as GameObject;
        }

        Pair<int, int>[] a = { new Pair<int, int>(0,3), new Pair<int, int>(1,1),
            new Pair<int, int>(2,2), new Pair<int, int>(4,4), new Pair<int, int>(0,0), new Pair<int, int>(1,2),
            new Pair<int, int>(3,1), new Pair<int, int>(3,3) };

        /*Pair<int, int>[] _positions = { new Pair<int, int>(-13, 0.5f), new Pair<int, int>(-10,0,-11.5f), new Pair<int, int>(-10, 0, 9),
        new Pair<int, int>(-4.5f, 0, -2), new Pair<int, int>(-1, 0, 8.5f), new Pair<int, int>(0.5f, 0, 6), new Pair<int, int>(0.5f, 0, -12),
        new Pair<int, int>(2, 0, 12.5f), new Pair<int, int>(3.5f, 0, 11), new Pair<int, int>(6.5f, 0, 3.2f), new Pair<int, int>(7, 0, -10),
        new Pair<int, int>(9, 0, -5), new Pair<int, int>(11.5f, 0, -4)};*/

        foreach (Pair<int,int> pointPair in a)
        {
            Vector3 pos = new Vector3(pointPair.First, 0, pointPair.Second);
            Instantiate(pointPrefab, pos, Quaternion.identity, this.transform);// as GameObject;
        }





        int n = 8;
        printHull(a, n);

    }
    // Stores the result (points of convex hull)
    Stack<Pair<int, int>> hullStack = new Stack<Pair<int, int>>();
    
    

    // Returns the side of point p with respect to line
    // joining points p1 and p2.
    int findSide(Pair<int, int> l1, Pair<int, int> l2, Pair<int, int> l)
    {

       
        int val = (l.Second - l1.Second) * (l2.First - l1.First) -
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
    int dist(Pair<int, int> p, Pair<int, int> q)
    {
        return (p.Second - q.Second) * (p.Second - q.Second) +
            (p.First - q.First) * (p.First - q.First);
    }

    // returns a value proportional to the distance
    // between the point p and the line joining the
    // points p1 and p2
    int lineDist(Pair<int, int> p1, Pair<int, int> p2, Pair<int, int> p)
    {
        return Mathf.Abs((p.Second - p1.Second) * (p2.First - p1.First) -
                (p2.Second - p1.Second) * (p.First - p1.First));
    }

    // End points of line L are p1 and p2. side can have value
    // 1 or -1 specifying each of the parts made by the line L
    void quickHull(Pair<int, int>[] a, int n, Pair<int, int> p1, Pair<int, int> p2, int side)
    {
        int ind = -1;
        int max_dist = 0;

        // finding the point with maximum distance
        // from L and also on the specified side of L.
        for (int i = 0; i < n; i++)
        {
            int temp = lineDist(p1, p2, a[i]);
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
    void printHull(Pair<int, int>[] a, int n)
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

        Debug.Log("The points in Convex Hull are:\n");

        int j = 0;
        while (hullStack.Count != 0)
        {

            Debug.Log("(" + hullStack.Peek().First + ", " + hullStack.Peek().Second + ") ");
            hullStack.Pop();
            //hullList.Remove(hullList[0]);
            j++;

            //  Debug.Log("(" + hull. + ", ");

            // cout << "(" << (*hull.begin()).first << ", "
            //     << (*hull.begin()).second << ") ";
            // hull.erase(hull.begin());
        }
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
