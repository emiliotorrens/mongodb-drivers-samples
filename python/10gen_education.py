__author__ = 'emilio.torrens'

import pymongo
from bson.code import Code

#creamos la conexion y seleccionamos la base de datos
connection = pymongo.Connection('mongodb://localhost:27017')
#database = connection['students']
#
#ids = database.grades.distinct('student_id')
#
#for id in ids:
#    docs = database.grades.find({'student_id':id,'type':'homework'}).sort('score',1)
#    database.grades.remove(docs[0])

database = connection['test']

#operation  =\
#[
#    {'$group': {
#        '_id': '$state',
#        'total': { '$sum': 1 } } },
#    { '$sort': { 'total': -1 } }
#]
#
#results = database.command('aggregate', 'zips', pipeline= operation)
#
#for result in results['result']:
#    print result


map =   Code('function map_closest() {'
            'var pitt = [-80.064879, 40.612044];'
            'var phil = [-74.978052, 40.089738];'
            'function distance(a, b) {'
                'var dx = a[0] - b[0];'
                'var dy = a[1] - b[1];'
                'return Math.sqrt(dx * dx + dy * dy);'
            '}'
            'if (distance(this.loc, pitt) < distance(this.loc, phil)) {'
                'emit("pitt", 1);'
            '} else {'
                    'emit("phil", 1);'
                '}'
            '}')

reduce = Code('function Reduce(key, values) {'
              'var total = 0;'
              'for(var i in values) {'
                'total += values[i];'
              '}'
              'return total;'
              '}')

database.zips.map_reduce(map, reduce, "results", query = {'state':'PA'})
results = database.results.find()

pitt = 0
phil = 0

for result in results:
    if str(result['_id']) == 'pitt':
        pitt = int(result['value'])
    if str(result['_id']) == 'phil':
        phil = int(result['value'])

    print result

print phil - pitt



