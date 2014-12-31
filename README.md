Sample requests to the API using curl:

```
set BASEURI=http://localhost:53753

curl %BASEURI%/settings

curl %BASEURI%/settings/foo

curl -X PUT -H "Content-Type: application/json" --data "{ value:'Some value'  }" %BASEURI%/settings/foo

curl -X DELETE %BASEURI%/settings/foo
```
