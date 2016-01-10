using UnityEngine;
using System.Collections;

public class pmenu : MonoBehaviour {

    private Transform player;

    public string Restart;
    public string MainMenu;
    bool  go=false;
    bool  menu1=false;
    bool menu2 = false;
    bool  paused = false;
    bool saveMenu = false;
    bool loadMenu = false;

    public const int slotsAmount = 10;  // количество слотов загрузки/сохранения
    // массив наших скриншотов
    private Texture2D[] saveTexture = new Texture2D[slotsAmount]; 

    void Start()
    {
        player = GameObject.Find("playerShip").transform;
    }

    void  Update (){
        if (player == null)
        {
            menu2 = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton11))
        { 
          if(!paused){ 
              Time.timeScale = 0; 
              paused=true; 
              menu1=true;
          }
        } 
    } 


    void  OnGUI ()
    {
        if (menu1==true) {
            if (GUI.Button( new Rect((Screen.width/2)-25,(Screen.height/2),100,30),"Continue")) {menu1=false;paused=false;Time.timeScale = 1;}
            if (GUI.Button( new Rect((Screen.width/2)-25,(Screen.height/2)+30,100,30),"Restart"))  Application.LoadLevel(Restart);
            if (GUI.Button(new Rect((Screen.width / 2) - 25, (Screen.height / 2) + 60, 100, 30), "Save")) { menu1 = false; saveMenu = true; }
            if (GUI.Button(new Rect((Screen.width / 2) - 25, (Screen.height / 2) + 90, 100, 30), "Load")) { menu1 = false; loadMenu = true; }
            if (GUI.Button( new Rect((Screen.width/2)-25,(Screen.height/2)+120,100,30),"Main menu")) {menu1=false;go=true;}
        }

        if (go==true){
            if (GUI.Button(new Rect((Screen.width / 2) - 55, (Screen.height / 2), 100, 30), "Yes")) Application.LoadLevel(MainMenu);
            if (GUI.Button( new Rect((Screen.width/2)+55,(Screen.height/2),100,30),"No")) {go=false;menu1=true;}
        }

		//сохранение, то соответствующий элемент массива содержит текстуру в виде скриншота, иначе null
        if (saveMenu == true || loadMenu == true)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height * 2 / 3 + 50, 200, 30), "Back")) { saveMenu = false; loadMenu = false; menu1 = true; }

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
                if (saveMenu == true) 
                { 
                    savegame(slot);
                    saveMenu = false; 
                    loadMenu = false;
                    paused = false;
                    Time.timeScale = 1;
                }	// Если это было меню сохранения - сохраняем
                else if (loadMenu == true)
                {
                    loadgame(slot);
                    saveMenu = false;
                    loadMenu = false;
                    paused = false;
                    Time.timeScale = 1;
                }
        }
        if (menu2 == true)
        {
            if (GUI.Button(new Rect((Screen.width / 2) - 25, (Screen.height / 2), 100, 30), "Restart")) Application.LoadLevel(Restart);
            if (GUI.Button(new Rect((Screen.width / 2) - 25, (Screen.height / 2) + 30, 100, 30), "Main menu")) Application.LoadLevel(MainMenu);
        }
    }

    void savegame(int i)
    {

        PlayerPrefs.SetInt("slot" + i + "_Lvl", Application.loadedLevel);  // сохраняем текущий уровень

        PlayerPrefs.SetFloat("slot[" + i + "]_PlayerX", player.position.x);
        PlayerPrefs.SetFloat("slot[" + i + "]_PlayerY", player.position.y);

        PlayerPrefs.SetInt("slot[" + i + "]_Score", GameController.score);
        PlayerPrefs.SetInt("slot[" + i + "]_WavesEnds", GameController.waveNumber);

        PlayerPrefs.SetInt("slot[" + i + "]_PlayerHealth", playerScript.health);
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

