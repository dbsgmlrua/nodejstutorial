const port = 3000,
express = require("express"),
app = express();

app.get("/", (req,res) => {
    res.send("HelloUniverse");
    console.log(req.params);
    console.log(req.body);
    console.log(req.url);
    console.log(req.query);
}).listen(port, ()=>{
    console.log(`the express.js server has started and is listening on port number: ${port}`);
});

app.post("/contact",(req,res)=>{
    res.send("Contact information submitted successfully")
})