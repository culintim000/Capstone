FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY bin/Release/net6.0/publish/ .
ENTRYPOINT ["dotnet", "Checkout.dll"]
# > dotnet publish -c Release
# > docker build -t checkout:1 .
# > docker run -d -p 9001:80 --name checkout -d --net netECom checkout:1