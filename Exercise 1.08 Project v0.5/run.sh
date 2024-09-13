#!/usr/bin/env bash

k3d cluster delete ex-108 && k3d cluster create ex-108 --api-port 127.0.0.1:6443 --port 8082:30080@agent:0 -p 8081:80@loadbalancer --agents 2

k3d image import angularapp:1.0 --cluster ex-108

kubectl apply -f ./manifests/deployment.yaml
kubectl apply -f ./manifests/service.yaml
kubectl apply -f ./manifests/ingress.yaml