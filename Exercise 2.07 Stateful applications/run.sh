#!/usr/bin/env bash

cd "PingPong"
docker build -t pingpong:4.0 .

cd ..

# encrpyt the secret file
./encrypt-secrets.sh

# set the secret key
export SOPS_AGE_KEY_FILE=$(pwd)/secret.txt

# create the cluster
k3d cluster delete ex-207 && k3d cluster create ex-207 --api-port 127.0.0.1:6443 -p 8081:80@loadbalancer --agents 1

k3d image import pingpong:4.0 --cluster ex-207

kubectl apply -f ./manifests/namespace/namespace.yaml
# apply the decrypted file
sops --decrypt manifests/secret/secret.enc.yaml | kubectl apply -f -

kubectl apply -f ./manifests/configMaps/configMap.yaml
kubectl apply -f ./manifests/deployment/deployment.yaml
kubectl apply -f ./manifests/service/service.yaml
kubectl apply -f ./manifests/service/database.yaml
kubectl apply -f ./manifests/statefulSet/database.yaml
kubectl apply -f ./manifests/ingress/ingress.yaml

kubectl get all -n ex-207