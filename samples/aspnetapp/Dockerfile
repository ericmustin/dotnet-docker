FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY aspnetapp/*.csproj ./aspnetapp/
RUN dotnet restore

# copy everything else and build app
COPY aspnetapp/. ./aspnetapp/
WORKDIR /app/aspnetapp
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/aspnetapp/out ./


RUN apt-get update && apt-get install curl && apt-get install tar && apt-get install bash

RUN mkdir -p /opt/datadog
RUN mkdir -p /var/log/datadog

RUN curl -L https://github.com/DataDog/dd-trace-dotnet/releases/download/v1.11.0/datadog-dotnet-apm-1.11.0.tar.gz \
| tar xzf - -C /opt/datadog

ENV DD_TRACE_DEBUG=true
ENV DD_AGENT_HOST=datadog-agent
ENV DD_TRACE_AGENT_PORT=8126
ENV CORECLR_ENABLE_PROFILING=1
ENV CORECLR_PROFILER={846F5F1C-F9AE-4B07-969E-05C26BC060D8}
ENV CORECLR_PROFILER_PATH=/opt/datadog/Datadog.Trace.ClrProfiler.Native.so
ENV DD_INTEGRATIONS=/opt/datadog/integrations.json
ENV DD_DOTNET_TRACER_HOME=/opt/datadog


ENTRYPOINT ["dotnet", "aspnetapp.dll"]
