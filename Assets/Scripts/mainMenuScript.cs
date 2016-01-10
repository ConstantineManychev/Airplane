using UnityEngine;
using System.Collections;

public class mainMenuScript : MonoBehaviour {

    //переменные для отлова нажатых кнопок
    public bool newGame = false;
    public bool load = false;
    public bool settings = false;
    public bool exit = false;

    public Renderer rend;

    public Camera mainCamera;
    public Camera settingsCamera;
    public Camera loadCamera;

    public bool audioBtn = false;
    public bool videoBtn = false;
    public bool controlBtn = false;
    public bool backBtn = false;

    bool loadMenu = false;
    public const int slotsAmount = 10;  // количество слотов загрузки/сохранения
    // массив наших скриншотов
    private Texture2D[] saveTexture = new Texture2D[slotsAmount]; 

	// Use this for initialization
	void Start () {
        mainCamera.enabled = true;
        settingsCamera.enabled = false;
        loadCamera.enabled = false;
        rend.sharedMaterial.color = Color.yellow;
	}

    void OnMouseEnter (){
        rend.material.color = Color.red;
    }

    void OnMouseExit()
    {
        rend.material.color = Color.yellow;
    }

    void OnMouseUp()
    {
        if (newGame == true) 
        {
            Application.LoadLevel(1);
        }
        else if (load == true)
        {
            mainCamera.enabled = false;
            settingsCamera.enabled = false;
            loadCamera.enabled = true;
            loadMenu = true;
        }
        else if (settings == true)
        {
            mainCamera.enabled = false;
            loadCamera.enabled = false;
            settingsCamera.enabled = true;
        }
        else if (backBtn == true)
        {
            mainCamera.enabled = true;
            settingsCamera.enabled = false;
            loadCamera.enabled = false;
            loadMenu = false;
        }
        else if (exit == true)
        {
            Application.Quit();
        }
    }

    void OnGUI()
    {
        if (loadMenu == true)
        {
            int slot = GUI.SelectionGrid(
                                new Rect(	// подстраиваем под размер экрана сетку 5x2
                                        Screen.width / 2 - Screen.height * 5 / 9, // с соотношением
                                        Screen.height / 3,	// сторон кнопки 4:3
                                        Screen.height * 10 / 9,
                                        Screen.height / 3
                                ),
                                -1,  // индекс вне пределов массива
                                saveTexture,  // если null, то просто не рисует
                                5);  // количество элементов в строке

            if (slot >= 0)  // выполнится только в случае клика
            {
                loadgame(slot);
                loadMenu = false;
            }
        }
    }

    void loadgame(int i)
    {
        if (PlayerPrefs.GetInt("slot" + i + "_Lvl") != 0)
        {
            Application.LoadLevel(PlayerPrefs.GetInt("slot" + i + "_Lvl"));//загружаем уровень

            playerScript.posX = PlayerPrefs.GetFloat("slot[" + i + "]_PlayerX");
            playerScript.posY = PlayerPrefs.GetFloat("slot[" + i + "]_PlayerY");

            GameController.score = PlayerPrefs.GetInt("slot[" + i + "]_Score");
            GameController.waveNumber = PlayerPrefs.GetInt("slot[" + i + "]_WavesEnds");

            playerScript.health = PlayerPrefs.GetInt("slot[" + i + "]_PlayerHealth");
        }
    }
}
