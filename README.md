Sample requests to the API using curl:

```
set BASEURI=http://localhost:53753

curl %BASEURI%/settings

curl %BASEURI%/settings/foo

curl -X PUT -H "Content-Type: application/json" --data "{ properties: { value: 'Some value', count: 5 }  }" %BASEURI%/settings/foo

curl -X DELETE %BASEURI%/settings/foo
```
