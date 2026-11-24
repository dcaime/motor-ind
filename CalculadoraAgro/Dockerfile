#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.22 AS build
RUN apk update && apk add --no-cache curl zip 
WORKDIR /src
COPY . .

RUN dotnet restore "CalculadoraAgro/CalculadoraAgro.csproj"

WORKDIR "/src/CalculadoraAgro"
RUN dotnet build "CalculadoraAgro.csproj" -c Release -o /app/build

FROM build AS publish

ARG NEXUS_USERNAME
ARG NEXUS_PASSWORD
ARG NEXUS_UPLOAD_URL
RUN dotnet publish "CalculadoraAgro.csproj" -c Release -o /app/publish /p:UseAppHost=false

WORKDIR "/app/publish"

# Ejecuta el comando zip para comprimir la carpeta de archivos
RUN zip -r publish.zip .
RUN ls -l /app/publish

# Upload .zip to Nexus
RUN curl --fail -u "${NEXUS_USERNAME}:${NEXUS_PASSWORD}" --upload-file "publish.zip" "${NEXUS_UPLOAD_URL}"

