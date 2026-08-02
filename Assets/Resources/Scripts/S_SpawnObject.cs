using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_SpawnObject : MonoBehaviour
{
    public GameObject prefab;
    public float radius = 0.5f;
    public float limitX = 0.15f;

    public float time;
    public int totalObjSpawn = 10;
    public List<S_Enzima> enzimas;
    // Start is called before the first frame update  
    void Start()
    {
        //SpawnObject();
    }

    IEnumerator SpawnTime()
    {
        yield return new WaitForSeconds(time);
        SpawnObject();
    }

    public void SpawnObject()
    {
        for (int i = 0; i < totalObjSpawn; i++)
        {
            Vector3 randomPos = transform.position + Random.insideUnitSphere * radius;
            randomPos.x = transform.position.x + Random.Range(-limitX, limitX);
            GameObject obj = Instantiate(prefab, randomPos + Vector3.up * 10, Quaternion.identity);
            obj.GetComponent<S_Enzima>().finalPosY = randomPos.y;
            enzimas.Add(obj.GetComponent<S_Enzima>());
        }
    }

    public void GravedadEnzimas()
    {
        foreach (S_Enzima enzima in enzimas)
        {
            if (!enzima.GetComponent<Rigidbody>().useGravity)
            {
                enzima.GetComponent<Rigidbody>().useGravity = true;
            }
        }
    }

    public void OffEnzimas()
    {
        foreach (S_Enzima enzima in enzimas)
        {
            enzima.gameObject.SetActive(false);
        }
    }
}
