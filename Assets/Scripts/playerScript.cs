using UnityEngine;
using System.Collections.Generic;

public class playerScript : MonoBehaviour {

    public Transform player;
    // Изменение скорости перемещения героя
    public float playerSpeed = 2.0f;

    // Текущая скорость перемещения
    private float currentSpeed = 0.0f;

    // Сохранение последнего перемещения
    private Vector3 lastMovement = new Vector3();

    // Переменная для лазера
    public Transform laser;

    // Как далеко от центра корабля будет появлятся лазер
    public float laserDistance = 0.2f;

    // Задержка между выстрелами (кулдаун)
    public float timeBetweenFires = 0.3f;

    // Счетчик задержки между выстрелами
    private float timeTilNextFire = 0.0f;

    // Кнопка, которая используется для выстрела
    public List<KeyCode> shootButton;

    // Переменная для звука выстрела лазером
    public AudioClip shootSound;

    // Анимация при уничтожении объекта
    public Transform explosion;

    public static int health = 10;
    public static float posX = 0.0f;
    public static float posY = 0.0f;

    public float inX;
    public float inY;

	// Use this for initialization
	void Start () {
        player.position = new Vector3(posX,posY,0);
	}
	
	// Update is called once per frame
	void Update () {
        // Поворот героя к мышке
        Rotation();
        // Перемещение героя
        Movement();
        foreach (KeyCode element in shootButton)
        {
            if (Input.GetKey(element) && timeTilNextFire < 0)
            {
                timeTilNextFire = timeBetweenFires;
                ShootLaser();
                break;
            }
        }
        timeTilNextFire -= Time.deltaTime;
	}

    // Поворот героя к мышке
    void Rotation()
    {
        float mhorizontal = Input.GetAxis("MHorizontal");
        float mvertical = Input.GetAxis("MVertical");
        // Показываем игроку, где мышка
        Vector3 worldPos = Input.mousePosition;
        worldPos = Camera.main.ScreenToWorldPoint(worldPos);
        // Сохраняем координаты указателя мыши
        inX = worldPos.x;
        inY = worldPos.y;
        float dx = this.transform.position.x - worldPos.x;
        float dy = this.transform.position.y - worldPos.y;
        // Вычисляем угол между объектами «Корабль» и «Указатель»
        float angle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        if (mhorizontal != 0 || mvertical != 0)
        {
            angle = Mathf.Atan2(mvertical, mhorizontal) * Mathf.Rad2Deg-180;
        }
        // Трансформируем угол в вектор
        Quaternion rot = Quaternion.Euler(new Vector3(0, 0, angle + 90));
        // Изменяем поворот героя
        this.transform.rotation = rot;

    }


    // Движение героя к мышке
    void Movement()
    {
        // Необходимое движение
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        // Проверка нажатых клавиш
        if (horizontal != 0 || vertical != 0)
        {
            Vector3 movement = new Vector3(horizontal, vertical, 0f);
            // Проверка нажатия кнопки
            if (movement.magnitude > 0)
            {
                // После нажатия двигаемся в этом направлении
                currentSpeed = playerSpeed;
                this.transform.Translate(movement * Time.deltaTime * playerSpeed, Space.World);
                lastMovement = movement;
            }
            else
            {
                // Если ничего не нажато
                this.transform.Translate(lastMovement * Time.deltaTime * currentSpeed, Space.World);
                // Замедление со временем
                currentSpeed *= 0.9f;
            }
        }
    }

    // Возвращает движение, если нажата кнопка
    Vector3 MoveIfPressed(List<KeyCode> keyList, Vector3 Movement)
    {
        // Проверяем кнопки из списка
        foreach (KeyCode element in keyList)
        {
            if (Input.GetKey(element))
            {
                // Если нажато, покидаем функцию
                return Movement;
            }
        }
        // Если кнопки не нажаты, то не двигаемся
        return Vector3.zero;
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
        if (theCollision.gameObject.name.Contains("blast"))
        {
            laserScript laser = theCollision.gameObject.GetComponent("laserScript") as laserScript;
            health = health - laser.damage;
            Destroy(theCollision.gameObject);
        }
        //Проверяем коллизию с объектом типа «enemy» или падение здоровья к нулю
        if (theCollision.gameObject.name.Contains("enemy") || health <= 0)
        {
            Destroy(theCollision.gameObject);
            // Срабатывает при уничтожении объекта
            if (explosion)
            {
                GameObject exploder = ((Transform)Instantiate(explosion, this.transform.position, this.transform.rotation)).gameObject;
                Destroy(exploder, 2.0f);
            }
            Destroy(this.gameObject);
        }
    }

}
