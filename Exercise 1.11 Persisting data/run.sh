#!/usr/bin/env bash

cd "Log output"
docker build -t logout-writer:1.0 .
docker build -t logout-reader:1.0 -f Dockerfile-Reader .

cd ..

cd "PingPong"
docker build -t pingpong:1.0 .


k3d cluster delete ex-111 && k3d cluster create ex-111 --api-port 127.0.0.1:6443 -p 8081:80@loadbalancer --agents 1

# docker exec k3d-ex-111-agent-0 mkdir -p /tmp/kube
# docker exec k3d-ex-111-agent-1 mkdir -p /tmp/kube

k3d image import logout-writer:1.0  --cluster ex-111 && k3d image import logout-reader:1.0  --cluster ex-111 && k3d image import pingpong:1.0 --cluster ex-111

# winpty is required to when running using git bash from windows environment
# tty is not recongnized error is given otherwise
winpty docker exec -it k3d-ex-111-agent-0 sh -c "mkdir -p /tmp/kube && exit"
winpty docker exec -it k3d-ex-111-server-0 sh -c "mkdir -p /tmp/kube && exit"

# winpty docker exec -it k3d-ex-111-agent-1 sh -c "mkdir -p /tmp/kube && exit"

cd ..

kubectl apply -f ./manifests/volume/pv.yaml
kubectl apply -f ./manifests/pvc/pvc-logger.yaml
kubectl apply -f ./manifests/deployment/deployment1.yaml
kubectl apply -f ./manifests/deployment/deployment2.yaml
kubectl apply -f ./manifests/deployment/service1.yaml
kubectl apply -f ./manifests/deployment/service2.yaml
kubectl apply -f ./manifests/deployment/ingress.yaml
