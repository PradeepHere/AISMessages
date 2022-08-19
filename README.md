# AISMessages

# AIS data service

## Introduction
[The automatic identification system (AIS)](https://en.wikipedia.org/wiki/Automatic_identification_system) is an automatic tracking system that uses transceivers on ships and is used by vessel traffic services (VTS). AIS data that ships send is collected and used for many purposes. Also Napa collects AIS data via third parties and use it in NAPA Fleet Intelligence platform.

## Assignment
Your task is to create a web API for storing and retrieving (simplified) AIS messages.

### Specification
- AIS messages are serialized in json format.
    - AIS message is a json object with the following fields:
        - imoNumber: Seven digit number used for identifying ships. (More info for the curious: [IMO number wikipedia article](https://en.wikipedia.org/wiki/IMO_number))
        - timestamp: Time in UTC when the message was sent as a string in ISO 8601 format, e.g. "2022-08-12T05:40:17Z".
        - latitude: Latitude coordinate of the ship location.
        - longitude: Longitude coordinate of the ship location.
    - Example AIS message json:
        ```json
        {
            "imoNumber": 7654321,
            "timestamp": "2022-08-12T05:40:00Z",
            "latitude": 60.218839,
            "longitude": 25.198925
        }
        ```
- Implement the following endpoints:
    - POST /ais-messages
        - Description: Stores an AIS message.
        - Request body: AIS message as json
    - GET /ais-messages/last-daily-positions
        - Decription: Finds the last AIS message of each day after a given start date for a given ship and returns them as a json array. I.e. One AIS message (the last one) is returned for each day after the given start date for which the ship has some AIS messages stored.
        - Query parameters:
            - imoNumber: IMO number for which the data is retrived
            - startDate: Only AIS messages after startDate are returned.
        - Example:
            - If a ship has AIS messages stored with the following timestamps:
                - 2022-08-10T12:40:17Z
                - 2022-08-12T05:40:00Z
                - 2022-08-12T07:45:00Z
                - 2022-08-14T12:40:17Z
            - Response for `GET /ais-messages/last-daily-messages?imoNumber=7654321&startDate=2022-08-11` could be:
                ```json
                [
                    {
                        "imoNumber": 7654321,
                        "timestamp": "2022-08-12T07:45:00Z",
                        "latitude": 60.2,
                        "longitude": 25.1
                    },
                    {
                        "imoNumber": 7654321,
                        "timestamp": "2022-08-14T12:40:17Z",
                        "latitude": 60.0,
                        "longitude": 25.0
                    }
                ]
                ```

### Requirements
- Implement the web api with ASP.NET Core using C# or F#. You can also use any framework built on top of ASP.NET Core, such as [ASP.NET Core MVC](https://docs.microsoft.com/en-us/aspnet/core/mvc/overview?view=aspnetcore-6.0), [Giraffe](https://github.com/giraffe-fsharp/Giraffe) or similar.
- Store the AIS messages in a relational database, such as Postgres, Microsoft SQL Server, SQLite or similar.
- Do NOT use a full ORM, such as EF Core, that writes SQL queries for you. You can, though, use a simple object mapper, such as [Dapper](https://github.com/DapperLib/Dapper) or similar.
- All stored AIS messages should be kept in the database although last-daily-positions endpoint returns only the last for each day.
- Pay attention that last daily positions can be retrieved reasonably efficiently also when there are lots of ships and lots of AIS message in the database.
- Keep it simple. No need to make it "production ready" or overengineer the architecture of the code.
