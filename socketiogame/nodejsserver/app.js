var app = require('express')();
var server = require('http').Server(app);
var io = require('socket.io')(server);
var Player = require('./classes/Player.js');
server.listen(3000);

var clients = [];
app.get('/', function(req,res){
    res.send('hey you got back "/"');
});

io.on('connection',function(socket){
    var CurrentPlayer = new Player();
    CurrentPlayer.Name = 'unknown'
    socket.on('Login', function(data){
        console.log("playerLogin");
        var uniqueid = true;
        for(var i = 0;i<clients.length;i++){
            if(data.Name == clients[i].Name)
                uniqueid = false;
        }
        if(uniqueid){
            socket.emit('login success');
            
            for(var i = 0;i<clients.length;i++){
                var playerConnected = new Player();
                playerConnected.Name = clients[i].Name;
                playerConnected.positions = clients[i].positions;
                socket.emit('other player connected', playerConnected);
                console.log(CurrentPlayer.Name+'emit: otherPlayer :' +JSON.stringify(playerConnected));
            }
        }
        else{
            socket.emit('login fail');
        }
    });
    socket.on('play', function(data){
        var pos ={};
        CurrentPlayer.Name = data.Name;
        CurrentPlayer.positions = data.positions;
        CurrentPlayer.rotations = data.rotations;
        
        clients.push(CurrentPlayer);
        socket.broadcast.emit('other player connected', CurrentPlayer);
    })
    socket.on('player move', function(data){
        console.log(CurrentPlayer.Name + 'recv: move: ' + JSON.stringify(data));
        CurrentPlayer.positions = data.positions;
        CurrentPlayer.rotations = data.rotations;
        socket.broadcast.emit('player move', CurrentPlayer);
    });
    socket.on('player attack', function(data){

    });
    socket.on('disconnect', function(){
        console.log(CurrentPlayer.Name+'disconnected');
        socket.broadcast.emit('other player disconnected', CurrentPlayer);
        for(var i=0;i<clients.length;i++){
            if(clients[i].Name == CurrentPlayer.Name){
                clients.splice(i,1);
            }
        }
        socket.broadcast.emit('player disconnected', CurrentPlayer);
    })
    socket.on('message', function(data){
        console.log(data);
    });
});

console.log('--- server is running ...');