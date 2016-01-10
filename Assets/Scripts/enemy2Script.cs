using UnityEngine;
using System.Collections;

public class enemy2Script : MonoBehaviour {

    // скорость полета и скорость поворота в секунду
    public float speed = 3.0f;
    public float turnSpeed = 90;
    public int health = 2;

    private Transform thisShip;
    private Transform player;

    // Переменная для лазера
    public Transform laser;

    // Как далеко от центра корабля будет появлятся лазер
    public float laserDistance = 5.0f;

    // Задержка между выстрелами (кулдаун)
    public float timeBetweenFires = 3.0f;

    // Счетчик задержки между выстрелами
    private float timeTilNextFire = 3.0f;

    // Переменная для звука выстрела лазером
    public AudioClip shootSound;

    // Анимация при уничтожении объекта
    public Transform explosion;

	// Use this for initialization
	void Start () {

        // Получаем компонент трансформации объекта, к которому привязан данный компонент
        thisShip = transform;
        player = GameObject.Find("playerShip").transform;
	}
	
	// Update is called once per frame
    void Update()
    {
        float randDistance;
        // направление на игрока
        float dx = thisShip.position.x - player.position.x;
        float dy = thisShip.position.y - player.position.y;

        // угол поворота на игрока
        float angle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        // максимальный угол поворота на текущем кадре
        float maxAngle = turnSpeed * Time.deltaTime;

        // Вычисляем прямой поворот на игрока
        Quaternion rot = Quaternion.Euler(new Vector3(0, 0, angle + 90));

        // поворачиваем врага на игрока с учетом скорости поворота
        if (maxAngle < angle)
        {
            thisShip.rotation = rot;
        }
        else
        {
            thisShip.rotation = rot;
        }

        // если дистанция до игрока больше 
        randDistance = Random.Range(3, 20);
        if (Vector3.Distance(player.position, thisShip.position) > randDistance)
        {
            // двигаемся к игроку
            Vector3 delta = player.position - thisShip.position;
            delta.Normalize();
            float moveSpeed = speed * Time.deltaTime;
            transform.position = transform.position + (delta * moveSpeed);
        }
        else // если меньше или равна трем метрам
        {
            if (timeTilNextFire < 0)
            {
                timeTilNextFire = timeBetweenFires;
                ShootLaser();
            }
            timeTilNextFire -= Time.deltaTime;
        }
    }

    // Создание лазера
    void ShootLaser()
    {
        // Высчитываем позицию корабля
        float posX = this.transform.position.x + (Mathf.Cos((transform.localEulerAngles.z - 90) * Mathf.Deg2Rad) * -laserDistance);
        float posY = this.transform.position.y + (Mathf.Sin((transform.localEulerAngles.z - 90) * Mathf.Deg2Rad) * -laserDistance);
        // Создаём лазер на этой позиции
        Instantiate(laser, new Vector3(posX, posY, 0), this.transform.rotation);
        // Воспроизвести звук выстрела лазером
        GetComponent<AudioSource>().PlayOneShot(shootSound);
    }

    void OnCollisionEnter2D(Collision2D theCollision)
    {
        //Проверяем коллизию с объектом типа «лазер»
        if (theCollision.gameObject.name.Contains("laser") || theCollision.gameObject.name.Contains("playerShip"))
        {
            if (theCollision.gameObject.name.Contains("playerShip"))
            {
                health -= 100;
            }
            laserScript laser = theCollision.gameObject.GetComponent("laserScript") as laserScript;
            health = health - laser.damage;
            Destroy(theCollision.gameObject);
        }
        if (health <= 0)
        {
            // Срабатывает при уничтожении объекта
            if (explosion)
            {
                GameObject exploder = ((Transform)Instantiate(explosion, this.transform.position, this.transform.rotation)).gameObject;
                Destroy(exploder, 2.0f);
            }
            Destroy(this.gameObject);
            GameController controller = GameObject.FindGameObjectWithTag("GameController").GetComponent("GameController") as GameController;
            controller.KilledEnemy();
            controller.IncreaseScore(10);
        }
    }
}
