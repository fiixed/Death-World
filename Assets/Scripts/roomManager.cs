using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class roomManager : MonoBehaviour
{
    private GameObject currentPlayer;

    public string gameVersion = "0.01 Alpha";
    public GameObject playerPrefab;
    public Transform[] spawnPoints;
    public GUISkin guiSkin;

    public Text winnerText;

    public string[] weaponsInOrder;

    bool roundOver = false;

    public string leadingPlayer = "Nobody";

    public GameObject UI;
    public GameObject spawnUI;
    public GameObject roundOverUI;

    [HideInInspector]
    public bool isSpawned = false;

    void Awake()
    {
        if (!PhotonNetwork.connected)
        {
            PhotonNetwork.ConnectUsingSettings(gameVersion);


        }

        if (!PhotonNetwork.inRoom)
        {
            spawnUI.SetActive(false);
        }

        if (PhotonNetwork.playerName == null)
        {
            PhotonNetwork.playerName = "Player " + Random.Range(0, 999);
        }
        PhotonNetwork.player.SetScore(0);
        Debug.Log(PhotonNetwork.playerName);
    }


    public int highestScore = 0;

    void Update()
    {


        foreach (PhotonPlayer pl in PhotonNetwork.playerList)
        {
            if (pl.GetScore() > highestScore)
            {
                highestScore = pl.GetScore();
                leadingPlayer = pl.NickName;
            }
        }

        AudioListener aL = GetComponentInChildren<AudioListener>();

        if(aL != null)
        {
            aL.enabled = !isSpawned;
        }

        

        if (!roundOver)
        {
            UI.SetActive(!isSpawned);
        }
        else
        {
            UI.SetActive(true);
            winnerText.text = leadingPlayer + " Won!";
        }

        if (highestScore > weaponsInOrder.Length - 1)
        {
            roundOver = true;

            //foreach (characterControls cc in FindObjectsOfType<characterControls>())
            //{
            //    cc.pv.RPC("destroy", PhotonTargets.AllBuffered, null);
            //}

            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

            }

            if (PhotonNetwork.room.IsVisible)
            {
                PhotonNetwork.room.IsVisible = false;
            }

        }

        spawnUI.SetActive(!roundOver);
        roundOverUI.SetActive(roundOver);
    }


    void OnJoinedRoom()
    {

        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        currentPlayer = PhotonNetwork.Instantiate("Player", spawn.position, spawn.rotation, 0);

        //currentPlayer = PhotonNetwork.Instantiate("Player", new Vector3(0, 1.6f, 0), Quaternion.identity, 0);
        currentPlayer.GetComponent<PlayerController>().isControllable = true;

    }

    //public void spawnPlayer()
    //{
    //    if (PhotonNetwork.inRoom)
    //    {
    //        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
    //        GameObject pl = PhotonNetwork.Instantiate(playerPrefab.name, spawn.position, spawn.rotation, 0) as GameObject;
    //        isSpawned = true;
            //if (pl != null)
            //{
            //    if (pl.GetComponent<characterControls>() != null)
            //    {
            //        characterControls cc = pl.GetComponent<characterControls>();
            //        cc.enabled = true;
            //        cc.fpCam.SetActive(true);

            //        foreach (SkinnedMeshRenderer smr in cc.skinnedPlayerMeshes)
            //        {
            //            smr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            //        }

            //        foreach (MeshRenderer mr in cc.playerMeshes)
            //        {
            //            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            //        }
            //    }
            //    else
            //    {
            //        Debug.LogError("Character Controls not found on Player Prefab!");
            //    }
            //}
    //    }
    //}

    //void OnGUI()
    //{
    //    GUI.skin = guiSkin;
    //    if (!roundOver)
    //    {
    //        string leadingWep = weaponsInOrder[highestScore];
    //        GUI.Box(new Rect(Screen.width / 2 - 100, 10, 200, 30), "Leader: " + leadingPlayer + " [" + leadingWep + "]");
    //    }
    //}

    public void disconnectRoom()
    {
        PhotonNetwork.LeaveRoom();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
