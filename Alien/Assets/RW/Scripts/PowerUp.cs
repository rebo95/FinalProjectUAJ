using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float speed;
    private float maxY = -5;

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        transform.Translate(Vector3.down * Time.deltaTime * speed);
        if (transform.position.y < maxY)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "ShipModel")
        {
            Destroy(gameObject);
        }
    }

    //Método que nos permite fijar la velocidad del powerUp que se estará
    //Spawneando
    public void SetSpeed(float s)
    {
        speed = s;
    }
}
