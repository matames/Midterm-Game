using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenChange : MonoBehaviour
    
{
    Rigidbody2D myBody;
    public float newcamPos;
    // Start is called before the first frame update
    void Start()
    {
        myBody = gameObject.GetComponent<Rigidbody2D>();
        newcamPos = -8;
       
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 newCamPos = new Vector3(newcamPos, myBody.transform.position.y + 1/2, -10);
        Camera.main.transform.position = newCamPos;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "camTrigger")

        
        {

            if (myBody.velocity.x > 0)
            {
                newcamPos += 6;
            }
            else
            {
                if (newcamPos == -8)
                {
                    return;
                }
                else
                {


                    newcamPos -= 6;
                }
            }

        }
    }
}


