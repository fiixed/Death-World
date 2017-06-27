using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class mainMenu : MonoBehaviour {

    public string defaultScene = "1";
    public string defaultRoom = "Room 1";
    public string playerName;


    public string version = "0.1 Alpha";


    bool createRoom = false;

    RoomOptions ro;


    void Start()
    {
        //defaultScene = maps[0].displayName;
        ro = new RoomOptions();
        ro.MaxPlayers = 8;
        if (!PhotonNetwork.connected) {
            PhotonNetwork.ConnectUsingSettings(version);
        }

        playerName = "Player " + Random.Range(0, 999);

    }

    public void quickPlay()
    {
        string newRoom = "Room1";
        if (PhotonNetwork.GetRoomList().Length > 0)
        {
            joinCreate(newRoom);
            Debug.Log(newRoom);
            //find random room 
            //joinCreate(PhotonNetwork.GetRoomList()[Random.Range(0, PhotonNetwork.GetRoomList().Length)].Name);
        }
        else
        {
           
            joinCreate(newRoom);
            Debug.Log(newRoom);
        }
    }


    public void joinCreate(string nme)
    {
        if (PhotonNetwork.connectedAndReady)
        {
            setName();
            PhotonNetwork.JoinOrCreateRoom(nme, ro, null);
            //string[] roomNme = nme.Split(nameSeperator[0]);
            //Debug.Log(roomNme[0] + "/" + roomNme[1]);
           
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
             
            }
        }

    public void setName() {
        if (PhotonNetwork.connected) {
            PhotonNetwork.playerName = this.playerName;
        }
    }
}

   

   
    
       
    
    

