using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour {

    public GameObject teleportAimerObject;
    public Vector3 teleportLocation;
    public GameObject player;
    public LayerMask laserMask;
    public float yNudgeAmount = 0.5f;

    public LineRenderer laser;

    private void Start() {
        //laser = GetComponentInChildren<LineRenderer>();
    }

    void Update() {
        if (GvrController.ClickButton) {

            laser.gameObject.SetActive(true);
            teleportAimerObject.gameObject.SetActive(true);

            laser.SetPosition(0, transform.position);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 15, laserMask)) {
                teleportLocation = hit.point;
                laser.SetPosition(1, teleportLocation);
                // aimer position
                teleportAimerObject.transform.position = new Vector3(teleportLocation.x, teleportLocation.y + yNudgeAmount, teleportLocation.z);
                } else {
                //teleportLocation = transform.position + transform.forward * 15;
                RaycastHit groundRay;
                if (Physics.Raycast(teleportLocation, -Vector3.up, out groundRay, 17, laserMask)) {
                    teleportLocation = new Vector3(transform.forward.x * 15 + transform.position.x, groundRay.point.y, transform.forward.z * 15 + transform.position.z);
                }
                laser.SetPosition(1, transform.forward * 15 + transform.position);
                // aimer position
                teleportAimerObject.transform.position = teleportLocation + new Vector3(0, yNudgeAmount, 0);
                }
            }
			if (GvrController.ClickButtonUp) {
              
                laser.gameObject.SetActive(false);
                teleportAimerObject.gameObject.SetActive(false);
                player.transform.position = teleportLocation;
                //GvrLaserPointerImpl laserPointerImpl = (GvrLaserPointerImpl)GvrPointerManager.Pointer;
                //if (laserPointerImpl.IsPointerIntersecting) {
                //    gameObject.transform.position = new Vector3(laserPointerImpl.PointerIntersection.x, gameObject.transform.position.y + 1.6f, laserPointerImpl.PointerIntersection.z);
                //}
            }
	}
}
