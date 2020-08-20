const mocha = require('mocha');
const assert = require('assert');
const MarioChar = require('../models/mariochar');

describe('Saving records', function(){
    it('Save a record to the database', function(done){
        var char = new MarioChar({
            name: 'Mario'
        });
        char.save().then(function(){
            assert(char.isNew === false);
            done();
        });
    });
});