var http = require('http');
var fs = require('fs');
const { json } = require('body-parser');

var server = http.createServer(function(req,res){
    console.log('request was made: ' + req.url);
    //res.writeHead(200,{'Content-Type': 'text/html'});
    res.writeHead(200,{'Content-Type': 'application/json'});
    //var myReadStream = fs.createReadStream(__dirname + '/index.html', 'utf8');
    //myReadStream.pipe(res);

    var myObj = {
        name: 'HK',
        job: 'Programmer',
        age: 28
    };
    res.end(JSON.stringify(myObj));
});

server.listen(3000,'127.0.0.1');
console.log('Now listening to port 3000');