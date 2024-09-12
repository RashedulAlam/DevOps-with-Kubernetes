#!/usr/bin/env bash

cd ..
cd "Exercise 1.02 - Project v0.1/Project v0.1"
docker build -t netwebapp:1.1.1 .

k3d cluster delete ex-104 && k3d cluster create ex-104 --api-port 127.0.0.1:6443
k3d image import netwebapp:1.1.1 --cluster ex-104

cd ..
kubectl apply -f manifests/deployment.yaml
