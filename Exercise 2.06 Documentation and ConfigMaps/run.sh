#!/usr/bin/env bash

cd "Log output"
docker build -t logout-writer:3.0 .
docker build -t logout-reader:3.0 -f Dockerfile-Reader .

cd ..

cd "PingPong"
docker build -t pingpong:3.0 .

cd ..

k3d cluster delete ex-206 && k3d cluster create ex-206 --api-port 127.0.0.1:6443 -p 8081:80@loadbalancer --agents 1

k3d image import logout-writer:3.0  --cluster ex-206 && k3d image import logout-reader:3.0  --cluster ex-206 && k3d image import pingpong:3.0 --cluster ex-206

kubectl apply -f ./manifests/namespace/namespace.yaml
kubectl apply -f ./manifests/configMaps/configMap.yaml
kubectl apply -f ./manifests/deployment/deployment1.yaml
kubectl apply -f ./manifests/deployment/deployment2.yaml
kubectl apply -f ./manifests/service/service1.yaml
kubectl apply -f ./manifests/service/service2.yaml
kubectl apply -f ./manifests/ingress/ingress.yaml

kubectl get all --namespace ex-206