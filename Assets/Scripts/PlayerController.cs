using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text countText;
    public Text winText;
    private Rigidbody rb;
    private int count;


    void Start() {
        rb = GetComponent<Rigidbody>();
        count =0;
        SetCountText();
        winText.text = "";
    }

    void Update()
    {
        if (gameObject!=null){

            gameObject.GetComponent(typeof(CharacterController));
            {
    
                CharacterController cc = (CharacterController)gameObject.GetComponent( typeof(CharacterController));
    
                float rotateSpeed = 10.0f;
                float rotationY = Input.GetAxis ("Mouse X") * rotateSpeed;
                transform.Rotate (0 , rotationY , 0);
            
                float moveSpeed = 40.0f;
                float dt = Time.deltaTime;
                float dy =  0;
                if(Input.GetKey(KeyCode.Space))
                {
                    dy = moveSpeed * dt;
                }
                if(Input.GetKey(KeyCode.P))
                {
                    dy -= moveSpeed * dt;
                }
                float dx = Input.GetAxis("Horizontal") * dt * moveSpeed;
                float dz= Input.GetAxis("Vertical") * dt * moveSpeed;
            
                cc.Move(transform.TransformDirection(new Vector3(dx, dy,dz))  );
            }
        }
 
    }

    void OnTriggerEnter(Collider other) {
        
        if (other.gameObject.CompareTag("Pick Up")) {
            other.gameObject.SetActive(false);
            count += 1;
            SetCountText();
        }
    }

    void SetCountText(){
        countText.text = "Count: " + count.ToString();
        if (count >= 8){
            winText.text = "You Win";
        }
    }

}

