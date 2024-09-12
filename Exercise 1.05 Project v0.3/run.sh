#!/usr/bin/env bash

cd ..
cd "Exercise 1.05 Project v0.3/simple-application"
docker build -t angularapp:1.0 .

k3d cluster delete ex-105 && k3d cluster create ex-105 --api-port 127.0.0.1:6443
k3d image import angularapp:1.0 --cluster ex-105

cd ..
kubectl apply -f manifests/deployment.yaml
