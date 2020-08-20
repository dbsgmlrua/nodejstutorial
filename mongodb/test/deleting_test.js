const mocha = require('mocha');
const assert = require('assert');
const MarioChar = require('../models/mariochar');

describe('Deleting records', function(){
    var char;
    beforeEach(function(done){
        char = new MarioChar({
            name: 'Mario'
        });
        char.save().then(function(){
            assert(char.isNew === false);
            done();
        });
    });

    it('Delete one record from the database', function(done){
        
        MarioChar.findOneAndRemove({
            name: 'Mario'
        }).then(function(){
           MarioChar.findOne({name: 'Mario'}).then(function(result){
                assert(result === null);
                done();
           }); 
        });
    });
});