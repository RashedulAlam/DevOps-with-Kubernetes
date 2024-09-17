#!/usr/bin/env bash

# create the cluster
k3d cluster delete ex-206 && k3d cluster create ex-206 --api-port 127.0.0.1:6443 -p 8081:80@loadbalancer --agents 1




kubectl apply -f ./manifests/namespace/namespace.yaml

kubectl get all -n ex-206