
FROM microsoft/dotnet:3.0-sdk
WORKDIR /app
COPY . ./
RUN dotnet build
EXPOSE 5000/tcp
CMD ["dotnet", "run", "--project", "CryptoTechProject"]
