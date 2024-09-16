#!/usr/bin/env bash

cd "TodoApp"
docker build -t todoapp:1.0 .

cd ..

cd "TodoService"
docker build -t todoservice:1.0 .

cd ..

k3d cluster delete ex-202 && k3d cluster create ex-202 --api-port 127.0.0.1:6443 -p 8081:80@loadbalancer --agents 1

k3d image import todoapp:1.0 --cluster ex-202 && k3d image import todoservice:1.0  --cluster ex-202

winpty docker exec -it k3d-ex-202-agent-0 sh -c "mkdir -p /tmp/kube && exit"

kubectl apply -f ./manifests/volume/pv.yaml
kubectl apply -f ./manifests/pvc/pvc-logger.yaml
kubectl apply -f ./manifests/deployment/deployment.yaml
kubectl apply -f ./manifests/deployment/service.yaml
kubectl apply -f ./manifests/deployment/ingress.yaml

