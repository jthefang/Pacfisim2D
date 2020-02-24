using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
    To have player collide with other game objects:
        - Player should have components: 
            - Collider2D (e.g. Box)
            - RigidBody2D (freeze Z rotation, set gravity scale to 0, set interpolate to interpolate if not smooth movement)
        - Other object should have components: Collider2D
*/
public class PlayerController2D : MonoBehaviour, IDependentScript {

    [SerializeField]
    float moveSpeed = 2.0f;
    [SerializeField]
    Propulsion propulsion;

    Rigidbody2D rigidbody2D;
    Vector3 moveDirection;
    Player player;
    AudioSource audioSource;
    bool propulsionLoaded;
    //CharacterAnimation characterAnim;

    void Start() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        player = GetComponent<Player>();
        
        List<ILoadableScript> dependencies = new List<ILoadableScript>();
        dependencies.Add(propulsion);
        ScriptDependencyManager.Instance.UpdateDependencyDicts(this, dependencies);
        propulsionLoaded = propulsion.IsInitialized();
        //characterAnim = GetComponent<CharacterAnimation>();
    }

    void FixedUpdate() {
        if (!propulsionLoaded) {
            return;
        }

        if (player.gameManager.IsPlaying) {
            LookTowardMouse();

            float speed = 0f;
            if (Input.GetKey(KeyCode.Space)) {
                speed = moveSpeed;
            } 
            Move(speed);
        } else {
            Stop();
        }
    }

    public void OnAllDependenciesLoaded() {
        propulsionLoaded = true;
    }

    void Move(float speed) {
        //let physics system decide movement (=> no jittery movement, e.g. when moving into walls)
        rigidbody2D.velocity = -moveDirection * speed;
        if (speed > 0) {
            propulsion.Play();
            //characterAnim.PlayMoveAnim(moveDirection);
        } else {
            propulsion.Stop();
        }
    }

    public void Stop() {
        Move(0f);
    }

    void LookTowardMouse() {
        //Get the screen positions of the player and mouse
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        moveDirection = (positionOnScreen - mouseOnScreen).normalized; //=> doesn't move too fast along diagonals
        transform.right = moveDirection; //sets player face direction
    }

}