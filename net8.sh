rm -rf bin obj && dotnet restore SRE-net8.csproj && dotnet run --project SRE-net8.csproj -f net8.0-ios -c Release -p:ArchiveOnBuild=true -r:ios-arm64 /p:_DeviceName=$1 /p:EnableAssemblyILStripping=false -v:n