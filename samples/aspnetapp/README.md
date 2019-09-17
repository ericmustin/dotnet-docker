# ASP.NET Core Docker Sample

This [sample Dockerfile](Dockerfile) demonstrates how to use ASP.NET Core and Docker together. The sample works with both Linux and Windows containers and can also be used without Docker. There are also instructions that demonstrate how to push the sample to [Azure Container Registry](../dotnetapp/push-image-to-acr.md) and test it with [Azure Container Instance](deploy-container-to-aci.md). You can [configure ASP.NET Core to use HTTPS with Docker](aspnetcore-docker-https.md).

The sample builds the application in a container based on the larger [.NET Core SDK Docker image](https://hub.docker.com/_/microsoft-dotnet-core-sdk/). It builds the application and then copies the final build result into a Docker image based on the smaller [ASP.NET Core Docker Runtime image](https://hub.docker.com/_/microsoft-dotnet-core-aspnet/).

This sample requires [Docker 17.06](https://docs.docker.com/release-notes/docker-ce) or later of the [Docker client](https://www.docker.com/products/docker).

## Try a pre-built ASP.NET Core Docker Image

You can quickly run a container with a pre-built [sample ASP.NET Core Docker image](https://hub.docker.com/_/microsoft-dotnet-core-samples/), based on this [sample](Dockerfile).

Type the following command to run a sample with [Docker](https://www.docker.com/products/docker):

```console
docker run --name aspnetcore_sample --rm -it -p 8000:80 mcr.microsoft.com/dotnet/core/samples:aspnetapp
```

After the application starts, navigate to `http://localhost:8000` in your web browser. On Windows, you may need to navigate to the container via IP address. See [ASP.NET Core apps in Windows Containers](aspnetcore-docker-windows.md) for instructions on determining the IP address, using the value of `--name` that you used in `docker run`.

See [Hosting ASP.NET Core Images with Docker over HTTPS](aspnetcore-docker-https.md) to use HTTPS with this image.

## Getting the sample

The easiest way to get the sample is by cloning the samples repository with git, using the following instructions:

```console
git clone https://github.com/dotnet/dotnet-docker/
```

You can also [download the repository as a zip](https://github.com/dotnet/dotnet-docker/archive/master.zip).

## Build and run the sample with Docker (no datadog agent)

You can build and run the sample in Docker using the following commands. The instructions assume that you are in the root of the repository.

```console
cd samples
cd aspnetapp
docker build --pull -t aspnetapp .
docker run --name aspnetcore_sample --rm -it -p 8000:80 aspnetapp
```

You should see the following console output as the application starts.

```console
C:\git\dotnet-docker\samples\aspnetapp>docker run --name aspnetcore_sample --rm -it -p 8000:80 aspnetapp
Hosting environment: Production
Content root path: /app
Now listening on: http://[::]:80
Application started. Press Ctrl+C to shut down.
```

After the application starts, navigate to `http://localhost:8000` in your web browser. On Windows, you may need to navigate to the container via IP address. See [ASP.NET Core apps in Windows Containers](aspnetcore-docker-windows.md) for instructions on determining the IP address, using the value of `--name` that you used in `docker run`.

> Note: The `-p` argument maps port 8000 on your local machine to port 80 in the container (the form of the port mapping is `host:container`). See the [Docker run reference](https://docs.docker.com/engine/reference/commandline/run/) for more information on commandline parameters. In some cases, you might see an error because the host port you select is already in use. Choose a different port in that case.

## Build and run the sample with Docker (with Datadog Agent)

To build and run the sample in docker, Correlated Logs and Traces collected by the Datadog Agent

```console
cd samples
cd aspnetapp
docker build --pull -t aspnetapp .
```

Next, create a docker network

```console
docker network create dotnetconnect
```

Then, start the datadog container agent. Be sure to replace the below <YOUR_API_KEY> with your Datadog API Key. You'll notice we have the datadog-agent running on the network we just created

```console
# Datadog Agent
docker run -d --name datadog-agent \
              --network dotnetconnect \
              -v /var/run/docker.sock:/var/run/docker.sock:ro \
              -v /proc/:/host/proc/:ro \
              -v /sys/fs/cgroup/:/host/sys/fs/cgroup:ro \
              -e DD_API_KEY=<YOUR_API_KEY> \
              -e DD_APM_ENABLED=true \
              -e DD_APM_NON_LOCAL_TRAFFIC=true \
              -e DD_LOGS_ENABLED=true \
              -e DD_LOGS_CONFIG_CONTAINER_COLLECT_ALL=true \
              -e DD_AC_EXCLUDE="name:datadog-agent" \
              datadog/agent:latest
```

Finally, start your application container on the same network as the agent is running on. In the Dockerfile for this application you'll notice we've enabled automated log injection(ENV DD_LOGS_INJECTION=true) as well as pointed the the .NET Tracer to send traces to our agent running in the container `datadog-agent` (ENV DD_AGENT_HOST=datadog-agent) 

```console
docker run -d --name aspnetcore_sample --network dotnetconnect --rm -it -p 8000:80 aspnetapp
```

After the application starts, navigate to `http://localhost:8000` in your web browser.

Visit `http://localhost:8000/home/about` , `http://localhost:8000/home/contact` , or `http://localhost:8000/home/privacy` to generate Traces and Logs correlated to them. 

You can view these logs injected with trace_id and span_id from your host by running `docker logs aspnetcore_sample`
