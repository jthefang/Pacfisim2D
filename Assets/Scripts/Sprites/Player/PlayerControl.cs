using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{   
    Player player;
    public float speed = 2.0f;

    private Rigidbody2D rigidbody2d;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player.gameManager.IsPlaying) {
            LookTowardMouse();

            if (Input.GetKey(KeyCode.Space)) {
                transform.position -= transform.right * Time.deltaTime * speed;
            }
        }
    }
         
    void LookTowardMouse() {
        //Get the Screen positions of the object
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint (transform.position);
        //Get the Screen position of the mouse
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        transform.right = (positionOnScreen - mouseOnScreen);
    }
}
