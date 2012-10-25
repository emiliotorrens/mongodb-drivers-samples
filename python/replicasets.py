__author__ = 'emilio.torrens'

import pymongo

connection = pymongo.ReplicaSetConnection('192.168.1.218:27017', replicaSet='devSet')
instances = connection.instances
print instances