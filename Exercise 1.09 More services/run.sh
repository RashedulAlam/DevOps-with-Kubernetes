#!/usr/bin/env bash

cd "PingPong"
docker build -t pingpong:1.0 .

cd ..

k3d cluster delete ex-109 && k3d cluster create ex-109 --api-port 127.0.0.1:6443 --port 8082:30080@agent:0 -p 8081:80@loadbalancer --agents 2

k3d image import pingpong:1.0  --cluster ex-109 && k3d image import logoutput:1.0  --cluster ex-109

kubectl apply -f ./manifests/deployment1.yaml
kubectl apply -f ./manifests/deployment2.yaml
kubectl apply -f ./manifests/service1.yaml
kubectl apply -f ./manifests/service2.yaml
kubectl apply -f ./manifests/ingress.yaml
