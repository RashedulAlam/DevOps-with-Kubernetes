#!/usr/bin/env bash

cd ..
cd "Exercise 1.01/RandomStringGenerator"
docker build -t rsg .

k3d cluster delete ex-103 && k3d cluster create ex-103 --api-port 127.0.0.1:6443
k3d image import rsg --cluster ex-103

cd ..
kubectl apply -f manifests/deployment.yaml
