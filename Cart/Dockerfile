FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY bin/Release/net6.0/publish/ .
ENTRYPOINT ["dotnet", "Cart.dll"]
# > dotnet publish -c Release
# > docker build -t cart:1 .
# > docker run -d -p 9000:80 --name cart -d --net netECom cart:1