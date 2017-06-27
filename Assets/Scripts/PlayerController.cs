using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject gvrControllerPointer;
    public GameObject head;
    public GameObject body;
    public GameObject otherPlayersController;
    public GameObject playerCamera;

    public PhotonView pv;
    

    // Used to check if is this user's player or an external player
    public bool isControllable;



    private void Awake() {
        if (pv.isMine) {
            pv.RPC("setName", PhotonTargets.AllBuffered, PhotonNetwork.playerName);
        }
    }

    void Start() {
        if (isControllable) {
            playerCamera.SetActive(true);
            gvrControllerPointer.SetActive(true);
            head.SetActive(false);
            body.GetComponent<MeshRenderer>().enabled = false;
            otherPlayersController.SetActive(false);
        } else {
            playerCamera.SetActive(false);
            gvrControllerPointer.SetActive(false);
        }
    }

    [PunRPC]
    public void setName(string nm) {
        gameObject.name = nm;
    }
}
