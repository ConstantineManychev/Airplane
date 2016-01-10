using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    // Создание переменной «враг»
    public Transform enemy;
    public Transform enemy2;

    // Временные промежутки между событиями, кол-во врагов
    public float timeBeforeSpawning = 1.5f;
    public float timeBetweenEnemies = 1.0f;
    public float timeBeforeWaves = 2.0f;
    public int enemiesPerWave = 10;
    public int currentNumberOfEnemies = 0;

    // Переменные для вывода на экран
    public static int score = 0;
    public static int waveNumber = 0;

    // Ссылки на текстовые объекты
    public GUIText scoreText;
    public GUIText waveText;

	// Use this for initialization
	void Start () {

        StartCoroutine(SpawnEnemies());
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Появление волн врагов
    IEnumerator SpawnEnemies()
    {
        // Начальная задержка перед первым появлением врагов
        yield return new WaitForSeconds(timeBeforeSpawning);
        // Когда таймер истекёт, начинаем производить эти действия
        while (true)
        {
            // Не создавать новых врагов, пока не уничтожены старые
            if (currentNumberOfEnemies <= 0)
            {
                if (score < 150)
                {
                    waveNumber++;
                    waveText.text = "Волна: " + waveNumber;
                    float randDirection;
                    float randDistance;
                    // Создать 10 врагов в случайных местах за экраном
                    for (int i = 0; i < enemiesPerWave; i++)
                    {
                        // Задаём случайные переменные для расстояния и направления
                        randDistance = Random.Range(10, 25);
                        randDirection = Random.Range(0, 360);
                        // Используем переменные для задания координат появления врага
                        float posX = this.transform.position.x + (Mathf.Cos((randDirection) * Mathf.Deg2Rad) * randDistance);
                        float posY = this.transform.position.y + (Mathf.Sin((randDirection) * Mathf.Deg2Rad) * randDistance);
                        // Создаём врага на заданных координатах
                        Instantiate(enemy, new Vector3(posX, posY, 0), this.transform.rotation);
                        currentNumberOfEnemies++;
                        yield return new WaitForSeconds(timeBetweenEnemies);
                    }
                }
                else if (score >= 150 && score < 250)
                {
                    waveNumber++;
                    waveText.text = "Волна: " + waveNumber;
                    float randDirection;
                    float randDistance;
                    // Создать 10 врагов в случайных местах за экраном
                    for (int i = 0; i < enemiesPerWave; i++)
                    {
                        // Задаём случайные переменные для расстояния и направления
                        randDistance = Random.Range(10, 25);
                        randDirection = Random.Range(0, 360);
                        // Используем переменные для задания координат появления врага
                        float posX = this.transform.position.x + (Mathf.Cos((randDirection) * Mathf.Deg2Rad) * randDistance);
                        float posY = this.transform.position.y + (Mathf.Sin((randDirection) * Mathf.Deg2Rad) * randDistance);
                        // Создаём врага на заданных координатах
                        Instantiate(enemy2, new Vector3(posX, posY, 0), this.transform.rotation);
                        currentNumberOfEnemies++;
                        yield return new WaitForSeconds(timeBetweenEnemies);
                    }
                }
                else if (score >= 250 && score < 400)
                {
                    waveNumber++;
                    waveText.text = "Волна: " + waveNumber;
                    float randDirection;
                    float randDistance;
                    // Создать 10 врагов в случайных местах за экраном
                    for (int i = 0; i < enemiesPerWave/2; i++)
                    {
                        // Задаём случайные переменные для расстояния и направления
                        randDistance = Random.Range(10, 25);
                        randDirection = Random.Range(0, 360);
                        // Используем переменные для задания координат появления врага
                        float posX = this.transform.position.x + (Mathf.Cos((randDirection) * Mathf.Deg2Rad) * randDistance);
                        float posY = this.transform.position.y + (Mathf.Sin((randDirection) * Mathf.Deg2Rad) * randDistance);
                        // Создаём врага на заданных координатах
                        Instantiate(enemy2, new Vector3(posX, posY, 0), this.transform.rotation);
                        currentNumberOfEnemies++;
                        yield return new WaitForSeconds(timeBetweenEnemies);
                        Instantiate(enemy, new Vector3(posX, posY, 0), this.transform.rotation);
                        currentNumberOfEnemies++;
                        yield return new WaitForSeconds(timeBetweenEnemies);
                    }
                }
            }
            // Ожидание до следующей проверки
            yield return new WaitForSeconds(timeBeforeWaves);
        }
    }

    // Процедура уменьшения количества врагов в переменной
    public void KilledEnemy()
    {
        currentNumberOfEnemies--;
    }

    public void IncreaseScore(int increase)
    {
        score += increase;
        scoreText.text = "Очки: " + score;
    }
}
