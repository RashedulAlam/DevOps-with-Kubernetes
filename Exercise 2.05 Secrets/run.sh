#!/usr/bin/env bash

# encrpyt the secret file
./encrypt-secrets.sh

# set the secret key
export SOPS_AGE_KEY_FILE=$(pwd)/secret.txt

# create the cluster
k3d cluster delete ex-205 && k3d cluster create ex-205 --api-port 127.0.0.1:6443 -p 8081:80@loadbalancer --agents 1


kubectl apply -f ./manifests/namespace/namespace.yaml
# apply the decrypted file
sops --decrypt manifests/secret/secret.enc.yaml | kubectl apply -f -

kubectl get secrets -n ex-205