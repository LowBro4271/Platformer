using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    public Transform door;
    public Sprite mid, top;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void unlock()
    {
        isOpen = true;
        GetComponent<SpriteRenderer>().sprite = mid;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = top;
    }
    public void teleport(GameObject Player)
    {
        Player.transform.position = door.transform.position;

    }
}
