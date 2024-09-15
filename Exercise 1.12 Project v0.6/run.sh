#!/usr/bin/env bash

cd "ImageFinder"
docker build -t imagefinder:1.0 .

cd ..


k3d cluster delete ex-112 && k3d cluster create ex-112 --api-port 127.0.0.1:6443 -p 8081:80@loadbalancer --agents 1

k3d image import imagefinder:1.0  --cluster ex-112 

# winpty is required to when running using git bash from windows environment
# tty is not recongnized error is given otherwise
winpty docker exec -it k3d-ex-112-agent-0 sh -c "mkdir -p /tmp/kube && exit"
# winpty docker exec -it k3d-ex-112-server-0 sh -c "mkdir -p /tmp/kube && exit"

# winpty docker exec -it k3d-ex-111-agent-1 sh -c "mkdir -p /tmp/kube && exit"

kubectl apply -f ./manifests/volume/pv.yaml
kubectl apply -f ./manifests/pvc/pvc-logger.yaml
kubectl apply -f ./manifests/deployment/deployment.yaml
kubectl apply -f ./manifests/service/service.yaml
kubectl apply -f ./manifests/ingress.yaml
