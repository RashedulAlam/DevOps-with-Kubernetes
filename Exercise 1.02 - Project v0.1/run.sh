#!/usr/bin/env bash

cd "Project v0.1"
docker build -t netwebapp .

k3d cluster delete ex-102-v1 && k3d cluster create ex-102-v1 --api-port 127.0.0.1:6443
k3d image import netwebapp --cluster ex-102-v1
kubectl create deployment netwebapp --image=netwebapp
