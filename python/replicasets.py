__author__ = 'emilio.torrens'

import pymongo

connection = pymongo.ReplicaSetConnection('192.168.1.69:27017', replicaSet='mySet')
instances = connection.instances
print instances