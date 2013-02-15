__author__ = 'emilio.torrens'
import tornado.httpserver
import tornado.ioloop
import tornado.options
import tornado.web
from pymongo import MongoClient
from bson import json_util
import json
from bson import ObjectId

from tornado.options import define, options

define("port", default=88, help="run on the given port", type=int)
connection = MongoClient('mongodb://127.0.0.1')


class handler(tornado.web.RequestHandler):

    #find
    def get(self, database, collection):
        db = connection[database]
        col = db[collection]

        query = self.get_argument('query', None)
        if query is None:
            result = col.find({})
        else:
            dic_query = eval(query)
            result = col.find(dic_query)

        self.set_header("Content-Type", "application/json")
        self.write(json.dumps(list(result), sort_keys=True, indent=4, default=json_util.default))
        self.finish()

    #insert
    def put(self, database, collection):
        db = connection[database]
        col = db[collection]

        data = eval(self.request.body)
        id = col.insert(data)
        result = col.find({'_id': ObjectId(id)})

        self.set_header("Content-Type", "application/json")
        self.write(json.dumps(list(result), sort_keys=True, indent=4, default=json_util.default))
        self.finish()


class handler_by_id(tornado.web.RequestHandler):

    #find by _id
    def get(self, database, collection, id):

        if id is None:
            raise tornado.web.HTTPError(404)

        db = connection[database]
        col = db[collection]

        result = col.find({'_id': ObjectId(id)})

        self.set_header("Content-Type", "application/json")
        self.write(json.dumps(list(result), sort_keys=True, indent=4, default=json_util.default))
        self.finish()

    #update
    def put(self, database, collection, id):

        if id is None:
            raise tornado.web.HTTPError(404)

        db = connection[database]
        col = db[collection]

        data = eval(self.request.body)
        col.update({'_id': ObjectId(id)}, data)
        result = col.find({'_id': ObjectId(id)})

        self.set_header("Content-Type", "application/json")
        self.write(json.dumps(list(result), sort_keys=True, indent=4, default=json_util.default))
        self.finish()

   #delete
    def delete(self, database, collection, id):

        if id is None:
            raise tornado.web.HTTPError(404)

        db = connection[database]
        col = db[collection]

        col.remove({'_id': ObjectId(id)})
        self.finish()


if __name__ == "__main__":
    tornado.options.parse_command_line()
    app = tornado.web.Application(
        handlers=[
            (r"/(.+)/(.+)/(.+)", handler_by_id),
            (r"/(.+)/(.+)/", handler), (r"/(.+)/(.+)", handler),
        ],
        gzip=True,
        #template_path=os.path.join(os.path.dirname(__file__), "templates")
    )
    http_server = tornado.httpserver.HTTPServer(app)
    http_server.listen(options.port)
    tornado.ioloop.IOLoop.instance().start()


