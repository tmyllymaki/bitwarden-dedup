Small tool to remove duplicate folders and items from an unencrypted Bitwarden vault JSON export.

Use with caution, this tool is not tested and may delete/modify items you don't want to delete.

Worked for me..

## Getting Started

First clone the project locally:

```shell
git clone 
cd bitwarden-dedup
```

Export your BitWarden vault to an unencrypted JSON file

### Local dotnet cli

Just run with dotnet cli passing the exported file as a parameter:

```shell
dotnet run -- path-to-exported-json-file
```

### Docker

Build the container:

```shell
docker build -t bitwarden-dedup .
```

Make directory for the export and the output and mount it in the container. Remember to pass the path to the export file
as a parameter:

```shell
docker run --rm -v /path/to/exported/json:/export bitwarden-dedup /export/bitwarden_export.json
```

## License

Distributed under the MIT License. See `LICENSE.txt` for more information.