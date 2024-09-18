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
kubectl apply -f k8s/deployment.yml
kubectl apply -f k8s/gateway.yml
kubectl apply -f k8s/destination-rule.yml
kubectl apply -f k8s/virtual-service.yml
kubectl annotate gateway webapi-gateway networking.istio.io/service-type=ClusterIP --namespace=default
kubectl port-forward svc/webapi-gateway-istio 8080:80
```

```bash
while true; do curl http://localhost:8080/version; echo; sleep 0.5; done;
```
