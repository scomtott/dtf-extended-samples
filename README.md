# DTF Extended Samples

The goal of this repo is to provide non trivial usage examples of the [DTF Framework](https://github.com/Azure/durabletask) in an minimalist (but realistic) Net Core Application, allowing developers to easily interact and learn the framework.

The application uses DTF `AzureStorage` as its backend/persistence mechanism and .Net `DI` to manage `IOC`.

## List of orchestrations

| Name                       | Description | Arguments - separated by ; |
| -------------------------- | ----------- | ----------| 
| RetrieveFilmsOrchestration | Uses the [StarWarsAPI](https://swapi.dev/) to retrieve a list of films where the supplied Star Wars character appears, the results are saved in the file system | $CHARACTER_NAME;$FILE_NAME;$DELAY


## Compiling the code

Clone the repo and build the solution using the CLI:

```bash
$ dotnet build
```

Use your favorite IDE.

## Running the applications

The code base provides a `server` (TaskWorker) and a `client`.

An [Azure Storage Account](https://learn.microsoft.com/en-us/azure/storage/common/storage-account-overview) is required, create one via the Azure Portal and retrieve its connection string.

You can them either change the `appsettings.json` or setup your IDE/terminal with environment variables.

```bash
$ export DOTNET_ENVIRONMENT=development
$ export DTF__StorageAccountConnectionString="CONNECTION_STRING"
```

### Running the server

Launch the server with the following command:

```bash
$ dotnet run server
```

Logs will be displayed by default.

### Running the client

Orchestrations can be scheduled using the syntax: `client --orchestration $NAME --argument $ARGS`, for example:

```bash
$ dotnet run client --orchestration RetrieveFilmsOrchestration --argument "Luke;films-luke.json"
```

## Discover and learn

- Try running multiple instances of the `server`;
- Kill the server during different occasions (while it is processing a task or the orchestration's control flow);
- Disconnect from the internet;
- Explore the code base;
- Use [Azure Storage Explorer](https://azure.microsoft.com/en-gb/products/storage/storage-explorer/) to view
- Create a new Orchestration;
- Keep hacking :)

