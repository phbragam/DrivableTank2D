using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject objPrefab;
    public Vector3 objPosition;

    private void Awake()
    {
        GameObject obj = Instantiate(objPrefab, new Vector3(Random.Range(-100, 100),
        Random.Range(-100, 100), objPrefab.transform.position.z),
        Quaternion.identity);

        objPosition = obj.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("Fuel location: " + obj.transform.position);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
