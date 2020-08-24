const { graphql, buildSchema } = require('graphql');
const schema = buildSchema('type Query{ hello : String, nodejs: Int}');

const root = {
    hello : ()=> 'Hello World',
    nodejs : ()=> 20
}

graphql( schema, '{nodejs}', root).then((response)=>{
    console.log(response);
});