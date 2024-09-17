#!/usr/bin/env bash

cd "Log output"
docker build -t logout-writer:2.0 .
docker build -t logout-reader:2.0 -f Dockerfile-Reader .

cd ..

cd "PingPong"
docker build -t pingpong:2.0 .

cd ..

k3d cluster delete ex-203 && k3d cluster create ex-203 --api-port 127.0.0.1:6443 -p 8081:80@loadbalancer --agents 1

k3d image import logout-writer:2.0  --cluster ex-203 && k3d image import logout-reader:2.0  --cluster ex-203 && k3d image import pingpong:2.0 --cluster ex-203

kubectl apply -f ./manifests/namespace/namespace.yaml
kubectl apply -f ./manifests/deployment/deployment1.yaml
kubectl apply -f ./manifests/deployment/deployment2.yaml
kubectl apply -f ./manifests/service/service1.yaml
kubectl apply -f ./manifests/service/service2.yaml
kubectl apply -f ./manifests/ingress/ingress.yaml

kubectl get all --namespace ex-203