# Giai đoạn 1: Build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["LevelUp.csproj", "LevelUp/"]

RUN dotnet restore "LevelUp/LevelUp.csproj"

COPY . .
WORKDIR "/src/LevelUp"

RUN dotnet build "LevelUp.csproj" -c Release -o /app/build


# Giai đoạn 2: Publish (đóng gói ứng dụng đã build)
FROM build AS publish
RUN dotnet publish "LevelUp.csproj" -c Release -o /app/publish /p:UseAppHost=false


# Giai đoạn 3: Tạo final image (hình ảnh cuối cùng để chạy)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080 # Port mà container sẽ lắng nghe
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LevelUp.dll"]
