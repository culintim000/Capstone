FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY bin/Release/net6.0/publish/ .
ENTRYPOINT ["dotnet", "Inventory.dll"]
# > dotnet publish -c Release
# > docker build -t inventory:1 .
# > docker run -d -p 9002:80 --name inventory -d --net netECom inventory:1