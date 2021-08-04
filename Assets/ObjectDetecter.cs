using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetecter : MonoBehaviour
{
    Queue sQ = new Queue();
    int round = 1;

    // Start is called once before the first frame
    private void Start()
    {
        startRound();
    }


    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collider = Physics2D.OverlapPoint(worldPoint);


            if (collider != null)
            {
                Debug.Log(collider.transform.gameObject.name);
                collider.transform.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            }

        }
    }

    private void startRound()
    {
        for (int i = 0; i < round; i++)
        {
            //random square light green and add to the queue
        }
    }
}