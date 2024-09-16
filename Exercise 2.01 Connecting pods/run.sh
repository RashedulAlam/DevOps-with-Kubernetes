#!/usr/bin/env bash

cd "Log output"
docker build -t logout-writer:2.0 .
docker build -t logout-reader:2.0 -f Dockerfile-Reader .

cd ..

cd "PingPong"
docker build -t pingpong:2.0 .

cd ..

k3d cluster delete ex-201 && k3d cluster create ex-201 --api-port 127.0.0.1:6443 -p 8081:80@loadbalancer --agents 1

k3d image import logout-writer:2.0  --cluster ex-201 && k3d image import logout-reader:2.0  --cluster ex-201 && k3d image import pingpong:2.0 --cluster ex-201


kubectl apply -f ./manifests/deployment/deployment1.yaml
kubectl apply -f ./manifests/deployment/deployment2.yaml
kubectl apply -f ./manifests/deployment/service1.yaml
kubectl apply -f ./manifests/deployment/service2.yaml
kubectl apply -f ./manifests/deployment/ingress.yaml
