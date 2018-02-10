curl -XPUT 'localhost:9200/service_comments_v01?pretty' -H 'Content-Type: application/json' -d'
{
    "settings" : {
        "index" : {
            "number_of_shards" : 1,
            "number_of_replicas" : 1
        }
    },
    "aliases" : {
        "service_comments_write" : {},
        "service_comments_read" : {}
    }
}'
