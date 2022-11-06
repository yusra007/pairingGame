using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishSocket : MonoBehaviour
{
    public GameObject availableSocket;
    public string socketColor;
    [SerializeField] private GameManager gameManager;


    bool chose, posChange,socketSit;

    GameObject movementPosition,itself;
    public void SelectionPosition(GameObject toGoObject, GameObject Socket)
    {
        movementPosition = toGoObject;
        chose = true;


    }
    public void ChangePosition(GameObject toGoObject, GameObject Socket)
    {
        itself = Socket;
        movementPosition = toGoObject;
        posChange = true;


    } 
    public void GoToSocket(GameObject Socket)
    {
        itself = Socket;
        socketSit = true;


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(chose)
        {
            transform.position = Vector3.Lerp(transform.position, movementPosition.transform.position, .040f);
            if(Vector3.Distance(transform.position,movementPosition.transform.position)<.010f)
            {
                chose = false;
            }
        }
        if(posChange)
        {
            transform.position = Vector3.Lerp(transform.position, movementPosition.transform.position, .040f);
            if(Vector3.Distance(transform.position,movementPosition.transform.position)<.010f)
            {
                posChange = false;
                socketSit = true;
            }
        }
        if(socketSit)
        {
            transform.position = Vector3.Lerp(transform.position, itself.transform.position, .040f);
            if(Vector3.Distance(transform.position,itself.transform.position)<.010f)
            {
                Debug.Log("týk");
                gameManager.playPlugSounds();
                socketSit = false;
                gameManager.availableMovement = false;
                availableSocket = itself;

                gameManager.checkPlugs();
            }
        }
        
    }
}
