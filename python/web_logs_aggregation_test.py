__author__ = 'emilio.torrens'

import pymongo
connection = pymongo.Connection('mongodb://192.168.1.218:27017')
database = connection['CloudManager_Dev']

operation  =\
[
    {'$group': {
        '_id': {'installation':'$value.installation', 'service':'$value.service'},
        'total': { '$sum': '$value.count' } ,
        'average': { '$avg': '$value.time_taken' },
        'origins': {'$addToSet': '$value.origin'}
        }
    }
    ,{'$match': { 'total': {'$gt': 10000} }}
    ,{'$project': {'source':'$origins','_id':0,'inst':'$_id.installation', 'count':'$total','average':1}}
    ,{'$sort':{'total':-1}}
    ,{'$unwind' : '$source'}
    ,{'$limit':10}
]

results = database.command('aggregate', 'web_logs_reduced_112012', pipeline= operation)

for result in results['result']:
    print result

