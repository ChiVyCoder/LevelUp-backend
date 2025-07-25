﻿# Giai đoạn 1: Build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY LevelUp.csproj . 

RUN dotnet restore LevelUp.csproj # Restore gói cho .csproj

COPY . . 

# Chuyển thư mục làm việc về /src (vì LevelUp.csproj nằm ở gốc của /src)
WORKDIR /src

# Build dự án
RUN dotnet build LevelUp.csproj -c Release -o /app/build

# Giai đoạn 2: Publish (đóng gói ứng dụng đã build)
FROM build AS publish
# Lệnh publish này được chạy từ WORKDIR hiện tại (/src)
RUN dotnet publish LevelUp.csproj -c Release -o /app/publish /p:UseAppHost=false

# Giai đoạn 3: Tạo final image (hình ảnh cuối cùng để chạy)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080 
COPY --from=publish /app/publish . 
ENTRYPOINT ["dotnet", "LevelUp.dll"] # Tên file DLL phải đúng LevelUp.dll (tên dự án của bạn)
