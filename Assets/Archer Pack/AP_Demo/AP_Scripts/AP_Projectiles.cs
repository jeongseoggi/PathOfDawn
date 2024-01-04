using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AP_Projectiles : MonoBehaviour
{
    public GameObject ExplosionPrefab;
    public float DestroyExplosion = 4.0f;
    public float DestroyChildren = 2.0f;
    public Vector3 Velocity;
    public ARROW_TYPE type;

    public enum ARROW_TYPE
    {
        NORMAL,
        SLERP
    }


    Rigidbody rb;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        if(type == ARROW_TYPE.NORMAL)
        {
            rb.velocity = new Vector3(0, 0, -15);
        }
        //transform.position = Vector3.Slerp(transform.position, new Vector3(1.240f, 5, -4.66f), Time.deltaTime);
    }

    private void Update()
    {
        if(type == ARROW_TYPE.SLERP)
        {
            transform.position = Vector3.Slerp(transform.position, new Vector3(1.240f, 5, -4.66f), Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (ExplosionPrefab)
        {
            var exp = Instantiate(ExplosionPrefab, col.contacts[0].point, ExplosionPrefab.transform.rotation);
            Destroy(exp, DestroyExplosion);
            SendMessageUpwards("GetExplosion", exp);
        }
        Transform child;
        child = transform.GetChild(0);
        transform.DetachChildren();
        Destroy(child.gameObject, DestroyChildren);
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
