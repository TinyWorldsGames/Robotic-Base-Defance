using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    private Vector3 shootDir;

    float damage = 9;

    [SerializeField]
    private float moveSpeed;

    public void Setup(Vector3 shootDir,Transform enemy)
    {
        this.shootDir = shootDir;
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(shootDir));
        transform.LookAt(enemy);
        Destroy(gameObject, 2f);
    }

    float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
       
        if (n < 0)
        {
            n += 360;
        }
        return n;
    }




    private void Update()
    {
        transform.position += shootDir * moveSpeed * Time.deltaTime;
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyAIManager>(out EnemyAIManager enemy))
        {
            enemy.health -= damage;
            enemy.healthBarBG.gameObject.SetActive(true);

            Destroy(gameObject);


        }

        if (other.TryGetComponent<AIManager>(out AIManager soldier))
        {
            soldier.health -= damage;

            Destroy(gameObject);


        }





    }

}