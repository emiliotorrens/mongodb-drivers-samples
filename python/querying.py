__author__ = 'emilio.torrens'

import pymongo
import random

#creamos la conexion y seleccionamos la base de datos
connection = pymongo.Connection('mongodb://localhost:27017')
database = connection['testdb']

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

persons = database.persons.find()

print 'All Persons'
#for person in persons:
#    print person

print ''
print 'Personas con age = 25'
persons = database.persons.find({'age':25})

for person in persons:
    print person

print ''
print 'Personas con age >= 30 y <= 40 ordenado por age'
persons = database.persons.find({'age':{'$gte':30,'$lte':40}}).sort('age',1)

for person in persons:
    print person


print ''
print 'Personas con age = 30 y con child age = 5 ordenado por age'
persons = database.persons.find({'$and':[{'age':30},{'childs.age':5}]}).sort('age',1)

for person in persons:
    print person

print ''
print 'Personas con Chidls age = 5, solo devolvemos el campo childs'
persons = database.persons.find({'childs.age':5}, fields = ['childs'])

for person in persons:
    print person