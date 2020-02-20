using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    private void Awake()
    {
        instance = this;
    }
    public Dictionary<string, PlayerObj> Players = new Dictionary<string, PlayerObj>();
    public string CurrentPlayerName;
    public SocketIOComponent socket;
    public GameObject playerPrepab;
    // Start is called before the first frame update
    void Start()
    {
        socket.On("player move", PlayerMove);
        socket.On("other player connected", OtherPlayerConnected);
        JoinTheGame();
    }

    void JoinTheGame()
    {
        StartCoroutine(nameof(ConnectionToServer));
    }
    IEnumerator ConnectionToServer()
    {
        yield return new WaitForSeconds(0.5f);

        //TODO change
        CurrentPlayerName = "Mario";
        PlayerJSON p = new PlayerJSON();
        p.Name = CurrentPlayerName;

        string data = JsonUtility.ToJson(p);
        socket.Emit("Login", new JSONObject(data));
    }
    #region BroadCast
    void PlayerMove(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        PlayerJSON player = PlayerJSON.CreateFromJSON(data);
        Vector3 pos = new Vector3(player.positions[0], player.positions[1], player.positions[2]);
        if (player.Name == CurrentPlayerName)
            return;

        if (Players.ContainsKey(player.Name))
            Players[player.Name].SetPosition(pos);
    }
    void OtherPlayerConnected(SocketIOEvent socketIOEvent)
    {
        Debug.Log("Someone else joined");
        string data = socketIOEvent.data.ToString();
        PlayerJSON player = PlayerJSON.CreateFromJSON(data);
        if (Players.ContainsKey(player.Name))
            return;
        else
        {
            GameObject go = Instantiate(playerPrepab, Vector3.zero, Quaternion.Euler(0, 0, 0),null);
            PlayerObj p = go.GetComponent<PlayerObj>();
            p.Name = player.Name;
            Players.Add(player.Name, p);
            if(player.Name == CurrentPlayerName)
                InputManager.instance.SetCurrentPlayer(p);
        }
    }
    public void SendPlayerPos(Vector3 pos)
    {
        string data = JsonUtility.ToJson(new PositionJSON(pos));
        socket.Emit("player move", new JSONObject(data));
    }
    #endregion
    #region JsonClasses
    public class PlayerJSON
    {
        public string Name;
        public float[] positions;
        public float[] rotations;

        public static PlayerJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<PlayerJSON>(data);
        }
    }
    public class PositionJSON
    {
        public float[] positions;
        public PositionJSON(Vector3 data)
        {
            positions = new float[] { data.x, data.y, data.z };
        }
    }
    #endregion

}
