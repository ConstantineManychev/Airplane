using UnityEngine;
using System.Collections;

public class laserScript : MonoBehaviour {

    // Как долго существует лазер
    public float lifetime = 1.0f;

    // Как быстро движется лазер
    public float speed = 1.0f;

    // Как много наносит урона лазер при соприкосновении с врагами
    public int damage = 1;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, lifetime);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
	}
}
