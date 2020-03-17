FROM mcr.azk8s.cn/dotnet/core/runtime:3.1

# dotnet publish -r linux-x64 -c Release -o publish
# docker build . -t jijiechen/hello-idcf:v1

COPY publish /app

WORKDIR /app
EXPOSE 80
ENTRYPOINT ["dotnet", "hello-idcf.dll"]

