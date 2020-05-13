# ASP.NET Core Docker Sample

## Build and run the sample with Docker (with Datadog Agent)

To build and run the sample in docker, and generate Traces collected by the Datadog Agent

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

Finally, start your application container on the same network as the agent is running on. In the Dockerfile for this application you'll notice we've enabled pointed the the .NET Tracer to send traces to our agent running in the container `datadog-agent` (ENV DD_AGENT_HOST=datadog-agent) 

```console
docker run -d --name aspnetcore_sample --network dotnetconnect --rm -it -p 8000:80 aspnetapp
```

After the application starts, navigate to `http://localhost:8000` in your web browser.

Visit `http://localhost:8000/home/privacy` to generate Traces of the http outgoing requests. 


