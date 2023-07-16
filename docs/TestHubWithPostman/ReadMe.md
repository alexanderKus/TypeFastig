# Test hub with Postman

### URL

```url
wss://localhost:7130/typinghHub
```

### Init Messages

```json
{"protocol":"json","version":1}
```

Response: `{“type”:6}` - 6 means that connection is established successfully.

### Example Message

```json
{
    "arguments":[
      {
      "typistId": "31b608aa-49b8-4ec1-8e34-aaf13f13bc1b",
      "writtenWords": 82,
      "totalWords": 100,
      "madeErrors": 9
      }
    ],
    "target": "JoinRoom",
    "type":1
}
```

---

### Disclaimer

Every json ends with special ASCII character code `0X1E`
