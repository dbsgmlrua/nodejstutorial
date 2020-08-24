const express = require('express');
const { graphqlHTTP } = require('express-graphql');
const { buildSchema } = require('graphql');

const schema = buildSchema('type Product{id : ID!, name : String, price : Int, description : String} type Query{ getProduct( id : ID! ) : Product }');

const products =[{
    id : 1,
    name : 'firstproduct',
    price : 2000,
    description : '123123123'
},{
    id : 2,
    name : 'secondproduct',
    price : 4000,
    description : '2222222'
}];

const root = {
    getProduct : ({id})=> products.find(product=> product.id === parseInt(id) )
}

const app = express();
app.use('/graphql', graphqlHTTP({
    schema : schema,
    rootValue : root,
    graphiql : true
}));

app.listen(4000,()=>{
    console.log('listening on port 4000');
});