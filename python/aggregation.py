__author__ = 'emilio.torrens'

import pymongo
import random

#creamos la conexion y seleccionamos la base de datos
connection = pymongo.Connection('mongodb://localhost:27017')
database = connection['testdb']
database.persons.drop()

names = ["Juan", "Antonio", "Pedro", "Maria", "Jordi", "Mario"]
apellidos = ["Gomez","Perez"]

for id in range(1,100):
    #creamos un objeto persona
    apellido = apellidos[random.randrange(0,1)]
    person = dict(name= names[random.randrange(0,5)] + " " + apellido, age= random.randrange(25, 55), childs = [])
    person['childs'].append(dict(name= names[random.randrange(0,5)] + " " + apellido,age = random.randrange(1, 12)))
    person['childs'].append(dict(name= names[random.randrange(0,5)] + " " + apellido,age = random.randrange(1, 12)))

    #lo guardamos
    database.persons.insert(person)

print 'Cuantas personas de cada Nombre hay'

operation  = \
[
    {'$group': {
            '_id': '$name',
            'total': { '$sum': 1 } } }
]

results = database.command('aggregate', 'persons', pipeline= operation)

for result in results['result']:
    print result

print''
print 'Anyos Sumados de todas las personas x nombre'

operation  =\
[
    {'$group': {
        '_id': '$name',
        'total': { '$sum': '$age' } } }
]

results = database.command('aggregate', 'persons', pipeline= operation)

for result in results['result']:
    print result

print ''
print 'Cuantos hay y las edades unicas de los que se llaman Juan Gomez con edad >=40 y con hijos edad >= 3'

operation  =\
[
    {
        '$match': {'age':{'$gte': 35},'childs.age':{'$gte': 4}}
    },
    { '$group': {
        '_id': '$name',
        'total': { '$sum': 1 },
        'unique_ages': {'$addToSet': '$age'}} },
    { '$sort': { 'total': -1 } },
    { '$project' : { '_id' : 1, 'total' : 1, 'unique_ages' :1 } },
    #{ '$unwind' : '$unique_ages' }
]

results = database.command('aggregate', 'persons', pipeline= operation)

for result in results['result']:
    print result