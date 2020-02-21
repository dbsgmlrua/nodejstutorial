using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }
    public PlayerObj CurrentPlayer;
    public void SetCurrentPlayer(PlayerObj p)
    {
        CurrentPlayer = p;
        CurrentPlayer.PlayerCharacter = true;
        StartCoroutine(nameof(EventListener));
        StartCoroutine(nameof(PushNetwork));
    }

    IEnumerator EventListener()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "floor")
                    {
                        if (CurrentPlayer != null)
                            CurrentPlayer.SetDest(new Vector3(hit.point.x, 0, hit.point.z));
                    }
                }
            }
            yield return null;
        }
    }
    IEnumerator PushNetwork()
    {
        while (true)
        {
            NetworkManager.instance.SendPlayerPos(CurrentPlayer.GetPosition());
            yield return new WaitForSeconds(0.1f);
        }
    }
}
