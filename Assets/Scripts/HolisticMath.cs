using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolisticMath
{
    static public float Square(float value)
    {
        return value * value;
    }


    static public float Distance(Coords point1, Coords point2)
    {
        float x = point2.x - point1.x;
        float y = point2.y - point1.y;
        float z = point2.z - point1.z;

        return Mathf.Sqrt(Square(x) + Square(y) + Square(z));
    }

    static public Coords GetNormal(Coords vector)
    {
        float distance = Distance(new Coords(0, 0, 0), vector);
        vector.x /= distance;
        vector.y /= distance;
        vector.z /= distance;

        return vector;
    }

    static public float Dot(Coords vector1, Coords vector2)
    {
        // Dot(vector1, vector2) > 0       angle < 90
        // Dot(vector1, vector2) = 0       angle = 90
        // Dot(vector1, vector2) < 0       angle > 90

        return vector1.x * vector2.x + vector1.y * vector2.y + vector1.z * vector1.z;
    }

    static public float Angle(Coords vector1, Coords vector2)
    {
        // Independent to order 
        float denominator = Dot(vector1, vector2);
        // Distance is always positive because of pitagoras
        // Also independent to order
        float numerator = Distance(new Coords(0, 0, 0), vector1) * Distance(new Coords(0, 0, 0), vector2);

        return Mathf.Acos(denominator / numerator); // radians. For degree * 180/Mathf.PI;
    }

    static public Coords Rotate(Coords vector, float angle, bool clockwise) // in radians (because it will come from angle method)
    {
        if (clockwise)
        {
            angle = 2 * Mathf.PI - angle;
        }

        float xVal = vector.x * Mathf.Cos(angle) - vector.y * Mathf.Sin(angle);
        float yVal = vector.x * Mathf.Sin(angle) + vector.y * Mathf.Cos(angle);
        return new Coords(xVal, yVal, 0);
    }

    // the first vector is the direction you are currently facing and the second vector is the direction 
    // you want to face
    static public Coords Cross(Coords vector1, Coords vector2)
    {
        float coordX = vector1.y * vector2.z - vector1.z * vector2.y;
        float coordY = vector1.z * vector2.x - vector1.x * vector2.z;
        float coordZ = vector1.x * vector2.y - vector1.y * vector2.x;

        return new Coords(coordX, coordY, coordZ);
    }

    static public Vector3 LookAt2D(GameObject currentGameObject, GameObject targetGameObject)
    {
        Vector3 forwardVector = currentGameObject.transform.up;

        Vector3 direction = targetGameObject.transform.position - currentGameObject.transform.position;
        Coords dirNormal = GetNormal(new Coords(direction));
        direction = dirNormal.ToVector();
        float angle = Angle(new Coords(forwardVector), new Coords(direction));

        bool clockwise = false;

        if (Cross(new Coords(forwardVector), dirNormal).z < 0)
        {
            // If this cross product between transform.up (forwardVector) and direction to goal is positive, by right hand rule 
            // (going from first parameter to second parameter),
            // that indicates anti-clockwise rotation.
            // If this cross product between transform.up (forwardVector) and direction to goal is negative, by right hand rule 
            // (going from first parameter to second parameter),
            // that indicates clockwise rotation.

            // green axis: transform.up
            // red axis: transform.right
            // blue axis: transform.forward

            clockwise = true;
        }

        Coords newDir = Rotate(new Coords(forwardVector), angle, clockwise);
        currentGameObject.transform.up = new Vector3(newDir.x, newDir.y, newDir.z);

        return direction;
    }
}
