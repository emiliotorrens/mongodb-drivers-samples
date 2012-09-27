__author__ = 'emilio.torrens'

import pymongo

#creamos la conexion y seleccionamos la base de datos
connection = pymongo.Connection('mongodb://localhost:27017')
database = connection['testdb']
database.persons.drop()

#creamos un objeto persona
person = dict(name="Person Name", age=25, childs = [])
person['childs'].append(dict(name="child 1",age =12))
person['childs'].append(dict(name="child 2",age =6))

#lo guardamos
database.persons.insert(person)
person = database.persons.find_one()

print person

#modificamos el objeto y lo enviamos entero
person['name'] = "person Name 2";
person['age'] = 35;
person['childs'][0]['age'] = 5;
database.persons.save(person)

person = database.persons.find_one()

print person

#le modificamos el nombre
database.persons.update({'_id': person['_id']},{'$set':{'name':'Name Modified'}})
person = database.persons.find_one()

print person

#le anyadimos un hijo
database.persons.update({'_id': person['_id']},{'$push':{'childs':dict(name="child 3",age =3)}})
person = database.persons.find_one()

print person



