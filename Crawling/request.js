const request = require('request');
const { response } = require('express');

const url = "http://ncov.mohw.go.kr/en/";

request(url, (error, response, body) =>{
    console.log(body);
});