using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class airPatrol : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    public float speed = 2f;
    public float waitTime = 3f;
    public bool canGo = true;
    public bool moveLeft = true;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3(point1.position.x, point1.position.y, point1.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        if (canGo)
            transform.position = Vector3.MoveTowards(transform.position, point1.position, speed * Time.deltaTime);
        if (transform.position == point1.position)
        {
            Transform t = point1;
            point1 = point2;
            point2 = t;
            canGo = false;

            StartCoroutine(Waiting());
            if (moveLeft == true)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                moveLeft = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                moveLeft = true;

            }
        }
        IEnumerator Waiting()
        {
            yield return new WaitForSeconds(waitTime);
            canGo = true;
        }
    }
}
