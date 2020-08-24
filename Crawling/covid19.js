const express = require('express');

const request = require('request-promise');
const cheerio = require('cheerio');

const app = express();
const port = 3000;

app.get("/", async(req,res)=>{
    const url = "https://search.naver.com/search.naver?sm=top_sug.pre&fbm=1&acr=1&acq=covid&qdt=0&ie=utf8&query=covid-19";
    let result = [];
    const html = await request(url);
    const $ = cheerio.load( html , 
        { decodeEntities: false } 
    );
    const tdElements = $("div.status_info").find("ul li");

    var temp = {};

    for(let i=0;i<4;i++){
          temp = {};
          temp["name"] = tdElements[i].children[1].children[0].data.trim();
          temp["data"] = tdElements[i].children[3].children[0].data.trim();

          result.push(temp);
    };

    res.json(result);
});

app.listen( port, function(){
    console.log('Express listening on port', port);
});