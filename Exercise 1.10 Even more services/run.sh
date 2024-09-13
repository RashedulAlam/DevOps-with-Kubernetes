#!/usr/bin/env bash

cd "Log output"
docker build -t logout-writer:1.0 .
docker build -t logout-reader:1.0 -f Dockerfile-Reader .

cd ..

k3d cluster delete ex-110 && k3d cluster create ex-110 --api-port 127.0.0.1:6443 --port 8082:30080@agent:0 -p 8081:80@loadbalancer --agents 2

k3d image import logout-writer:1.0  --cluster ex-110 && k3d image import logout-reader:1.0  --cluster ex-110

kubectl apply -f ./manifests/deployment1.yaml
kubectl apply -f ./manifests/service1.yaml
kubectl apply -f ./manifests/ingress.yaml
