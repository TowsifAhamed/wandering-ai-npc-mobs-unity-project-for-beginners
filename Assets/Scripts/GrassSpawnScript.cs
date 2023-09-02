using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawnScript : MonoBehaviour
{
    public Transform grassfab;
    void Start()
    {
        var grass = Instantiate(grassfab, transform.position, Quaternion.identity);
        grass.tag = "grass";
        StartCoroutine("Reset");
    }
    IEnumerator Reset()
    {
        yield return new WaitForSeconds(40);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("grass");
        foreach (GameObject enemy in enemies)
            GameObject.Destroy(enemy);

        yield return new WaitForSeconds(5);

        var grass = Instantiate(grassfab, transform.position, Quaternion.identity);
        grass.tag = "grass";

        StartCoroutine("Reset");
    }
}
