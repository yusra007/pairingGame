using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    GameObject choseObje;
    GameObject choseSocket;
    public bool availableMovement;

    [Header("Level Ayarlarý")]
    public GameObject[] collisionConditionObject; //çarpýþma kontrol objeleri
    public GameObject[] Sockets; // fiþler
    public int numberOfTargetSockets; //hedef soket sayýsý
    public List<bool> collisionConditions; //çarpýþma durumlarý 
    [SerializeField] private int rightOfMove;
    int numberOfCompletions; //tamamlanma sayýsý
    int multiplyCheckCount; //çarpma kontrol sayýsý

    [Header("uý objeleri")]
    [SerializeField] private GameObject ControlPanel;
   
    [SerializeField] private TextMeshProUGUI ControlText;
    [SerializeField] private TextMeshProUGUI rightOfMoveText;
    //[SerializeField] private GameObject[] GeneralPanel;
    [SerializeField] private TextMeshProUGUI[] UITexts;
    [SerializeField] private GameObject pausePanel;

    [SerializeField] private GameObject restartpnl;
    [SerializeField] private GameObject nextlvlpnl;


    [Header("other objeleri")]
    [SerializeField] private GameObject[] lights;
    [SerializeField] private AudioSource plugVoice;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        restartpnl.SetActive(false);
        nextlvlpnl.SetActive(false);
        rightOfMoveText.text ="Moves: "+ rightOfMove.ToString();
        for (int i = 0; i < numberOfTargetSockets-1; i++)
        {
            collisionConditions.Add(false);
        }
        UITexts[3].text = PlayerPrefs.GetInt("Money").ToString();
    }
    public void checkPlugs()
    {
        foreach (var item in Sockets)
        {
            if(item.GetComponent<FinishSocket>().availableSocket.name==item.GetComponent<FinishSocket>().socketColor)
            {
                numberOfCompletions++;
            }
        }
        if(numberOfCompletions== numberOfTargetSockets)
        {

            Debug.Log("soket yerinde");
            foreach (var item in collisionConditionObject)
            {
                item.SetActive(true);
            }
            StartCoroutine(collisionIsThere());
            
        }
        else
        {
            if(rightOfMove<=0)

            {
                lost();
            }
            

        }

        numberOfCompletions = 0;

    }
    
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100))
            {
                if(hit.collider !=null)
                {
                    //finishsocket

                    if (choseObje==null&&!availableMovement)
                    {
                        if (hit.collider.CompareTag("BlueSocket") || hit.collider.CompareTag("YellowSocket") || hit.collider.CompareTag("RedSocket"))
                        {
                            plugVoice.Play();
                            FinishSocket _finishSocket = hit.collider.GetComponent<FinishSocket>();
                            _finishSocket.SelectionPosition(_finishSocket.availableSocket
                                .GetComponent<socket>().MovementPosition, _finishSocket.availableSocket);

                            choseObje = hit.collider.gameObject;
                            choseSocket = _finishSocket.availableSocket;
                            availableMovement = true;


                        }
                    }
                    //finishsocket

                    //socket

                    if (hit.collider.CompareTag("plugSocket"))
                    {
                        if(choseObje!=null&&!hit.collider.GetComponent<socket>().Full&&choseSocket!=hit
                            .collider.gameObject)
                        {
                            choseSocket.GetComponent<socket>().Full=false;
                            socket _socket = hit.collider.GetComponent<socket>();
                            choseObje.GetComponent<FinishSocket>().ChangePosition(_socket.MovementPosition,hit.collider.gameObject);
                            _socket.Full = true;
                            choseObje =null;
                            choseSocket =null;
                            availableMovement = true;
                            rightOfMove--;
                            rightOfMoveText.text = "Moves: " + rightOfMove.ToString();
                        }
                        else if(choseSocket==hit.collider.gameObject)
                        {


                            choseObje.GetComponent<FinishSocket>().GoToSocket(hit.collider.gameObject);
                            choseObje = null;
                            choseSocket = null;
                            availableMovement = true;
                        }
                    }


                    //socket


                }

            }
        }
    }
    public void checkForCollision(int collisionIndex,bool condition)
    {
        collisionConditions[collisionIndex] = condition;
       


    }
    IEnumerator collisionIsThere()
    {

        lights[0].SetActive(false);
        lights[1].SetActive(true);
        ControlPanel.SetActive(true);
        ControlText.text = "CONTROLLED...";


        
        yield return new WaitForSeconds(2f);
        foreach (var item in collisionConditions)
        {
            if (item)
            {
                multiplyCheckCount++;
            }

        }
        if (multiplyCheckCount == collisionConditions.Count)
        {
            lights[1].SetActive(false);
            lights[2].SetActive(true);
            ControlText.text = "You Win...";
            nextlvlpnl.SetActive(true);
            
        }
        else
        {
            lights[1].SetActive(false);
            lights[0].SetActive(true);
            ControlText.text = "You Lost...";
            

            Invoke("closeThePanel", 2f);
            foreach (var item in collisionConditionObject)
            {
                item.SetActive(false);
            }

            if (rightOfMove <= 0)
            {
                lost();//oyun bitti panel
                
            }

               
            
        }
        
        
        
        multiplyCheckCount = 0;
    }
    void closeThePanel()
    {
        ControlPanel.SetActive(false);
    }

    public void stopbtn()
    {
        PauseGame();
    }
    public void playbtn()
    {
        ContinueGame();
    }

    public void exitBtn()
    {
        Application.Quit();
    }
    private void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        //Disable scripts that still work while timescale is set to 0
    }
    private void ContinueGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        //enable the scripts again
    }
    public void nextLevelBtn()
    {
        
        win();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void replayLevelbtn()
    {
        lost();
        
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //public void operationButton(string value)
    //{
    //    switch (value)
    //    {
    //        case "StopPanel":
    //            GeneralPanel[0].SetActive(true);
    //            Time.timeScale = 0;
    //            break;
    //        case "playbtn":
    //            GeneralPanel[0].SetActive(false);
    //            Time.timeScale = 1;
    //            break;
    //        case "ReplayLvlbtn":
    //            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    //            Time.timeScale = 1;
    //            break;
    //        case "SettingBtn":
    //            break;

    //        case "exitbtn":
    //            Application.Quit();
    //            break;
    //    }
    //}
    void win()
    {

        lights[1].SetActive(false);
        lights[2].SetActive(true);
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        UITexts[0].text = "Level : " + SceneManager.GetActiveScene().name;
        ControlText.text = "You Win";


        int randomMoney = Random.Range(5, 20);
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + randomMoney);
        UITexts[2].text = "Money : " + randomMoney;
        //GeneralPanel[1].SetActive(true);
        Time.timeScale = 0;


    }
    void lost()
    {
        restartpnl.SetActive(true);

        UITexts[1].text = "Level : " + SceneManager.GetActiveScene().name;
        //GeneralPanel[2].SetActive(true);
        Time.timeScale = 0;
    }
    public void playPlugSounds()
    {
        plugVoice.Play();
    }
}
