# poc-kubernetes-service-mesh

Criação de uma prova de conceito para realizar canary e a/b deploy

## Deploy on K8S

```bash
export IMAGE_TAG=v2
docker build -t tfsantosbr/webapi:$IMAGE_TAG ./src/WebApi
docker push tfsantosbr/webapi:$IMAGE_TAG
envsubst < k8s/deployment.yml | kubectl apply -f -

```

## Test on K8S

```bash
kubectl port-forward service/webapi-service 8000:80
```
