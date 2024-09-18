# poc-kubernetes-service-mesh

Criação de uma prova de conceito para realizar canary e a/b deploy

## Criação das Imagens

Modifique a versão na aplicação antes de gerar as imagens versionadas:

```json
// src/WebApi/appsettings.json

{
  "AppVersion": "v2"
}
```

Compile e envie as imagens versionadas ao dockerhub:

```bash
export IMAGE_TAG=v2
docker build -t tfsantosbr/webapi:$IMAGE_TAG ./src/WebApi
docker push tfsantosbr/webapi:$IMAGE_TAG
```

## Deploy no Kubernetes

Configure as imagens dos containers em ambos os Deployments

```yml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: webapi-v1
  template:
    ...
    spec:
      containers:
        - name: webapi
          image: tfsantosbr/webapi:v1 # trocar a imagem versionada aqui
---
apiVersion: apps/v1
kind: Deployment
metadata:
  name: webapi-v2
  template:
    ...
    spec:
      containers:
        - name: webapi
          image: tfsantosbr/webapi:v2 # trocar a imagem versionada aqui
---
```

Faça o deploy dos manifestos `.yml` para o Kubernetes:

```bash
# aplicando os manifestos
kubectl apply -f k8s/namespace.yml
kubectl apply -f k8s/

# checar os manifestos criados
kubectl get all -n service-mesh
```

## Test on K8S (sem gateway)

Entre em um dos pods (de preferencia o pod do nginx):

```bash
kubectl exec -it nginx -c nginx -n service-mesh -- bash
```

Execute o comando `curl` e veja a resposta de acordo com a configuração do `virtual-service`:

```bash
while true; do curl http://webapi/version; echo; sleep 0.5; done;
```

## Limpar todos os recursos

Para limpar todos os recursos instalados e remover o namespace, execute o comando abaixo:

```bash
kubectl delete namespace service-mesh
```
