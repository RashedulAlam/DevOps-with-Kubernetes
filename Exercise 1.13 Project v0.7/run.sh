#!/usr/bin/env bash

cd "TodoList"
docker build -t todolist:1.0 .

cd ..


k3d cluster delete ex-113 && k3d cluster create ex-113 --api-port 127.0.0.1:6443 -p 8081:80@loadbalancer --agents 1

k3d image import todolist:1.0  --cluster ex-113 

# winpty is required to when running using git bash from windows environment
# tty is not recongnized error is given otherwise
winpty docker exec -it k3d-ex-113-agent-0 sh -c "mkdir -p /tmp/kube && exit"
# winpty docker exec -it k3d-ex-112-server-0 sh -c "mkdir -p /tmp/kube && exit"

# winpty docker exec -it k3d-ex-111-agent-1 sh -c "mkdir -p /tmp/kube && exit"

kubectl apply -f ./manifests/volume/pv.yaml
kubectl apply -f ./manifests/pvc/pvc-logger.yaml
kubectl apply -f ./manifests/deployment/deployment.yaml
kubectl apply -f ./manifests/deployment/service.yaml
kubectl apply -f ./manifests/deployment/ingress.yaml
