var app = require('express')();
var server = require('http').Server(app);
var io = require('socket.io')(server);

server.listen(3000);

var clients = [];
app.get('/', function(req,res){
    res.send('hey you got back "/"');
});

io.on('connection',function(socket){
    var CurrentPlayer = {};
    CurrentPlayer.Name = 'unknown'
    socket.on('Login', function(data){
        console.log("playerLogin");
        for(var i = 0;i<clients.length;i++){
            var playerConnected = {
                Name:clients[i].Name,
                position:clients[i].position
            };
            socket.emit('other player connected', playerConnected);
            console.log(CurrentPlayer.Name+'emit: otherPlayer :' +JSON.stringify(playerConnected));
        }
        var pos ={};
        CurrentPlayer = {
            Name:data.Name,
        }
        clients.push(CurrentPlayer);
        socket.emit('other player connected', CurrentPlayer);
    });

    socket.on('player move', function(data){
        console.log(CurrentPlayer.Name + 'recv: move: ' + JSON.stringify(data));
        CurrentPlayer.positions = data.positions;
        socket.broadcast.emit('player move', CurrentPlayer);
    });
    socket.on('disconnect', function(){
        console.log(CurrentPlayer.Name+'disconnected');
        socket.broadcast.emit('other player disconnected', CurrentPlayer);
        for(var i=0;i<clients.length;i++){
            if(clients[i].Name == CurrentPlayer.Name){
                clients.splice(i,1);
            }
        }
    })
    socket.emit('message', {hello:'world'});
    socket.on('message', function(data){
        console.log(data);
    });
});

console.log('--- server is running ...');