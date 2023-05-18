using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AdvancedAirPatrol : MonoBehaviour
{
    public Transform[] points;
    public float speed = 2f;
    public float waitTime = 3f;
    bool canGo = true;
    public int i = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    void Update()
    {
        if (canGo)
            transform.position = Vector3.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
        if (transform.position == points[i].position)
        {
            
            if (i < points.Length - 1)
                i++;
            else
                i = 0;
            /*if (points[i].position.x > points[i - 1].position.x)
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            else
                gameObject.GetComponent<SpriteRenderer>().flipX = false;*/
            canGo = false;
            

            StartCoroutine(Waiting());
           
        }
        IEnumerator Waiting()
        {
            yield return new WaitForSeconds(waitTime);
            canGo = true;
            if (i != 0)
            {
                /*points[i - 1] = points[points.Length - 1];*/
                if (points[i].position.x > points[i - 1].position.x)
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                else
                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
                if (points[i].position.x > points[points.Length - 1].position.x)
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            else
                gameObject.GetComponent<SpriteRenderer>().flipX = false;

        }
    }
}
