using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{
    public GameObject LoginWindow;
    public InputField LoginId;
    public Text ErrorMsg;
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
        LoginWindow.SetActive(true);
        socket.On("login success", onLoginSuccess);
        socket.On("login fail", onLoginFail);
        socket.On("player move", PlayerMove);
        socket.On("other player connected", OtherPlayerConnected);
        socket.On("player disconnected", OtherPlayerDisconnected);
        JoinTheGame();
    }
    public bool ClickedCommitButton = false;
    public void CommitButton()
    {
        ClickedCommitButton = true;
    }
    void JoinTheGame()
    {
        StartCoroutine(nameof(ConnectionToServer));
    }
    IEnumerator ConnectionToServer()
    {
        while (true)
        {
            GetLoginResult = false;
            ClickedCommitButton = false;
            loginSuccess = false;
            yield return new WaitUntil(() => ClickedCommitButton);
            CurrentPlayerName = LoginId.text;
            LoginForm l = new LoginForm(CurrentPlayerName);
            string d = JsonUtility.ToJson(l);
            socket.Emit("Login",new JSONObject(d));
            yield return new WaitUntil(() => GetLoginResult);
            if (loginSuccess)
                break;
        }
        LoginWindow.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        //TODO change
        PlayerJSON player = new PlayerJSON();
        player.Name = CurrentPlayerName;
        player.SetPosition(Vector3.zero);
        string data = JsonUtility.ToJson(player);
        yield return new WaitForSeconds(0.5f);
        socket.Emit("play", new JSONObject(data));
        yield return new WaitForSeconds(0.2f);
        GameObject go = Instantiate(playerPrepab, Vector3.zero, Quaternion.Euler(0, 0, 0), null);
        PlayerObj p = go.GetComponent<PlayerObj>();
        p.Name = player.Name;
        Players.Add(p.Name, p);
        p.SetPosition(p.transform.position);
        InputManager.instance.SetCurrentPlayer(p);
    }
    #region BroadCast
    bool loginSuccess = false;
    bool GetLoginResult = false;
    void onLoginSuccess(SocketIOEvent socketIOEvent)
    {
        GetLoginResult = true;
        loginSuccess = true;
    }
    void onLoginFail(SocketIOEvent socketIOEvent)
    {
        GetLoginResult = true;
        loginSuccess = false;
        ErrorMsg.text = "Input Another Name!";
    }
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
            p.SetPosition(p.transform.position);
            Players.Add(p.Name, p);
        }
    }
    void OtherPlayerDisconnected(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        PlayerJSON player = PlayerJSON.CreateFromJSON(data);
        if (Players.ContainsKey(player.Name))
        {
            PlayerObj p = Players[player.Name];
            Players.Remove(player.Name);
            Destroy(p.gameObject);
        }
    }
    public void SendPlayerPos(Vector3 pos)
    {
        string data = JsonUtility.ToJson(new PositionJSON(pos));
        socket.Emit("player move", new JSONObject(data));
    }
    #endregion
    #region JsonClasses
    public class LoginForm
    {
        public string Name;
        public LoginForm(string n)
        {
            Name = n;
        }
    }
    public class PlayerJSON
    {
        public string Name;
        public float[] positions;

        public static PlayerJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<PlayerJSON>(data);
        }
        public void SetPosition(Vector3 data)
        {
            positions = new float[] { data.x, data.y, data.z };
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
