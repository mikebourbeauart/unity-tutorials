using UnityEngine;
using System.Collections;

public class DeathStar : MonoBehaviour 
{
    public float height; //Something like 50
    public float distance; //Something like 80
    public float rotationSpeed; //Something like 5

	void Start() 
	{
        transform.position = new Vector3(transform.position.x, height, distance);
	}
	
	void Update() 
	{
        transform.position = new Vector3(transform.position.x, height, distance);
        transform.RotateAround(Vector3.zero, Vector3.up, rotationSpeed * Time.deltaTime);
    }
}